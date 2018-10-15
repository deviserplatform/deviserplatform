using Deviser.Admin.Config;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
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
        IFieldConfig FieldConfig { get; }
        IFieldSetConfig FieldSetConfig { get; }
        IListConfig ListConfig { get; }
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

    public class ListConfig<TEntity>: IListConfig
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

                MemberExpression memberExpression;

                if (FieldExpression.Body is UnaryExpression)
                {
                    UnaryExpression unaryExpression = (UnaryExpression)(FieldExpression.Body);
                    memberExpression = (MemberExpression)(unaryExpression.Operand);
                }
                else
                {
                    memberExpression = (MemberExpression)(FieldExpression.Body);
                }


                _fieldName = ((PropertyInfo)memberExpression.Member).Name;
                return _fieldName;
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
        public bool IsHide { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsRequired { get; set; }
        public ValidationType ValidationType { get; set; }
        public string ValidatorRegEx { get; set; }
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
        Currency=20,
        Url=21,
        CreditCard=22,
        Custom=23
    }

    public enum ValidationType
    {
        Email, NumberOnly, LettersOnly, Password, UserExist, UserExistByEmail, RegEx, Custom
    }

    public class FieldSet
    {
        public string GroupName { get; set; }
        public string CssClasses { get; set; }
        public string Description { get; set; }
        public List<List<Field>> Fields { get; set; }
    }
}