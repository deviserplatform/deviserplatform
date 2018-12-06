using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Deviser.Admin.Attributes;
using System.Collections;
using Deviser.Admin.Data;
using System.Threading;

namespace Deviser.Admin.Config
{
    public class AdminSite : IAdminSite
    {
        public const string IEnumerableOfIFormFileName = "IEnumerable`" + nameof(IFormFile);

        private static readonly string _defaultDateFormat = $"{Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern} HH:mm:ss";
        private readonly DbContext _dbContext;
        private readonly Type _dbContextType;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        // Mapping from datatype names and data annotation hints to values for the <input/> element's "type" attribute.
        private static readonly Dictionary<string, FieldType> _defaultInputTypes =
            new Dictionary<string, FieldType>(StringComparer.OrdinalIgnoreCase)
            {
                { "HiddenInput", FieldType.Hidden },
                { "Password", FieldType.Password },
                { "Text", FieldType.TextBox },
                { "PhoneNumber", FieldType.Phone },
                { "Url", FieldType.TextBox },
                { "EmailAddress", FieldType.EmailAddress },
                { "Date", FieldType.Date },
                { "DateTime", FieldType.DateTime },
                { "DateTime-local", FieldType.DateTime},
                { "Time", FieldType.Time },
                { nameof(Byte), FieldType.Number },
                { nameof(SByte), FieldType.Number },
                { nameof(Int16), FieldType.Number },
                { nameof(UInt16), FieldType.Number },
                { nameof(Int32), FieldType.Number },
                { nameof(UInt32), FieldType.Number },
                { nameof(Int64), FieldType.Number },
                { nameof(UInt64), FieldType.Number },
                { nameof(Single), FieldType.Number },
                { nameof(Double), FieldType.Number },
                { nameof(Boolean), FieldType.CheckBox },
                { nameof(Decimal), FieldType.Number },
                { nameof(String), FieldType.TextBox },
                { nameof(IFormFile), FieldType.FileAttachment }
            };

        // Mapping from <input/> element's type to RFC 3339 date and time formats.
        private static readonly Dictionary<string, string> _rfc3339Formats =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                { "date", "{0:yyyy-MM-dd}" },
                { "datetime", "{0:yyyy-MM-ddTHH:mm:ss.fffK}" },
                { "datetime-local", "{0:yyyy-MM-ddTHH:mm:ss.fff}" },
                { "time", "{0:HH:mm:ss.fff}" },
            };

        public string SiteName { get; set; }
        public Type DbContextType => _dbContextType;
        public IDictionary<Type, IAdminConfig> AdminConfigs { get; }


        public AdminSite(DbContext dbContext, IModelMetadataProvider modelMetadataProvider)
        {
            if (dbContext == null)
                throw new ArgumentNullException("Constructor paramater dbContent cannot be null");

            _dbContext = dbContext;
            _modelMetadataProvider = modelMetadataProvider;
            _dbContextType = _dbContext.GetType();
            AdminConfigs = new Dictionary<Type, IAdminConfig>();
        }

