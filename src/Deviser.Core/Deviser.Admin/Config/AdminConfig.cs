using Deviser.Admin.Config;
using Deviser.Core.Common.Extensions;
using Deviser.Core.Common.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Deviser.Admin
{
    public interface IAdminConfig
    {
        Type EntityType { get; }
        List<Field> KeyFields { get; }
        IFieldConfig FieldConfig { get; }
        IFieldSetConfig FieldSetConfig { get; }
        IListConfig ListConfig { get; }

        [JsonIgnore]
        EntityConfig EntityConfig { get; }
    }

    public class AdminConfig<TEntity> : IAdminConfig
        where TEntity : class
    {
        public FieldConfig<TEntity> FieldConfig { get; }
        public FieldSetConfig<TEntity> FieldSetConfig { get; }
        public ListConfig<TEntity> ListConfig { get; }
        public EntityConfig EntityConfig { get; }
        public Type EntityType { get; }
        public List<Field> KeyFields { get; }

        [JsonIgnore]
        public FieldConditions FieldConditions { get; }

        IFieldConfig IAdminConfig.FieldConfig => FieldConfig;
        IFieldSetConfig IAdminConfig.FieldSetConfig => FieldSetConfig;
        IListConfig IAdminConfig.ListConfig => ListConfig;

        public AdminConfig()
        {
            EntityType = typeof(TEntity);
            FieldConfig = new FieldConfig<TEntity>();
            FieldSetConfig = new FieldSetConfig<TEntity>();
            ListConfig = new ListConfig<TEntity>();
            EntityConfig = new EntityConfig();
            KeyFields = new List<Field>();
            FieldConditions = new FieldConditions();
        }

        public void ShowOn(LambdaExpression fieldExpression, Expression<Func<TEntity, bool>> predicate)
        {

        }
    }

    public interface IFieldConfig
    {
        List<List<Field>> Fields { get; }
        List<Field> ExcludedFields { get; }
    }

    public interface IFieldSetConfig
    {
        List<FieldSet> FieldSets { get; }
    }

    public interface IListConfig
    {
        List<Field> Fields { get; }
    }

    public class FieldConfig<TEntity> : IFieldConfig
        where TEntity : class
    {
        public List<List<Field>> Fields { get; }

        [JsonIgnore]
        public List<Field> ExcludedFields { get; }

        public FieldConfig()
        {
            Fields = new List<List<Field>>();
            ExcludedFields = new List<Field>();
        }
    }

    public class FieldSetConfig<TEntity> : IFieldSetConfig
        where TEntity : class
    {
        public List<FieldSet> FieldSets { get; }

        public FieldSetConfig()
        {
            FieldSets = new List<FieldSet>();
        }
    }

    public class ListConfig<TEntity> : IListConfig
    {
        public List<Field> Fields { get; }

        public ListConfig()
        {
            Fields = new List<Field>();
        }
    }

    public class Field
    {

        private string _fieldName;

        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType FieldType { get; set; }

        [JsonIgnore]
        public LambdaExpression FieldExpression { get; set; }

        [JsonIgnore]
        public Type FieldClrType
        {
            get
            {
                if (FieldExpression == null)
                    return null;
                return FieldExpression.Type.GenericTypeArguments[1];
            }
        }

        public string FieldName
        {
            get
            {
                if (!string.IsNullOrEmpty(_fieldName))
                    return _fieldName;

                if (FieldExpression == null)
                    return null;

                _fieldName = ReflectionExtensions.GetMemberName(FieldExpression);
                return _fieldName;
            }
        }

        public string FieldNameCamelCase
        {
            get
            {
                return FieldName.Camelize();
            }
        }

        public FieldOption FieldOption { get; set; }
    }

    public class FieldOption
    {
        [JsonIgnore]
        public ModelMetadata Metadata { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Format { get; set; }
        public int MaxLength { get; set; }
        public string NullDisplayText { get; set; }
        public bool IsHidden { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsRequired { get; set; }

        [JsonConverter(typeof(ExpressionJsonConverter))]
        public LambdaExpression ShowOn { get; set; }

        [JsonConverter(typeof(ExpressionJsonConverter))]
        public LambdaExpression EnableOn { get; set; }

        [JsonConverter(typeof(ExpressionJsonConverter))]
        public LambdaExpression ValidateOn { get; set; }

        public ValidationType ValidationType { get; set; }
        public string ValidatorRegEx { get; set; }
    }

    public class FieldCondition
    {
        private string _fieldName;
        public LambdaExpression FieldExpression { get; set; }
        public string FieldName
        {
            get
            {
                if (!string.IsNullOrEmpty(_fieldName))
                    return _fieldName;

                if (FieldExpression == null)
                    return null;

                _fieldName = ReflectionExtensions.GetMemberName(FieldExpression);
                return _fieldName;
            }
        }
        public LambdaExpression ConditionExpression { get; set; }
    }

    public class FieldConditions
    {
        public List<FieldCondition> ShowOnConditions { get; }
        public List<FieldCondition> EnableOnConditions { get; }
        public List<FieldCondition> ValidateOnConditions { get; }

        public FieldConditions()
        {
            ShowOnConditions = new List<FieldCondition>();
            EnableOnConditions = new List<FieldCondition>();
            ValidateOnConditions = new List<FieldCondition>();
        }

    }

    public enum FieldType
    {
        Unknown = 0,
        Static = 1,
        TextBox = 2,
        Number = 3,
        EmailAddress = 4,
        Phone = 5,
        TextArea = 6,
        RichText = 7,
        Date = 8,
        Time = 9,
        DateTime = 10,
        Select = 11,
        MultiSelect = 12,
        RadioButton = 13,
        MultiSelectCheckBox = 14,
        FileAttachment = 15,
        Image = 16,
        Password = 17,
        Hidden = 18,
        CheckBox = 19,
        Currency = 20,
        Url = 21,
        CreditCard = 22,
        Custom = 23
    }

    public enum ValidationType
    {
        Email = 0,
        NumberOnly = 1,
        LettersOnly = 2,
        Password = 3,
        UserExist = 4,
        UserExistByEmail = 5,
        RegEx = 6,
        Custom = 7
    }

    public class FieldSet
    {
        public string GroupName { get; set; }
        public string CssClasses { get; set; }
        public string Description { get; set; }
        public List<List<Field>> Fields { get; set; }
    }

    public class PropertyBuilder<TEntity>
        where TEntity : class
    {
        public AdminConfig<TEntity> AdminConfig { get; set; }
        public LambdaExpression FieldExpression { get; set; }
    }
}