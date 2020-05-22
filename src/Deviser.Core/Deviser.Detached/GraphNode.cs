using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Deviser.Detached
{
    internal class GraphNode
    {   
        private bool _isCollection;
        private bool _isOwned;

        protected readonly PropertyInfo Accessor;

        protected string IncludeString
        {
            get
            {
                var ownIncludeString = Accessor != null ? Accessor.Name : null;
                return Parent != null && Parent.IncludeString != null
                        ? Parent.IncludeString + "." + ownIncludeString
                        : ownIncludeString;
            }
        }
        
        public GraphNode Parent { get; set; }
        public Stack<GraphNode> Members { get; private set; }

        public GraphNode()
        {
            Members = new Stack<GraphNode>();
        }

        //public GraphNode(bool isCollection, bool isOwned)
        //    :this()
        //{
        //    _isCollection = isCollection;
        //    _isOwned = isOwned;
        //}

        public GraphNode(GraphNode parent, PropertyInfo accessor, bool isCollection, bool isOwned)
            : this()
        {
            Accessor = accessor;
            Members = new Stack<GraphNode>();
            Parent = parent;
            _isCollection = isCollection;
            _isOwned = isOwned;
        }


        public List<string> GetIncludeStrings(IEntityManager entityManager)
        {
            var includeStrings = new List<string>();
            var ownIncludeString = IncludeString;
            if (!string.IsNullOrEmpty(ownIncludeString))
            {
                includeStrings.Add(ownIncludeString);
            }

            //includeStrings.AddRange(GetRequiredNavigationPropertyIncludes(entityManager));

            foreach (var member in Members)
            {
                includeStrings.AddRange(member.GetIncludeStrings(entityManager));
            }

            return includeStrings;
        }

        public string GetUniqueKey()
        {
            var key = "";
            if (Parent != null && Parent.Accessor != null)
            {
                key += Parent.Accessor.DeclaringType.FullName + "_" + Parent.Accessor.Name;
            }
            else
            {
                key += "NoParent";
            }
            return key + "_" + Accessor.DeclaringType.FullName + "_" + Accessor.Name;
        }

        protected T GetValue<T>(object instance)
        {
            return (T)Accessor.GetValue(instance, null);
        }

        protected void SetValue(object instance, object value)
        {
            Accessor.SetValue(instance, value, null);
        }

        protected virtual IEnumerable<string> GetRequiredNavigationPropertyIncludes(IEntityManager entityManager)
        {
            if (!_isOwned)
            {
                return Accessor != null
                    ? GetRequiredNavigationPropertyIncludes(entityManager, Accessor.PropertyType, IncludeString)
                    : new string[0];
            }

            if (_isCollection)
            {
                return Accessor != null
                        ? GetRequiredNavigationPropertyIncludes(entityManager, GetCollectionElementType(), IncludeString)
                        : new string[0];
            }

            return new string[0];
        }

        protected List<string> GetMappedNaviationProperties()
        {
            return Members.Select(m => m.Accessor.Name).ToList();
        }

        public void Update<T>(IChangeTracker changeTracker, IEntityManager entityManager, T persisted, T updating) where T : class
        {
            if (Accessor == null)
            {
                changeTracker.UpdateItem(updating, persisted);

                // Foreach branch perform recursive update
                foreach (var member in Members)
                {
                    member.Update(changeTracker, entityManager, persisted, updating);
                }
            }
            else if (_isCollection)
            {
                //CollectionGraph (Association/Owned)
                var innerElementType = GetCollectionElementType();
                var updateValues = GetValue<IEnumerable>(updating) ?? new List<object>();
                var dbCollection = GetValue<IEnumerable>(persisted) ?? CreateMissingCollection(persisted, innerElementType);

                var dbHash = dbCollection.Cast<object>().ToDictionary(item => entityManager.GetEntityKey(item));
                
                // Iterate through the elements from the updated graph and try to match them against the db graph
                var updateList = updateValues.OfType<object>().ToList();
                for (var i = 0; i < updateList.Count; i++)
                {
                    var updateItem = updateList[i];
                    var key = entityManager.GetEntityKey(updateItem);

                    // try to find item with same key in db collection
                    object dbItem;
                    if (dbHash.TryGetValue(key, out dbItem))
                    {
                        UpdateElement(changeTracker, entityManager, persisted, updateItem, dbItem);
                        dbHash.Remove(key);
                    }
                    else
                    {
                        updateList[i] = AddElement(changeTracker, entityManager, persisted, updateItem, dbCollection);
                    }
                }

                // remove obsolete items
                foreach (var dbItem in dbHash.Values)
                {
                    RemoveElement(changeTracker, dbItem, dbCollection);
                }
            }
            else if (_isOwned)
            {
                //OwnedEntity

                var dbValue = GetValue<object>(persisted);
                var newValue = GetValue<object>(updating);

                if (dbValue == null && newValue == null)
                {
                    return;
                }

                // Merging options
                // 1. No new value, set value to null. entity will be removed if cascade rules set.
                // 2. If new value is same as old value lets update the members
                // 3. Otherwise new value is set and we don't care about old dbValue, so create a new one.
                if (newValue == null)
                {
                    SetValue(persisted, null);
                    return;
                }

                if (dbValue != null && entityManager.AreKeysIdentical(newValue, dbValue))
                {
                    changeTracker.UpdateItem(newValue, dbValue);
                }
                else
                {
                    dbValue = CreateNewPersistedEntity(changeTracker, persisted, newValue);
                }

                changeTracker.AttachCyclicNavigationProperty(persisted, newValue, GetMappedNaviationProperties());

                foreach (var childMember in Members)
                {
                    childMember.Update(changeTracker, entityManager, dbValue, newValue);
                }
            }
            else
            {
                //AssociatedEntity
                var dbValue = GetValue<object>(persisted);
                var newValue = GetValue<object>(updating);

                if (newValue == null)
                {
                    SetValue(persisted, null);
                    return;
                }

                // do nothing if the key is already identical
                if (entityManager.AreKeysIdentical(newValue, dbValue))
                {
                    return;
                }

                newValue = changeTracker.AttachAndReloadAssociatedEntity(newValue);
                SetValue(persisted, newValue);
            }
        }

        private void UpdateElement<T>(IChangeTracker changeTracker, IEntityManager entityManager, T existing, object updateItem, object dbItem)
        {
            if (!_isOwned)
            {
                return;
            }

            changeTracker.UpdateItem(updateItem, dbItem);
            changeTracker.AttachCyclicNavigationProperty(existing, updateItem, GetMappedNaviationProperties());

            foreach (var childMember in Members)
            {
                childMember.Update(changeTracker, entityManager, dbItem, updateItem);
            }
        }

        private object AddElement<T>(IChangeTracker changeTracker, IEntityManager entityManager, T existing, object updateItem, object dbCollection)
        {
            if (!_isOwned)
            {
                updateItem = changeTracker.AttachAndReloadAssociatedEntity(updateItem);
            }
            else if (changeTracker.GetItemState(updateItem) == EntityState.Detached)
            {
                var instance = entityManager.CreateEmptyEntityWithKey(updateItem);

                changeTracker.AddItem(instance);
                changeTracker.UpdateItem(updateItem, instance);

                foreach (var childMember in Members)
                {
                    childMember.Update(changeTracker, entityManager, instance, updateItem);
                }

                updateItem = instance;
            }

            dbCollection.GetType().GetMethod("Add").Invoke(dbCollection, new[] { updateItem });

            if (_isOwned)
            {
                changeTracker.AttachCyclicNavigationProperty(existing, updateItem, GetMappedNaviationProperties());
            }

            return updateItem;
        }

        private void RemoveElement(IChangeTracker changeTracker, object dbItem, object dbCollection)
        {
            dbCollection.GetType().GetMethod("Remove").Invoke(dbCollection, new[] { dbItem });
            changeTracker.AttachRequiredNavigationProperties(dbItem, dbItem);

            if (_isOwned)
            {
                changeTracker.RemoveItem(dbItem);
            }
        }

        protected static IEnumerable<string> GetRequiredNavigationPropertyIncludes(IEntityManager entityManager, Type clrType, string ownIncludeString)
        {
            return entityManager.GetRequiredNavigationPropertiesForType(clrType)
                .Select(navigation => ownIncludeString + "." + navigation.Name);
            //.GetRequiredNavigationPropertiesForType(entityType)
            //.Select(navigationProperty => ownIncludeString + "." + navigationProperty.Name);
        }

        protected static void ThrowIfCollectionType(PropertyInfo accessor, string mappingType)
        {
            if (IsCollectionType(accessor.PropertyType))
                throw new ArgumentException(string.Format("Collection '{0}' can not be mapped as {1} entity. Please map it as {1} collection.", accessor.Name, mappingType));
        }

        private static bool IsCollectionType(Type propertyType)
        {
            return propertyType.IsArray || typeof(IEnumerable<>).IsAssignableFrom(propertyType); //propertyType.GetInterface(typeof(IEnumerable<>).FullName) != null;
        }

        private Type GetCollectionElementType()
        {
            if (Accessor.PropertyType.IsArray)
            {
                return Accessor.PropertyType.GetElementType();
            }

            if (Accessor.PropertyType.GetTypeInfo().IsGenericType)
            {
                return Accessor.PropertyType.GetGenericArguments()[0];
            }

            throw new InvalidOperationException("GraphDiff requires the collection to be either IEnumerable<T> or T[]");
        }

        private IEnumerable CreateMissingCollection(object existing, Type elementType)
        {
            var collectionType = !Accessor.PropertyType.GetTypeInfo().IsInterface ? Accessor.PropertyType : typeof(List<>).MakeGenericType(elementType);
            var collection = (IEnumerable)Activator.CreateInstance(collectionType);
            SetValue(existing, collection);
            return collection;
        }

        private object CreateNewPersistedEntity<T>(IChangeTracker changeTracker, T existing, object newValue) where T : class
        {
            var instance = Activator.CreateInstance(newValue.GetType(), true);
            SetValue(existing, instance);
            changeTracker.AddItem(instance);
            changeTracker.UpdateItem(newValue, instance);
            return instance;
        }
    }
}
