using Deviser.Admin.Config;
using Deviser.Core.Common.DomainTypes.Admin;
using Deviser.Core.Common.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Deviser.Admin
{
    public interface IAdminConfig
    {
        ICollection<IChildConfig> ChildConfigs { get; }

        [JsonConverter(typeof(TypeJsonConverter))]
        Type EntityType { get; }

        [JsonIgnore]
        EntityConfig EntityConfig { get; }

        LookUpDictionary LookUps { get; }

        IFormConfig FormConfig { get; }
    }

    public class AdminConfig<TEntity> : IAdminConfig
        where TEntity : class
    {

        public ICollection<IChildConfig> ChildConfigs { get; }

        public Type EntityType { get; }

        [JsonIgnore]
        public EntityConfig EntityConfig { get; }

        public IFormConfig FormConfig { get; }

        public LookUpDictionary LookUps { get; }

        public AdminConfig()
        {
            ChildConfigs = new List<IChildConfig>();
            EntityType = typeof(TEntity);
            EntityConfig = new EntityConfig();
            FormConfig = new FormConfig<TEntity>();
            LookUps = new LookUpDictionary();
        }

        public void ShowOn(LambdaExpression fieldExpression, Expression<Func<TEntity, bool>> predicate)
        {

        }
    }

    public interface IChildConfig
    {
        [JsonIgnore]
        EntityConfig EntityConfig { get; }
        Field Field { get; }
        IFormConfig FormConfig { get; }
    }

    public class ChildConfig : IChildConfig
    {
        [JsonIgnore]
        public EntityConfig EntityConfig { get; }

        public Field Field { get; set; }

        public IFormConfig FormConfig { get; set; }

        public ChildConfig()
        {
            EntityConfig = new EntityConfig();
        }
    }

    public interface IFormConfig
    {
        [JsonIgnore]
        List<Field> AllFormFields { get; }

        IFieldConfig FieldConfig { get; }

        IFieldSetConfig FieldSetConfig { get; }

        [JsonIgnore]
        FieldConditions FieldConditions { get; }

        List<KeyField> KeyFields { get; }

        IListConfig ListConfig { get; }
    }

    public interface IFieldConfig
    {
        List<List<Field>> Fields { get; }
        List<Field> ExcludedFields { get; }

        [JsonIgnore]
        List<Field> AllIncludeFields { get; }
        void AddField(Field field);
        void AddInLineField(Field field);
        void RemoveField(Field field);

    }

    public interface IFieldSetConfig
    {
        List<FieldSet> FieldSets { get; }

        [JsonIgnore]
        List<Field> AllIncludeFields { get; }
    }

    public interface IListConfig
    {
        List<Field> Fields { get; }
    }

    public class FormConfig<TEntity> : IFormConfig
        where TEntity : class
    {
        [JsonIgnore]
        public List<Field> AllFormFields
        {
            get
            {
                var fields = FieldConfig.AllIncludeFields;
                var fieldSetFields = FieldSetConfig.AllIncludeFields;
                var returnList = new List<Field>();

                if (fields != null && fields.Count > 0)
                {
                    returnList.AddRange(fields);
                }

                if (fieldSetFields != null && fieldSetFields.Count > 0)
                {
                    returnList.AddRange(fieldSetFields);
                }

                return returnList;
            }
        }

        public FieldConfig<TEntity> FieldConfig { get; }

        public FieldSetConfig<TEntity> FieldSetConfig { get; }

        [JsonIgnore]
        public FieldConditions FieldConditions { get; }

        public List<KeyField> KeyFields { get; }

        public ListConfig<TEntity> ListConfig { get; }

        IFieldConfig IFormConfig.FieldConfig => FieldConfig;

        IFieldSetConfig IFormConfig.FieldSetConfig => FieldSetConfig;

        IListConfig IFormConfig.ListConfig => ListConfig;

        public FormConfig()
        {
            FieldConfig = new FieldConfig<TEntity>();
            FieldConditions = new FieldConditions();
            FieldSetConfig = new FieldSetConfig<TEntity>();
            KeyFields = new List<KeyField>();
            ListConfig = new ListConfig<TEntity>();
        }
    }

    public class FieldConfig<TEntity> : IFieldConfig
        where TEntity : class
    {

        private Dictionary<string, Field> allFields;
        public List<List<Field>> Fields { get; }

        [JsonIgnore]
        public List<Field> ExcludedFields { get; }

        [JsonIgnore]
        public List<Field> AllIncludeFields
        {
            get
            {
                return allFields?.Values?.ToList();
            }
        }

        public FieldConfig()
        {
            allFields = new Dictionary<string, Field>();
            Fields = new List<List<Field>>();
            ExcludedFields = new List<Field>();
        }

        public void AddField(Field field)
        {
            AddToDictionary(field);

            Fields.Add(new List<Field>() {
                field
            });
        }

        public void AddInLineField(Field field)
        {
            AddToDictionary(field);

            if (Fields.Count == 0)
                Fields.Add(new List<Field>());

            var FieldRow = Fields.Last();

            FieldRow.Add(field);
        }

        private void AddToDictionary(Field field)
        {
            if (!allFields.ContainsKey(field.FieldName))
                allFields.Add(field.FieldName, field);
        }

        public void RemoveField(Field field)
        {
            ExcludedFields.Add(field);
        }
    }

    public class FieldSetConfig<TEntity> : IFieldSetConfig
        where TEntity : class
    {
        private Dictionary<string, Field> allFields;
        public List<FieldSet> FieldSets { get; }

        [JsonIgnore]
        public List<Field> AllIncludeFields
        {
            get
            {
                return allFields?.Values?.ToList();
            }
        }

        public FieldSetConfig()
        {
            allFields = new Dictionary<string, Field>();
            FieldSets = new List<FieldSet>();
        }

        public void AddFieldSet(FieldSet fieldSet)
        {
            foreach (var fieldRow in fieldSet.Fields)
            {
                foreach (var field in fieldRow)
                {
                    AddToDictionary(field);
                }
            }

            FieldSets.Add(fieldSet);
        }

        private void AddToDictionary(Field field)
        {
            if (!allFields.ContainsKey(field.FieldName))
                allFields.Add(field.FieldName, field);
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

    public class SelectItem
    {
        public object Key { get; set; }
        public string DisplayName { get; set; }
    }

}