        public void Register<TEntity>(Action<AdminConfig<TEntity>> adminConfigAction = null) where TEntity : class
        {
            var entityClrType = typeof(TEntity);
            var entityType = _dbContext.Model.FindEntityType(entityClrType);

            if (entityType == null)
                throw new InvalidOperationException($"The entity type {entityClrType} cannot be found on this context {_dbContext}");

            if (AdminConfigs.ContainsKey(entityClrType))
                throw new InvalidOperationException($"The entity type {entityClrType} has already been registered for this admin site, duplicate site registrations are not allowed");

            var adminConfig = new AdminConfig<TEntity>();

            AdminConfigs.Add(entityClrType, adminConfig);

            if (adminConfigAction == null)
            {
                //Register by default settings and fields
                PopulateFields(entityClrType, entityType, adminConfig);
            }
            else
            {
                adminConfigAction(adminConfig);
            }

            PopulateEntityConfig(entityType, adminConfig);

            var pk = adminConfig.EntityConfig.PrimaryKey;
            foreach (var prop in pk.Properties)
            {
                var field = new Field
                {
                    FieldExpression = GetFieldExpression(entityClrType, prop),
                    FieldType = FieldType.KeyField
                };
                adminConfig.KeyFields.Add(field);
            }

            bool hasExlcludeFields = adminConfig?.FieldConfig?.ExcludedFields?.Count > 0;
            bool hasListFields = adminConfig?.ListConfig.Fields.Count > 0;


            if (hasExlcludeFields)
            {
                PopulateFields(entityClrType, entityType, adminConfig, adminConfig.FieldConfig.ExcludedFields);
            }
            else if (adminConfig?.FieldConfig?.Fields?.Count > 0)
            {
                foreach (var fieldRow in adminConfig.FieldConfig.Fields)
                {
                    foreach (var field in fieldRow)
                    {
                        PopulateFieldOptions(field, entityType, adminConfig.FieldConditions);
                    }
                }
            }
            else if (adminConfig?.FieldSetConfig?.FieldSets?.Count > 0)
            {
                foreach (var fieldSet in adminConfig.FieldSetConfig.FieldSets)
                {
                    foreach (var fieldRow in fieldSet.Fields)
                    {
                        foreach (var field in fieldRow)
                        {
                            PopulateFieldOptions(field, entityType, adminConfig.FieldConditions);
                        }
                    }
                }
            }

            if (hasListFields)
            {
                foreach (var field in adminConfig.ListConfig.Fields)
                {
                    PopulateFieldOptions(field, entityType, adminConfig.FieldConditions);
                }
            }
            else
            {
                var properties = GetProperties(entityType);
                foreach (var prop in properties)
                {
                    var field = new Field
                    {
                        FieldExpression = GetFieldExpression(entityClrType, prop)
                    };
                    PopulateFieldOptions(field, entityType, adminConfig.FieldConditions);
                    adminConfig.ListConfig.Fields.Add(field);
                }
            }
        }






        private void PopulateFields<TEntity>(Type entityClrType, IEntityType entityType, AdminConfig<TEntity> adminConfig, List<Field> excludeField = null) where TEntity : class
        {

            var properties = GetProperties(entityType);
            foreach (var prop in properties)
            {
                bool isExclude = (excludeField != null && excludeField.Any(f => f.FieldClrType == prop.ClrType));

                if (!isExclude)
                {
                    adminConfig.FieldConfig.Fields.Add(new List<Field>
                    {
                        new Field
                        {
                            FieldExpression = GetFieldExpression(entityClrType, prop)
                        }
                    });
                }
            }
        }

        private List<IProperty> GetProperties(IEntityType entityType)
        {
            return entityType
                .GetProperties()
                .OrderBy(p => GetOrder(p))
                .ToList();
        }

        private int GetOrder(IProperty property)
        {
            var attr = property.PropertyInfo.GetCustomAttributes(typeof(OrderAttribute), false);
            return attr != null && attr.Length > 0 ? ((OrderAttribute)attr.Single()).Order : 0;
        }

        /// <summary>
        /// Returns the FieldExpression for the given parameters. E.g. e => e.Name
        /// </summary>
        /// <param name="entityClrType"></param>
        /// <param name="efProperty"></param>
        /// <returns>Expression<Func<TEntity, TKey>> as LambdaExpression</returns>
        private LambdaExpression GetFieldExpression(Type entityClrType, IProperty efProperty)
        {
            //ParameterExpression param = Expression.Parameter(entityClrType);
            //MemberExpression memberExpression = Expression.Property(param, efProperty.PropertyInfo);            
            //var fieldExpression = Expression.Lambda(memberExpression, param);

            var fieldExpression = ExpressionHelper.GetPropertyExpression(entityClrType, efProperty.PropertyInfo);
            return fieldExpression;
        }

