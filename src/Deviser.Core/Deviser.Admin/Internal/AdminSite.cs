using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using AutoMapper;
using Deviser.Admin.Attributes;
using Deviser.Admin.Config;
using Deviser.Admin.Data;
using Deviser.Core.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Deviser.Admin.Internal
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


        private readonly DbContext _dbContext;
        private readonly Type _dbContextType;
        //private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly IServiceProvider _serviceProvider;
        private List<TypeMap> _typeMaps;

        public IDictionary<Type, IAdminConfig> AdminConfigs { get; }
        public AdminType AdminType { get; }
        public Type DbContextType => _dbContextType;
        public IMapper Mapper { get; set; }
        public string SiteName { get; set; }

        public AdminSite(IServiceProvider serviceProvider, DbContext dbContext/*, IModelMetadataProvider modelMetadataProvider*/)
        {
            AdminType = AdminType.Entity;

            _dbContext = dbContext ?? throw new ArgumentNullException("Constructor paramater dbContent cannot be null");

            _serviceProvider = serviceProvider;

            //_modelMetadataProvider = modelMetadataProvider;
            _dbContextType = _dbContext.GetType();
            AdminConfigs = new Dictionary<Type, IAdminConfig>();
        }

        public AdminSite(IServiceProvider serviceProvider/*, IModelMetadataProvider modelMetadataProvider*/)
        {
            AdminType = AdminType.Custom;
            _serviceProvider = serviceProvider;
            //_modelMetadataProvider = modelMetadataProvider;
            AdminConfigs = new Dictionary<Type, IAdminConfig>();
        }

        public void Build<TModel>(AdminConfig<TModel> adminConfig, bool hasConfiguration = false) where TModel : class
        {
            var modelType = typeof(TModel);

            switch (AdminType)
            {
                //TODO: Validate adminConfig for entity type
                case AdminType.Entity when AdminConfigs.ContainsKey(modelType):
                    throw new InvalidOperationException($"The ModelType {modelType} has already been registered for this admin site, duplicate site registrations are not allowed");
                case AdminType.Entity when Mapper == null:
                    throw new InvalidOperationException($"AutoMapper configuration is required when creating admin site using AdminType: {AdminType}");
                case AdminType.Entity:
                    {
                        if (_typeMaps == null)
                        {
                            _typeMaps = Mapper.ConfigurationProvider.GetAllTypeMaps().ToList();
                        }

                        var entityClrType = GetEntityClrTypeFor(modelType);

                        if (entityClrType == null)
                        {
                            throw new InvalidOperationException($"The entity type for the ModelType: {modelType} cannot be found in MapperConfiguration, please AutoMapper configuration for ModelType and ModelType");
                        }

                        var entityType = _dbContext.Model.FindEntityType(entityClrType);

                        if (entityType == null)
                        {
                            throw new InvalidOperationException($"The entity type for the ModelType: {modelType} cannot be found on this context {_dbContext}");
                        }

                        adminConfig.EntityConfig = new EntityConfig(_dbContextType, entityType);
                        BuildMainFormAndCustomForm(adminConfig, hasConfiguration, modelType);

                        if (adminConfig.ChildConfigs != null && adminConfig.ChildConfigs.Count > 0)
                        {
                            foreach (var childConfig in adminConfig.ChildConfigs)
                            {
                                var childModelType = childConfig.Field.FieldClrType;

                                var childEntityClrType = GetEntityClrTypeFor(childModelType);

                                if (childEntityClrType == null)
                                {
                                    throw new InvalidOperationException($"The entity type for the ModelType: {childModelType} cannot be found in MapperConfiguration, please check MapperConfiguration");
                                }

                                var childEntityType = _dbContext.Model.FindEntityType(entityClrType);

                                if (childEntityType == null)
                                {
                                    throw new InvalidOperationException($"The entity type for the ModelType: {modelType} cannot be found on this context {_dbContext}");
                                }

                                childConfig.EntityConfig = new EntityConfig(_dbContextType, childEntityType);
                                BuildForm(childConfig, hasConfiguration, childModelType);
                            }
                        }

                        break;
                    }
                case AdminType.Custom when adminConfig.AdminServiceType == null:
                    throw new InvalidOperationException($"AdminService is required when creating admin site with AdminType: {AdminType}");
                case AdminType.Custom when (adminConfig.ModelConfig.KeyField.FieldExpression == null && adminConfig.AdminConfigType == AdminConfigType.GridAndForm):
                    throw new InvalidOperationException($"KeyField is required when creating admin site with AdminType: {AdminType}");
                case AdminType.Custom:
                    {
                        //TODO: Validate adminConfig for custom type

                        BuildMainFormAndCustomForm(adminConfig, hasConfiguration, modelType);

                        if (adminConfig.ChildConfigs != null && adminConfig.ChildConfigs.Count > 0)
                        {
                            foreach (var childConfig in adminConfig.ChildConfigs)
                            {
                                var childModelType = childConfig.Field.FieldClrType;
                                BuildForm(childConfig, hasConfiguration, childModelType);
                            }
                        }

                        break;
                    }
            }

            LoadMasterData(adminConfig);
            AdminConfigs.Add(modelType, adminConfig);
        }

        private void BuildMainFormAndCustomForm<TModel>(AdminConfig<TModel> adminConfig, bool hasConfiguration, Type modelType)
            where TModel : class
        {
            BuildForm(adminConfig, hasConfiguration, modelType);

            foreach (var customForm in adminConfig.ModelConfig.CustomForms.Values)
            {
                if (customForm.SubmitActionExpression == null)
                {
                    throw new InvalidOperationException($"SubmitActionExpression is required while adding a custom form");
                }

                //hasConfiguration must be always true, since custom forms must have configuration and it must be configured!
                BuildForm(customForm.FormConfig, true, customForm.ModelType);
            }
        }

        public TypeMap GetTypeMapFor(Type modelType)
        {
            var typeMap = _typeMaps.FirstOrDefault(tm => tm.SourceType == modelType);
            if (typeMap == null)
            {
                typeMap = _typeMaps.FirstOrDefault(tm => tm.DestinationType == modelType);
            }
            return typeMap;
        }

        public Type GetEntityClrTypeFor(Type modelType)
        {
            var entityClrType = _typeMaps.FirstOrDefault(tm => tm.SourceType == modelType)?.DestinationType;
            if (entityClrType == null)
            {
                entityClrType = _typeMaps.FirstOrDefault(tm => tm.DestinationType == modelType)?.SourceType;
            }
            return entityClrType;
        }

        private void BuildForm(IFormConfig formConfig, bool hasConfiguration, Type modelType)
        {
            BuildForm(formConfig, null, null, hasConfiguration, modelType);
        }

        private void BuildForm(IAdminBaseConfig adminBaseConfig, bool hasConfiguration, Type modelType)
        {
            BuildForm(adminBaseConfig.ModelConfig.FormConfig, adminBaseConfig.ModelConfig.GridConfig,
                adminBaseConfig.EntityConfig, hasConfiguration, modelType);

            if (adminBaseConfig is ChildConfig childConfig && childConfig.ShowOnStaticExpression != null)
            {
                bool ShowOnDelegate()
                {
                    var lookUpFields = new List<LookUpField>();
                    var ShowOnStaticExpressionDel = childConfig.ShowOnStaticExpression.Compile();
                    var serviceProvider =
                        _serviceProvider.GetService<IHttpContextAccessor>().HttpContext.RequestServices;
                    var result = ShowOnStaticExpressionDel.DynamicInvoke(new object[] { serviceProvider });
                    return (bool)result;
                }

                childConfig.ShowOnDelegateFunc = ShowOnDelegate;
            }
        }

        private void BuildForm(IFormConfig formConfig, IGridConfig gridConfig, EntityConfig entityConfig, bool hasConfiguration, Type modelType)
        {
            //var modelConfig = adminBaseConfig.ModelConfig;
            //var entityConfig = adminBaseConfig.EntityConfig;
            if (!hasConfiguration)
            {
                //Register by default settings and fields
                PopulateFields(modelType, formConfig);
            }

            //PopulateEntityConfig(entityType, entityConfig);

            //Adding primary keys to KeyField

            var hasExcludeFields = formConfig?.FieldConfig?.ExcludedFields?.Count > 0;



            if (hasExcludeFields)
            {
                PopulateFields(modelType, formConfig, formConfig.FieldConfig.ExcludedFields);
            }

            var fields = formConfig.AllFields;
            foreach (var field in fields)
            {
                PopulateFieldOptions(field, formConfig.FieldConditions, entityConfig);
            }

            if (gridConfig != null)
            {
                var hasListFields = gridConfig.Fields.Count > 0;
                if (hasListFields)
                {
                    foreach (var field in gridConfig.Fields)
                    {
                        PopulateFieldOptions(field, formConfig.FieldConditions, entityConfig);
                    }
                }
                else
                {
                    var properties = GetProperties(modelType);
                    foreach (var prop in properties)
                    {
                        var field = new Field
                        {
                            FieldExpression = ExpressionHelper.GetPropertyExpression(modelType, prop)
                        };
                        PopulateFieldOptions(field, formConfig.FieldConditions, entityConfig);
                        gridConfig.AddField(field);
                    }
                }
            }


        }

        private void PopulateFields(Type modelType, IFormConfig formConfig, ICollection<Field> excludeField = null)
        {
            var properties = GetProperties(modelType);

            foreach (var prop in properties)
            {
                var isExclude = (excludeField != null && excludeField.Any(f => f.FieldClrType == prop.PropertyType));

                if (!isExclude)
                {
                    formConfig.FieldConfig.AddField(new Field
                    {
                        FieldExpression = ExpressionHelper.GetPropertyExpression(modelType, prop)
                    });
                }
            }
        }

        private List<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).ToList();
        }

        //private List<PropertyInfo> GetProperties(IEntityType entityType)
        //{
        //    return entityType
        //        .GetProperties()
        //        .Where(p => !p.IsForeignKey() && !p.IsPrimaryKey()) //Get only non key fields!
        //        .OrderBy(p => GetOrder(p))
        //        .Select(p => p.PropertyInfo)
        //        .ToList();
        //}

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
            var navigation = entityConfig?.Navigations?.FirstOrDefault(n => n.Name == field.FieldName);
            if (efProperty == null && entityConfig?.EntityType != null && navigation != null)
            {
                var releatedField = GetReleatedFields(navigation.ForeignKey, entityConfig.EntityType.ClrType).FirstOrDefault();
                efProperty = entityConfig.EntityType.GetProperties().FirstOrDefault(p => p.Name == releatedField.FieldName);
            }

            if (field.FieldOption == null)
                field.FieldOption = new FieldOption();


            //field.FieldOption.Metadata = _modelMetadataProvider.GetMetadataForType(field.FieldClrType);



            //var displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            //var displayFormatAttribute = attributes.OfType<DisplayFormatAttribute>().FirstOrDefault();
            //var displayNameAttribute = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();

            if (field.FieldOption.FieldType == FieldType.Unknown)
            {
                var fieldTypeAttribute = attributes.OfType<FieldInfoAttribute>().FirstOrDefault();
                field.FieldType = fieldTypeAttribute?.FieldType ?? FieldType.Unknown;
            }
            else
            {
                field.FieldType = field.FieldOption.FieldType;
            }

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

            if (field.FieldOption.IsRequired == null)
            {
                field.FieldOption.IsRequired = (!efProperty?.IsColumnNullable() ?? false) || !field.FieldClrType.IsNullable();
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

        private List<RelatedField> GetReleatedFields(IForeignKey foreignKey, Type entityType)
        {
            var releatedField = new List<RelatedField>();

            for (var index = 0; index < foreignKey.PrincipalKey.Properties.Count; index++)
            {
                var fKeyProp = foreignKey.Properties[index];
                var fkDecType = foreignKey.DeclaringEntityType.ClrType;
                var fKeyExpr = GetFieldExpression(fkDecType, fKeyProp);

                var principalProp = foreignKey.PrincipalKey.Properties[index];
                var pKDecType = foreignKey.PrincipalKey.DeclaringEntityType.ClrType;
                var principalExpr = GetFieldExpression(pKDecType, principalProp);

                releatedField.Add(new RelatedField
                {
                    IsParentField = entityType == pKDecType,
                    FieldExpression = fKeyExpr,
                    SourceFieldExpression = principalExpr
                });
            }
            return releatedField;
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

            var fieldClrType = field.FieldExpression.Body.Type;

            if (fieldClrType != typeof(string) && (fieldClrType.GetInterface(nameof(IEnumerable)) != null ||
                                                  fieldClrType.GetInterface(nameof(ICollection)) != null))
            {
                return FieldType.MultiSelect;
            }

            if (!fieldClrType.IsPrimitive && fieldClrType != typeof(decimal) && fieldClrType != typeof(string))
            {
                return FieldType.Select;
            }

            //If nothing found return textbox
            return FieldType.TextBox;
        }

        private static IEnumerable<string> GetFieldTypeHints(Field field)
        {
            //var metadata = field.FieldOption.Metadata;

            //if (!string.IsNullOrEmpty(metadata.TemplateHint))
            //{
            //    yield return metadata.TemplateHint;
            //}

            //if (!string.IsNullOrEmpty(metadata.DataTypeName))
            //{
            //    yield return metadata.DataTypeName;
            //}

            //// In most cases, we don't want to search for Nullable<T>. We want to search for T, which should handle
            //// both T and Nullable<T>. However we special-case bool? to avoid turning an <input/> into a <select/>.
            //var fieldType = metadata.ModelType;
            //if (typeof(bool?) != fieldType)
            //{
            //    fieldType = metadata.UnderlyingOrModelType;
            //}

            foreach (var typeName in GetTypeNames(field.FieldClrType))
            {
                yield return typeName;
            }
        }

        public static IEnumerable<string> GetTypeNames(Type fieldType)
        {
            // Not returning type name here for IEnumerable<IFormFile> since we will be returning
            // a more specific name, IEnumerableOfIFormFileName.
            var fieldTypeInfo = fieldType.GetTypeInfo();

            //if (typeof(IEnumerable<IFormFile>) != fieldType)
            //{
            //    yield return fieldType.Name;
            //}

            var isComplexType = !TypeDescriptor.GetConverter(fieldTypeInfo).CanConvertFrom(typeof(string));

            if (fieldType == typeof(string))
            {
                // Nothing more to provide
                yield break;
            }
            else if (!isComplexType)
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

                yield return fieldType.IsGenericType ? fieldType.GetGenericArguments()[0].Name : fieldType.Name;
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

            //if (typeof(IEnumerable).IsAssignableFrom(fieldType))
            //{
            //    if (typeof(IEnumerable<IFormFile>).IsAssignableFrom(fieldType))
            //    {
            //        yield return IEnumerableOfIFormFileName;

            //        // Specific name has already been returned, now return the generic name.
            //        if (typeof(IEnumerable<IFormFile>) == fieldType)
            //        {
            //            yield return fieldType.Name;
            //        }
            //    }

            //    yield return "Collection";
            //}
            //else if (typeof(IFormFile) != fieldType && typeof(IFormFile).IsAssignableFrom(fieldType))
            //{
            //    yield return nameof(IFormFile);
            //}

            yield return "Object";
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
            var relatedFileds = adminConfig.ModelConfig.FormConfig.AllFields
                .Where(f => f.FieldOption.RelationType == RelationType.ManyToMany || f.FieldOption.RelationType == RelationType.ManyToOne)
                .ToList();

            var relatedFieldsInGrid = adminConfig.ModelConfig.GridConfig.AllFields
                .Where(f => f.FieldOption.LookupExpression != null).ToList();

            var matrixFields = adminConfig.ModelConfig.FormConfig.AllFields
                .Where(f => f.FieldOption.FieldType == FieldType.CheckBoxMatrix)
                .ToList();

            foreach (var field in relatedFieldsInGrid)
            {
                if (relatedFileds.All(f => f.FieldName != field.FieldName))
                {
                    relatedFileds.Add(field);
                }
            }

            if (adminConfig.ChildConfigs != null)
            {
                foreach (var childConfig in adminConfig.ChildConfigs)
                {
                    var childConfigReleatedFields = childConfig.ModelConfig.FormConfig.AllFields
                        .Where(f => f.FieldOption.RelationType == RelationType.ManyToMany || f.FieldOption.RelationType == RelationType.ManyToOne)
                        .ToList();

                    relatedFileds.AddRange(childConfigReleatedFields);
                }
            }


            if (AdminType == AdminType.Entity)
            {
                foreach (var relatedField in relatedFileds)
                {
                    var relatedModelType = relatedField.FieldOption.LookupModelType;
                    var entityClrType = GetEntityClrTypeFor(relatedModelType);

                    Func<List<LookUpField>> masterDataDelegate = () => CallGenericMethod<List<LookUpField>>(nameof(GetLookUpDataFromEntity), new Type[] { relatedModelType, entityClrType }, new object[] { relatedField.FieldOption.LookupDisplayExpression });
                    adminConfig.LookUps.Add(relatedField.FieldName, masterDataDelegate);
                }
            }
            else if (AdminType == AdminType.Custom)
            {
                foreach (var relatedFiled in relatedFileds)
                {
                    if (relatedFiled.FieldOption.LookupFilterExpression != null) continue;

                    AddLookUpField(adminConfig, relatedFiled.FieldName, relatedFiled.FieldOption.LookupExpression, relatedFiled.FieldOption.LookupKeyExpression, relatedFiled.FieldOption.LookupDisplayExpression);
                }

                foreach (var matrixField in matrixFields)
                {
                    if (matrixField.FieldOption.CheckBoxMatrix == null) continue;

                    var checkBoxMatrix = matrixField.FieldOption.CheckBoxMatrix;
                    AddLookUpField(adminConfig, checkBoxMatrix.RowType.Name, checkBoxMatrix.RowLookupExpression, checkBoxMatrix.RowLookupKeyExpression, checkBoxMatrix.RowLookupDisplayExpression);
                    AddLookUpField(adminConfig, checkBoxMatrix.ColumnType.Name, checkBoxMatrix.ColLookupExpression, checkBoxMatrix.ColLookupKeyExpression, checkBoxMatrix.ColLookupDisplayExpression);
                }
            }
        }

        private void AddLookUpField<TModel>(AdminConfig<TModel> adminConfig,
            string lookupKey,
            LambdaExpression lookupExpression,
            LambdaExpression lookupKeyExpression,
            LambdaExpression lookupDisplayExpression) where TModel : class
        {
            List<LookUpField> MasterDataDelegate()
            {
                var lookUpFields = new List<LookUpField>();

                var entityLookupExprDelegate = lookupExpression.Compile();
                var entityLookupExprKeyDelegate = lookupKeyExpression.Compile();
                var displayExprDelegate = lookupDisplayExpression.Compile();
                var keyFieldName = ReflectionExtensions.GetMemberName(lookupKeyExpression);

                var serviceProvider =
                    _serviceProvider.GetService<IHttpContextAccessor>().HttpContext.RequestServices;

                var items = entityLookupExprDelegate.DynamicInvoke(new object[] { serviceProvider }) as IList;

                foreach (var item in items)
                {
                    var keyValue = entityLookupExprKeyDelegate.DynamicInvoke(new object[] { item });
                    var displayName = displayExprDelegate.DynamicInvoke(new object[] { item }) as string;
                    lookUpFields.Add(new LookUpField()
                    { Key = new Dictionary<string, object>() { { keyFieldName, keyValue } }, DisplayName = displayName });
                }

                return lookUpFields;
            }

            //adminConfig.LookUps.Add(relatedFiled.FieldOption.LookupModelType.Name, MasterDataDelegate);
            adminConfig.LookUps.Add(lookupKey, MasterDataDelegate);
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

        private List<LookUpField> GetLookUpDataFromEntity<TModel, TEntity>(LambdaExpression displayExpr)
            where TEntity : class
            where TModel : class
        {
            try
            {
                var _serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
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

        public static List<Expression<Func<TEntity, object>>> GetPrimaryKeyExpressions<TEntity>(DbContext dbContext)
            where TEntity : class
        {
            var eClrType = typeof(TEntity);
            var eType = dbContext.Model.FindEntityType(eClrType);
            var primaryKey = eType.FindPrimaryKey();
            var properties = primaryKey.Properties;
            var eTypeParamExpr = Expression.Parameter(eClrType);
            var keySelectorExpressions = new List<Expression<Func<TEntity, object>>>();
            foreach (var prop in properties)
            {
                var memberExpression = Expression.Property(eTypeParamExpr, prop.PropertyInfo);
                Expression objectMemberExpr = Expression.Convert(memberExpression, typeof(object)); //Convert Value/Reference type to object using boxing/lifting
                var pkValExpr = Expression.Lambda<Func<TEntity, object>>(objectMemberExpr, eTypeParamExpr);
                keySelectorExpressions.Add(pkValExpr);
            }
            return keySelectorExpressions;
        }

        public static List<Expression<Func<TEntity, string>>> GetPrimaryKeyStringExpressions<TEntity>(DbContext dbContext)
            where TEntity : class
        {
            var eClrType = typeof(TEntity);
            var eType = dbContext.Model.FindEntityType(eClrType);
            var primaryKey = eType.FindPrimaryKey();
            var properties = primaryKey.Properties;
            var eTypeParamExpr = Expression.Parameter(eClrType);
            var keySelectorExpressions = new List<Expression<Func<TEntity, string>>>();
            foreach (var prop in properties)
            {
                var memberExpression = Expression.Property(eTypeParamExpr, prop.PropertyInfo);

                var toStringMethod = typeof(object).GetMethod("ToString");
                var methodCall = Expression.Call(memberExpression, toStringMethod);
                //Expression objectMemberExpr = Expression.Convert(memberExpression, typeof(object)); //Convert Value/Reference type to object using boxing/lifting
                var pkValExpr = Expression.Lambda<Func<TEntity, string>>(methodCall, eTypeParamExpr);
                keySelectorExpressions.Add(pkValExpr);
            }
            return keySelectorExpressions;
        }

        public static Dictionary<string, object> GetLookUpKey<TEntity>(TEntity item, List<Expression<Func<TEntity, object>>> primaryKeyExpr)
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
