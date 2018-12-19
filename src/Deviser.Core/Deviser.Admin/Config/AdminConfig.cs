using Deviser.Admin.Config;
using Deviser.Core.Common.DomainTypes.Admin;
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
        ICollection<IAdminConfig> ChildConfigs { get; }
        Type EntityType { get; }
        [JsonIgnore]
        EntityConfig EntityConfig { get; }
        IFieldConfig FieldConfig { get; }
        IFieldSetConfig FieldSetConfig { get; }
        [JsonIgnore]
        FieldConditions FieldConditions { get; }
        List<Field> KeyFields { get; }
        IListConfig ListConfig { get; }

    }

    public class AdminConfig<TEntity> : IAdminConfig
        where TEntity : class
    {
        public ICollection<IAdminConfig> ChildConfigs { get; }
        public Type EntityType { get; }
        [JsonIgnore]
        public EntityConfig EntityConfig { get; }
        public FieldConfig<TEntity> FieldConfig { get; }
        public FieldSetConfig<TEntity> FieldSetConfig { get; }
        [JsonIgnore]
        public FieldConditions FieldConditions { get; }
        public List<Field> KeyFields { get; }
        public ListConfig<TEntity> ListConfig { get; }

        IFieldConfig IAdminConfig.FieldConfig => FieldConfig;
        IFieldSetConfig IAdminConfig.FieldSetConfig => FieldSetConfig;
        IListConfig IAdminConfig.ListConfig => ListConfig;

        public AdminConfig()
        {
            ChildConfigs = new List<IAdminConfig>();
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
}