        private void PopulateEntityConfig<TEntity>(IEntityType entityType, AdminConfig<TEntity> adminConfig) where TEntity : class
        {
            adminConfig.EntityConfig.DbContextType = _dbContextType;
            adminConfig.EntityConfig.PrimaryKey = entityType.FindPrimaryKey();
            adminConfig.EntityConfig.ForeignKeys = entityType.GetForeignKeys();
            adminConfig.EntityConfig.Navigations = entityType.GetNavigations();
        }

        private void PopulateFieldOptions(Field field, IEntityType entityType, FieldConditions fieldConditions)
        {
            var attributes = field.FieldClrType.GetTypeInfo().GetCustomAttributes();
            var efProperty = entityType.GetProperties().FirstOrDefault(p => p.ClrType == field.FieldClrType);
            field.FieldOption = new FieldOption();


            var metadata = field.FieldOption.Metadata = _modelMetadataProvider.GetMetadataForType(field.FieldClrType);



            //var displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            //var displayFormatAttribute = attributes.OfType<DisplayFormatAttribute>().FirstOrDefault();
            //var displayNameAttribute = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            var fieldTypeAttribute = attributes.OfType<FieldTypeAttribute>().FirstOrDefault();

            //TODO: Localization in future
            //IStringLocalizer localizer = null;
            //if (_stringLocalizerFactory != null && _localizationOptions.DataAnnotationLocalizerProvider != null)
            //{
            //    localizer = _localizationOptions.DataAnnotationLocalizerProvider(containerType, _stringLocalizerFactory);
            //}

            // DisplayName
            // DisplayAttribute has precedence over DisplayNameAttribute.

            if (string.IsNullOrEmpty(field.FieldOption.DisplayName))
            {
                //if (displayAttribute?.GetName() != null)
                //{
                //    field.FieldOption.DisplayName = displayAttribute.GetName();
                //}
                //else if (displayNameAttribute != null)
                //{
                //    //if (localizer != null &&
                //    //    !string.IsNullOrEmpty(displayNameAttribute.DisplayName))
                //    //{
                //    //    displayMetadata.DisplayName = () => localizer[displayNameAttribute.DisplayName];
                //    //}
                //    //else
                //    //{
                //    field.FieldOption.DisplayName = displayNameAttribute.DisplayName;
                //    //}
                //}
                if (!string.IsNullOrEmpty(metadata.DisplayName))
                {
                    field.FieldOption.DisplayName = metadata.DisplayName;
                }
                else
                {
                    field.FieldOption.DisplayName = field.FieldName;
                }
            }


            if (string.IsNullOrEmpty(field.FieldOption.Format) /*&& displayFormatAttribute != null*/)
            {
                field.FieldOption.Format = metadata.DisplayFormatString; //displayFormatAttribute.DataFormatString;

                if (string.IsNullOrEmpty(field.FieldOption.Format))
                {
                    field.FieldOption.Format = _defaultDateFormat;
                }
            }

            if (string.IsNullOrEmpty(field.FieldOption.Description) /*&& displayAttribute != null*/)
            {
                //if (localizer != null &&
                //    !string.IsNullOrEmpty(displayAttribute.Description) &&
                //    displayAttribute.ResourceType == null)
                //{
                //    displayMetadata.Description = () => localizer[displayAttribute.Description];
                //}
                //else
                //{
                field.FieldOption.Description = metadata.Description; //displayAttribute.GetDescription();
                //}
            }

            // NullDisplayText
            if (string.IsNullOrEmpty(field.FieldOption.NullDisplayText) /*&& displayFormatAttribute != null*/)
            {
                field.FieldOption.NullDisplayText = metadata.NullDisplayText; //displayFormatAttribute.NullDisplayText;
            }

            if (field.FieldOption.MaxLength == 0)
            {
                field.FieldOption.MaxLength = efProperty.GetMaxLength() ?? 0;
            }

            if (field.FieldType == FieldType.Unknown)
            {
                field.FieldType = GetFieldType(field);
            }

            field.FieldOption.IsRequired = metadata.IsRequired || !efProperty.IsColumnNullable();

            //FieldConditions
            var showCondition = fieldConditions?.ShowOnConditions?.FirstOrDefault(f => f.FieldName == field.FieldName);
            if (showCondition != null)
            {
                field.FieldOption.ShowOn = showCondition.ConditionExpression;
            }

            var enableCondition = fieldConditions?.EnableOnConditions?.FirstOrDefault(f => f.FieldName == field.FieldName);
            if (enableCondition != null)
            {
                field.FieldOption.EnableOn = enableCondition.ConditionExpression;
            }

            var validateCondition = fieldConditions?.ValidateOnConditions?.FirstOrDefault(f => f.FieldName == field.FieldName);
            if (validateCondition != null)
            {
                field.FieldOption.ValidateOn = validateCondition.ConditionExpression;
            }

        }

