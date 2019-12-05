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
    public interface IAdminBaseConfig
    {
        [JsonIgnore]
        EntityConfig EntityConfig { get; set; }
        IModelConfig ModelConfig { get; }
    }

    public interface IAdminConfig : IAdminBaseConfig
    {
        Type AdminServiceType { get; set; }

        ICollection<IChildConfig> ChildConfigs { get; }

        [JsonConverter(typeof(TypeJsonConverter))]
        Type ModelType { get; }

        LookUpDictionary LookUps { get; }
    }

    public class AdminConfig<TModel> : IAdminConfig
        where TModel : class
    {
        public Type AdminServiceType { get; set; }

        public ICollection<IChildConfig> ChildConfigs { get; }

        public Type ModelType { get; }

        [JsonIgnore]
        public EntityConfig EntityConfig { get; set; }

        public IModelConfig ModelConfig { get; }

        public LookUpDictionary LookUps { get; }        

        public AdminConfig()
        {
            ChildConfigs = new List<IChildConfig>();
            ModelType = typeof(TModel);
            ModelConfig = new ModelConfig();
            LookUps = new LookUpDictionary();
        }

        public void ShowOn(LambdaExpression fieldExpression, Expression<Func<TModel, bool>> predicate)
        {

        }
    }

    public interface IChildConfig : IAdminBaseConfig
    {
        Field Field { get; }
    }

    public class ChildConfig : IChildConfig
    {
        [JsonIgnore]
        public EntityConfig EntityConfig { get; set; }

        public Field Field { get; set; }

        public IModelConfig ModelConfig { get; set; }

        public ChildConfig()
        {

        }
    }

    public interface IFormConfig
    {
        [JsonIgnore] List<Field> AllFormFields { get; }
        IFieldConfig FieldConfig { get; }
        IFieldSetConfig FieldSetConfig { get; }
        [JsonIgnore] FieldConditions FieldConditions { get; }
    }

    public interface IModelConfig
    {
        KeyField KeyField { get; }
        IGridConfig GridConfig { get; }
        IFormConfig FormConfig { get; }
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

    public interface IGridConfig
    {
        List<Field> Fields { get; }
    }

    public class ModelConfig : IModelConfig
    {
        public IFormConfig FormConfig { get; }
        public IGridConfig GridConfig { get; }
        public KeyField KeyField { get; }

        public ModelConfig()
        {
            FormConfig = new FormConfig();
            GridConfig = new GridConfig();
            KeyField = new KeyField();
        }
    }

    public class FormConfig : IFormConfig
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
        public IFieldConfig FieldConfig { get; }
        public IFieldSetConfig FieldSetConfig { get; }
        public FieldConditions FieldConditions { get; }

        public FormConfig()
        {
            FieldConfig = new FieldConfig();
            FieldConditions = new FieldConditions();
            FieldSetConfig = new FieldSetConfig();
        }
    }

    public class FieldConfig : IFieldConfig
    {

        private readonly Dictionary<string, Field> _allFields;
        public List<List<Field>> Fields { get; }

        [JsonIgnore]
        public List<Field> ExcludedFields { get; }

        [JsonIgnore]
        public List<Field> AllIncludeFields => _allFields?.Values?.ToList();

        public FieldConfig()
        {
            _allFields = new Dictionary<string, Field>();
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

            var fieldRow = Fields.Last();

            fieldRow.Add(field);
        }

        private void AddToDictionary(Field field)
        {
            if (!_allFields.ContainsKey(field.FieldName))
                _allFields.Add(field.FieldName, field);
        }

        public void RemoveField(Field field)
        {
            ExcludedFields.Add(field);
        }
    }

    public class FieldSetConfig : IFieldSetConfig
    {
        private readonly Dictionary<string, Field> _allFields;
        public List<FieldSet> FieldSets { get; }

        [JsonIgnore]
        public List<Field> AllIncludeFields => _allFields?.Values?.ToList();

        public FieldSetConfig()
        {
            _allFields = new Dictionary<string, Field>();
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
            if (!_allFields.ContainsKey(field.FieldName))
                _allFields.Add(field.FieldName, field);
        }

    }

    public class GridConfig : IGridConfig
    {
        public List<Field> Fields { get; }

        public GridConfig()
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