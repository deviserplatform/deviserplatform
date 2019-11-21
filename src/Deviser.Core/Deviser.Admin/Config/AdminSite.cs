﻿using System;
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
using Deviser.Core.Common.DomainTypes.Admin;
using Deviser.Core.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace Deviser.Admin.Config
{
    public class AdminSite : IAdminSite
    {
        public const string IEnumerableOfIFormFileName = "IEnumerable`" + nameof(IFormFile);
        private static readonly string _defaultDateFormat = $"{Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern} HH:mm:ss";
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

        private readonly AdminType _adminType;
        private readonly DbContext _dbContext;
        private readonly Type _dbContextType;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly IServiceProvider _serviceProvider;
        private List<TypeMap> _typeMaps;

        public IDictionary<Type, IAdminConfig> AdminConfigs { get; }
        public Type DbContextType => _dbContextType;
        public IMapper Mapper { get; set; }
        public string SiteName { get; set; }

        public AdminSite(IServiceProvider serviceProvider, DbContext dbContext, IModelMetadataProvider modelMetadataProvider)
        {
            _adminType = AdminType.Entity;
            if (dbContext == null)
                throw new ArgumentNullException("Constructor paramater dbContent cannot be null");

            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
            _modelMetadataProvider = modelMetadataProvider;
            _dbContextType = _dbContext.GetType();
            AdminConfigs = new Dictionary<Type, IAdminConfig>();
        }

        public AdminSite(IServiceProvider serviceProvider, IModelMetadataProvider modelMetadataProvider)
        {
            _adminType = AdminType.Custom;
            _serviceProvider = serviceProvider;
            _modelMetadataProvider = modelMetadataProvider;
            AdminConfigs = new Dictionary<Type, IAdminConfig>();
        }

        public void Build<TModel>(AdminConfig<TModel> adminConfig, bool hasConfiguration = false) where TModel : class
        {
            var modelType = typeof(TModel);

            if (_adminType == AdminType.Entity)
            {
                //TODO: Validate adminConfig for entity type

                if (AdminConfigs.ContainsKey(modelType))
                {
                    throw new InvalidOperationException($"The ModelType {modelType} has already been registered for this admin site, duplicate site registrations are not allowed");
                }

                if (Mapper == null)
                {
                    throw new InvalidOperationException($"Automapper configuration is required when creating admin site using AdminType: {_adminType}");
                }

                if (_typeMaps == null)
                {
                    _typeMaps = Mapper.ConfigurationProvider.GetAllTypeMaps().ToList();
                }

                Type entityClrType = GetEntityClrTypeFor(modelType);

                if (entityClrType == null)
                {
                    throw new InvalidOperationException($"The entity type for the ModelType: {modelType} cannot be found in MapperConfiguration, please automapper configuration for ModelType and EntityType");
                }

                var entityType = _dbContext.Model.FindEntityType(entityClrType);

                if (entityType == null)
                {
                    throw new InvalidOperationException($"The entity type for the ModelType: {modelType} cannot be found on this context {_dbContext}");
                }

                adminConfig.EntityConfig = new EntityConfig(_dbContextType, entityType);

                BuildEntityForm(adminConfig, hasConfiguration, modelType);

                if (adminConfig.ChildConfigs != null && adminConfig.ChildConfigs.Count > 0)
                {
                    foreach (var childConfig in adminConfig.ChildConfigs)
                    {
                        var childModelType = childConfig.Field.FieldClrType;

                        Type childEntityClrType = GetEntityClrTypeFor(childModelType);

                        if (childEntityClrType == null)
                        {
                            throw new InvalidOperationException($"The entity type for the ModelType: {childModelType} cannot be found in MapperConfiguration, please automapper configuration for ModelType and EntityType");
                        }

                        var childEntityType = _dbContext.Model.FindEntityType(entityClrType);

                        if (childEntityType == null)
                        {
                            throw new InvalidOperationException($"The entity type for the ModelType: {modelType} cannot be found on this context {_dbContext}");
                        }

                        childConfig.EntityConfig = new EntityConfig(_dbContextType, childEntityType);

                        BuildEntityForm(childConfig, hasConfiguration, childModelType);
                    }
                }

                LoadMasterData(adminConfig);
            }
            else if (_adminType == AdminType.Custom)
            {
                //TODO: Validate adminConfig for custom type
            }

            AdminConfigs.Add(modelType, adminConfig);
        }

        private Type GetEntityClrTypeFor(Type modelType)
        {
            Type entityClrType = _typeMaps.FirstOrDefault(tm => tm.SourceType == modelType)?.DestinationType;
            entityClrType = _typeMaps.FirstOrDefault(tm => tm.DestinationType == modelType)?.SourceType;
            return entityClrType;
        }

        private void BuildEntityForm(IAdminBaseConfig adminBaseConfig, bool hasConfiguration, Type modelType)
        {
            IFormConfig formConfig = adminBaseConfig.FormConfig;
            EntityConfig entityConfig = adminBaseConfig.EntityConfig;
            if (!hasConfiguration)
            {
                //Register by default settings and fields
                PopulateFields(modelType, formConfig);
            }

            //PopulateEntityConfig(entityType, entityConfig);

            //Adding primary keys to KeyField

            bool hasExlcludeFields = formConfig?.FieldConfig?.ExcludedFields?.Count > 0;
            bool hasListFields = formConfig?.ListConfig.Fields.Count > 0;


            if (hasExlcludeFields)
            {
                PopulateFields(modelType, formConfig, formConfig.FieldConfig.ExcludedFields);
            }

            var fields = formConfig.AllFormFields;
            foreach (var field in fields)
            {
                PopulateFieldOptions(field, formConfig.FieldConditions, entityConfig);
            }

            if (hasListFields)
            {
                foreach (var field in formConfig.ListConfig.Fields)
                {
                    PopulateFieldOptions(field, formConfig.FieldConditions, entityConfig);
                }
            }
            else
            {
                var properties = modelType.GetProperties().ToList();
                foreach (var prop in properties)
                {
                    var field = new Field
                    {
                        FieldExpression = ExpressionHelper.GetPropertyExpression(modelType, prop)
                    };
                    PopulateFieldOptions(field, formConfig.FieldConditions, entityConfig);
                    formConfig.ListConfig.Fields.Add(field);
                }
            }
        }

        private void PopulateFields(Type modelType, IFormConfig formConfig, List<Field> excludeField = null)
        {
            List<PropertyInfo> properties = modelType.GetProperties().ToList();

            foreach (var prop in properties)
            {
                bool isExclude = (excludeField != null && excludeField.Any(f => f.FieldClrType == prop.PropertyType));

                if (!isExclude)
                {
                    formConfig.FieldConfig.AddField(new Field
                    {
                        FieldExpression = ExpressionHelper.GetPropertyExpression(modelType, prop)
                    });
                }
            }
        }

        private List<PropertyInfo> GetProperties(IEntityType entityType)
        {
            return entityType
                .GetProperties()
                .Where(p => !p.IsForeignKey() && !p.IsPrimaryKey()) //Get only non key fields!
                .OrderBy(p => GetOrder(p))
                .Select(p => p.PropertyInfo)
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

        private void PopulateFieldOptions(Field field, FieldConditions fieldConditions, EntityConfig entityConfig = null)
        {
            var attributes = (field.FieldExpression.Body as MemberExpression).Member.GetCustomAttributes(true);
            var efProperty = entityConfig?.EntityType?.GetProperties().FirstOrDefault(p => p.Name == field.FieldName);

            if (field.FieldOption == null)
                field.FieldOption = new FieldOption();


            var metadata = field.FieldOption.Metadata = _modelMetadataProvider.GetMetadataForType(field.FieldClrType);



            //var displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            //var displayFormatAttribute = attributes.OfType<DisplayFormatAttribute>().FirstOrDefault();
            //var displayNameAttribute = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();

            var fieldTypeAttribute = attributes.OfType<FieldInfoAttribute>().FirstOrDefault();

            field.FieldType = fieldTypeAttribute != null ? fieldTypeAttribute.FieldType : FieldType.Unknown;

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
                //if (!string.IsNullOrEmpty(metadata.DisplayName))
                //{
                //    field.FieldOption.DisplayName = metadata.DisplayName;
                //}
                //else
                //{
                field.FieldOption.DisplayName = field.FieldName;
                //}
            }


            if (string.IsNullOrEmpty(field.FieldOption.Format) /*&& displayFormatAttribute != null*/)
            {
                //field.FieldOption.Format = metadata.DisplayFormatString; //displayFormatAttribute.DataFormatString;

                //if (string.IsNullOrEmpty(field.FieldOption.Format))
                //{
                field.FieldOption.Format = _defaultDateFormat;
                //}
            }

            //if (string.IsNullOrEmpty(field.FieldOption.Description) /*&& displayAttribute != null*/)
            //{
            //    //if (localizer != null &&
            //    //    !string.IsNullOrEmpty(displayAttribute.Description) &&
            //    //    displayAttribute.ResourceType == null)
            //    //{
            //    //    displayMetadata.Description = () => localizer[displayAttribute.Description];
            //    //}
            //    //else
            //    //{
            //    field.FieldOption.Description = metadata.Description; //displayAttribute.GetDescription();
            //    //}
            //}

            //// NullDisplayText
            //if (string.IsNullOrEmpty(field.FieldOption.NullDisplayText) /*&& displayFormatAttribute != null*/)
            //{
            //    field.FieldOption.NullDisplayText = metadata.NullDisplayText; //displayFormatAttribute.NullDisplayText;
            //}

            if (field.FieldOption.MaxLength == 0)
            {
                field.FieldOption.MaxLength = efProperty?.GetMaxLength() ?? 0;
            }

            if (field.FieldType == FieldType.Unknown)
            {
                field.FieldType = GetFieldType(field);
            }

            if (!field.FieldOption.IsRequired)
            {
                field.FieldOption.IsRequired = (!efProperty?.IsColumnNullable() ?? false);
            }

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

            //if (entityConfig != null)
            //{
            if (field.FieldOption.RelationType != RelationType.None)
            {
                //var navigation = entityConfig.Navigations.FirstOrDefault(n => n.Name == field.FieldName);
                if (field.FieldOption.RelationType == RelationType.ManyToMany)
                {
                    field.FieldType = FieldType.MultiSelect;
                    //var fieldClrType = navigation.FieldInfo.FieldType.GenericTypeArguments[0];
                    //var fieldEntityType = _dbContext.Model.FindEntityType(fieldClrType);
                    //var fieldNavigations = fieldEntityType.GetNavigations();

                    //field.FieldOption.ReleatedFields = new List<ReleatedField>();

                    //foreach (var fieldNav in fieldNavigations)
                    //{
                    //    var releatedFields = GetReleatedFields(fieldNav.ForeignKey, entityType.ClrType);
                    //    field.FieldOption.ReleatedFields.AddRange(releatedFields);
                    //}

                    //TODO: Validate ManyToMay relationship using the hints from following commented code
                }
                else if (field.FieldOption.RelationType == RelationType.ManyToOne)
                {
                    field.FieldType = FieldType.Select;

                    //var fKField = GetReleatedFields(navigation.ForeignKey, entityType.ClrType);
                    //field.FieldOption.ReleatedFields = fKField;

                    //TODO: Validate ManyToOne relationship using the hints from following commented code
                }
            }
            //}



            //Check wheter the field is one to many or many to many
            //var isMultiple = field.FieldClrType.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICollection<>));
            //var navigation = adminConfig.EntityConfig.Navigations.FirstOrDefault(n => n.Name == field.FieldName);
            //if (navigation != null)
            //{
            //    if (isMultiple && navigation.FieldInfo.FieldType.IsGenericType)
            //    {
            //        var releatedClrType = navigation.FieldInfo.FieldType.GenericTypeArguments[0];
            //        var releatedEntityType = _dbContext.Model.FindEntityType(releatedClrType);
            //        var releatedEntityNavigations = releatedEntityType.GetNavigations();
            //        var grandChildEntity = releatedEntityNavigations.FirstOrDefault(n => n.ForeignKey != navigation.ForeignKey);
            //        field.FieldOption.RelationType = grandChildEntity != null ? RelationType.ManyToMany : RelationType.OneToMany;
            //    }
            //    else
            //    {
            //        var releatedClrType = navigation.FieldInfo.FieldType;
            //        var releatedEntityType = _dbContext.Model.FindEntityType(releatedClrType);
            //    }

            //}

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

        private List<ReleatedField> GetReleatedFields(IForeignKey foreignKey, Type entityType)
        {
            var releatedField = new List<ReleatedField>();

            for (var index = 0; index < foreignKey.PrincipalKey.Properties.Count; index++)
            {
                var fKeyProp = foreignKey.Properties[index];
                var fkDecType = foreignKey.DeclaringEntityType.ClrType;
                var fKeyExpr = GetFieldExpression(fkDecType, fKeyProp);

                var principalProp = foreignKey.PrincipalKey.Properties[index];
                var pKDecType = foreignKey.PrincipalKey.DeclaringEntityType.ClrType;
                var principalExpr = GetFieldExpression(pKDecType, principalProp);

                releatedField.Add(new ReleatedField
                {
                    IsParentField = entityType == pKDecType,
                    FieldExpression = fKeyExpr,
                    SourceFieldExpression = principalExpr
                });
            }
            return releatedField;
        }

        private IKey GetPrimaryKey(Type clrType)
        {
            var entityType = _dbContext.Model.FindEntityType(clrType);
            var key = entityType.FindPrimaryKey();
            return key;
        }

        private void LoadMasterData<TModel>(AdminConfig<TModel> adminConfig) where TModel : class
        {
            //Loading Master Data
            var relatedFileds = adminConfig.FormConfig.AllFormFields
                .Where(f => f.FieldOption.RelationType == RelationType.ManyToMany || f.FieldOption.RelationType == RelationType.ManyToOne)
                .ToList();

            foreach (var relatedField in relatedFileds)
            {
                var relatedModelType = relatedField.FieldOption.ReleatedEntityType;
                var entityClrType = GetEntityClrTypeFor(relatedModelType);

                Func<List<LookUpField>> masterDataDelegate = () => CallGenericMethod<List<LookUpField>>(nameof(GetLookUpData), new Type[] { relatedModelType, entityClrType }, new object[] { relatedField.FieldOption.ReleatedEntityDisplayExpression });
                adminConfig.LookUps.Add(relatedField.FieldOption.ReleatedEntityType.Name, masterDataDelegate);
            }
        }

        private TReturnType CallGenericMethod<TReturnType>(string methodName, Type[] genericTypes, object[] parmeters)
            where TReturnType : class
        {
            try
            {
                var getItemMethodInfo = typeof(AdminSite).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
                var getItemMethod = getItemMethodInfo.MakeGenericMethod(genericTypes);
                var result = getItemMethod.Invoke(this, parmeters);
                return result as TReturnType;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private List<LookUpField> GetLookUpData<TModel, TEntity>(LambdaExpression displayExpr)
            where TEntity : class
            where TModel : class
        {
            try
            {
                IServiceScopeFactory _serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    using (var dbContext = (DbContext)scope.ServiceProvider.GetService(_dbContextType))
                    {
                        var primaryKeyExpr = GetPrimaryKeyExpressions<TEntity>(dbContext);
                        var del = displayExpr.Compile();

                        var items = dbContext
                            .Set<TEntity>()
                            .Select(item => new LookUpField()
                            {
                                Key = GetLookUpKey(item, primaryKeyExpr),
                                DisplayName = del.DynamicInvoke(new object[] { Mapper.Map<TModel>(item) }) as string //del.Method.Invoke(del, new object[]{ item }) as string
                            })
                            .ToList();
                        return items;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private List<Expression<Func<TEntity, object>>> GetPrimaryKeyExpressions<TEntity>(DbContext dbContext)
            where TEntity : class
        {
            Type eClrType = typeof(TEntity);
            var eType = dbContext.Model.FindEntityType(eClrType);
            var primaryKey = eType.FindPrimaryKey();
            var properties = primaryKey.Properties;
            var eTypeParamExpr = Expression.Parameter(eClrType);
            List<Expression<Func<TEntity, object>>> keySelectorExpressions = new List<Expression<Func<TEntity, object>>>();
            foreach (var prop in properties)
            {
                MemberExpression memberExpression = Expression.Property(eTypeParamExpr, prop.PropertyInfo);
                Expression objectMemberExpr = Expression.Convert(memberExpression, typeof(object)); //Convert Value/Reference type to object using boxing/lifting
                var pkValExpr = Expression.Lambda<Func<TEntity, object>>(objectMemberExpr, eTypeParamExpr);
                keySelectorExpressions.Add(pkValExpr);
            }
            return keySelectorExpressions;
        }

        private Dictionary<string, object> GetLookUpKey<TEntity>(TEntity item, List<Expression<Func<TEntity, object>>> primaryKeyExpr)
            where TEntity : class
        {
            var lookUpKey = new Dictionary<string, object>();
            foreach (var expr in primaryKeyExpr)
            {
                lookUpKey.Add(ReflectionExtensions.GetMemberName(expr), expr.Compile()(item));
            }
            return lookUpKey;
        }
    }
}