        private FieldType GetFieldType(Field field)
        {
            foreach (var hint in GetFieldTypeHints(field))
            {
                FieldType inputType;
                if (_defaultInputTypes.TryGetValue(hint, out inputType))
                {
                    return inputType;
                }
            }

            //If nothing found return textbox
            return FieldType.TextBox;
        }

        private static IEnumerable<string> GetFieldTypeHints(Field field)
        {
            var metadata = field.FieldOption.Metadata;

            if (!string.IsNullOrEmpty(metadata.TemplateHint))
            {
                yield return metadata.TemplateHint;
            }

            if (!string.IsNullOrEmpty(metadata.DataTypeName))
            {
                yield return metadata.DataTypeName;
            }

            // In most cases, we don't want to search for Nullable<T>. We want to search for T, which should handle
            // both T and Nullable<T>. However we special-case bool? to avoid turning an <input/> into a <select/>.
            var fieldType = metadata.ModelType;
            if (typeof(bool?) != fieldType)
            {
                fieldType = metadata.UnderlyingOrModelType;
            }

            foreach (string typeName in GetTypeNames(metadata, fieldType))
            {
                yield return typeName;
            }
        }

        public static IEnumerable<string> GetTypeNames(ModelMetadata modelMetadata, Type fieldType)
        {
            // Not returning type name here for IEnumerable<IFormFile> since we will be returning
            // a more specific name, IEnumerableOfIFormFileName.
            var fieldTypeInfo = fieldType.GetTypeInfo();

            if (typeof(IEnumerable<IFormFile>) != fieldType)
            {
                yield return fieldType.Name;
            }

            if (fieldType == typeof(string))
            {
                // Nothing more to provide
                yield break;
            }
            else if (!modelMetadata.IsComplexType)
            {
                // IsEnum is false for the Enum class itself
                if (fieldTypeInfo.IsEnum)
                {
                    // Same as fieldType.BaseType.Name in this case
                    yield return "Enum";
                }
                else if (fieldType == typeof(DateTimeOffset))
                {
                    yield return "DateTime";
                }

                yield return "String";
                yield break;
            }
            else if (!fieldTypeInfo.IsInterface)
            {
                var type = fieldType;
                while (true)
                {
                    type = type.GetTypeInfo().BaseType;
                    if (type == null || type == typeof(object))
                    {
                        break;
                    }

                    yield return type.Name;
                }
            }

            if (typeof(IEnumerable).IsAssignableFrom(fieldType))
            {
                if (typeof(IEnumerable<IFormFile>).IsAssignableFrom(fieldType))
                {
                    yield return IEnumerableOfIFormFileName;

                    // Specific name has already been returned, now return the generic name.
                    if (typeof(IEnumerable<IFormFile>) == fieldType)
                    {
                        yield return fieldType.Name;
                    }
                }

                yield return "Collection";
            }
            else if (typeof(IFormFile) != fieldType && typeof(IFormFile).IsAssignableFrom(fieldType))
            {
                yield return nameof(IFormFile);
            }

            yield return "Object";
        }

        private IKey GetPrimaryKey(Type clrType)
        {
            var entityType = _dbContext.Model.FindEntityType(clrType);
            var key = entityType.FindPrimaryKey();
            return key;
        }
    }
}
