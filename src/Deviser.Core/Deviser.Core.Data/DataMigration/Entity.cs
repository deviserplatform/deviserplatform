using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace Deviser.Core.Data.DataMigration
{
    public class Entity
    {
        private string _entityTypeName;

        [JsonIgnore]
        public IEntityType EntityType { get; set; }
        public string EntityTypeName
        {

            set
            {
                _entityTypeName = value;
            }
            get
            {
                if (EntityType == null)
                {
                    if (string.IsNullOrEmpty(_entityTypeName))
                        return string.Empty;
                    return _entityTypeName;
                }

                var clrType = EntityType.ClrType;
                if (clrType.GetTypeInfo().IsGenericType)
                {
                    var typeName = clrType.Name.Substring(0, clrType.Name.IndexOf("`"));
                    var parameter = GetCollectionElementType(clrType);
                    return $"{typeName}<{parameter}>";
                }
                else
                {
                    return EntityType.ClrType.Name;
                }
            }
        }
        public int SortOrder { get; set; }
        public dynamic AllRecords { get; set; }
        private Type GetCollectionElementType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            if (type.GetTypeInfo().IsGenericType)
            {
                return type.GetTypeInfo().GetGenericArguments()[0];
            }

            throw new InvalidOperationException("Entity Configuration requires the collection to be either IEnumerable<T> or T[]");
        }
    }
}
