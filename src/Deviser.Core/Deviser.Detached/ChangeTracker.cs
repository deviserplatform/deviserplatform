using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace Deviser.Detached
{
    internal class ChangeTracker<T> : IChangeTracker
        where T : class
    {
        private readonly DbContext _context;
        private readonly IEntityManager _entityManager;

        public ChangeTracker(DbContext context, IEntityManager entityManager)
        {
            _entityManager = entityManager;
            _context = context;
        }


        public EntityEntry AddItem(object entity)
        {
            return Add(entity, new HashSet<object>());
        }

        public object AttachAndReloadAssociatedEntity(object entity)
        {
            var localCopy = FindTrackedEntity(entity);
            if (localCopy != null)
            {
                return localCopy;
            }

            if (_context.Entry(entity).State == EntityState.Detached)
            {
                // TODO look into a possible better way of doing this, I don't particularly like it
                // will add a key-only object to the change tracker. at the moment this is being reloaded,
                // performing a db query which would impact performance
                var entityType = entity.GetType();
                //var instance = _entityManager.CreateEmptyEntityWithKey(entity);
                //_context.Attach(instance);
                //_context.Entry(instance).Reload();

                var instance = FindEntityByKey(entity);

                AttachRequiredNavigationProperties(entity, instance);
                return instance;
            }

            //if (GraphDiffConfiguration.ReloadAssociatedEntitiesWhenAttached)
            //{
            //    _context.Entry(entity).Reload();
            //}

            return entity;
        }

        public void AttachCyclicNavigationProperty(object parent, object child, List<string> mappedNavigationProperties)
        {
            if (parent == null || child == null)
            {
                return;
            }

            var parentType = parent.GetType();
            var childType = child.GetType();

            var parentNavigationProperties = _entityManager
                    .GetRequiredNavigationPropertiesForType(childType)
                    .Where(navigation => navigation.Name == parentType.Name && !mappedNavigationProperties.Contains(navigation.Name))
                    .Select(navigation => childType.GetProperty(navigation.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                    .ToList();

            if (parentNavigationProperties.Count > 1)
            {
                throw new NotSupportedException(
                        string.Format("Found ambiguous parent navigation property of type '{0}'. Map one of the parents ({1}) as an associate to disambiguate.",
                                      parentType, GetConcatenatedPropertyNames(parentNavigationProperties)));
            }

            var parentNavigationProperty = parentNavigationProperties.FirstOrDefault();
            if (parentNavigationProperty != null)
            {
                parentNavigationProperty.SetValue(child, parent, null);
            }

        }

        public void AttachRequiredNavigationProperties(object updating, object persisted)
        {
            var entityType = updating.GetType();
            foreach (var navigationProperty in _entityManager.GetRequiredNavigationPropertiesForType(updating.GetType()))
            {
                var navigationPropertyInfo = entityType.GetProperty(navigationProperty.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                var associatedEntity = navigationPropertyInfo.GetValue(updating, null);

                if (associatedEntity != null)
                {
                    // TODO this is performing a db query - look for alternative.
                    associatedEntity = FindEntityByKey(associatedEntity);
                }

                navigationPropertyInfo.SetValue(persisted, associatedEntity, null);
            }
        }

        public EntityState GetItemState(object entity)
        {
            return _context.Entry(entity).State;
        }

        public void RemoveItem(object entity)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Deleted;
            //visited.Add(entry.Entity);
        }

        public void UpdateItem(object detached, object persisted)
        {
            //if (doConcurrencyCheck && _context.Entry(to).State != EntityState.Added)
            //{
            //    EnsureConcurrency(from, to);
            //}

            var persistedEntry = _context.Entry(persisted);
            var modified = Copy(detached, persistedEntry);

            //_context.Entry(persisted).CurrentValues.SetValues(detached);
        }


        protected virtual EntityEntry Add(object detached, HashSet<object> visited)
        {
            // set state.
            //EntityEntry persisted = GetEntry(detached);
            //persisted.State = EntityState.Added;
            //return persisted;
            //var clrType = detached.GetType();

            //var contextSet = typeof(DbContext).GetMethod("Set").MakeGenericMethod(clrType).Invoke(_context, null);
            //var localValue = contextSet.GetType().GetMethod("Add").Invoke(contextSet, new object[] { detached });

            return _context.Add(detached);
        }

        EntityEntry GetEntry(object entity)
        {
            var presisted = _context.Entry(entity);
            Copy(entity, presisted);
            return presisted;
        }

        private bool Copy(object srcEntity, EntityEntry destEntry)
        {
            var modified = false;
            foreach (var property in destEntry.Properties)
            {
                if (!(property.Metadata.FieldInfo == null ||
                      property.Metadata.IsPrimaryKey() ||
                      property.Metadata.IsForeignKey() //||
                      //property.Metadata.MayBeStoreGenerated()||
                      /*property.Metadata.GetAfterSaveBehavior() == PropertySaveBehavior.Save||
                      property.Metadata.IsIgnored()*/))
                {
                    var getter = property.Metadata.GetGetter();
                    var srcValue = getter.GetClrValue(srcEntity);
                    if (srcValue != property.CurrentValue)
                    {
                        property.CurrentValue = srcValue;
                        modified = true;
                    }
                }
            }
            return modified;
        }

        private static string GetConcatenatedPropertyNames(IEnumerable<PropertyInfo> properties)
        {
            return properties.Aggregate("", (current, parentProperty) => current + string.Format("'{0}', ", parentProperty.Name)).TrimEnd(',', ' ');
        }

        private object FindTrackedEntity(object entity)
        {
            var clrType = entity.GetType();

            //var obj = _context.Entry(entity).CurrentValues.ToObject();


            var method = typeof(DbContext).GetMethods().First(m => m.Name == "Set" && m.GetParameters().Length == 0 && m.IsGenericMethod);

            var contextSet = method.MakeGenericMethod(clrType).Invoke(_context, null);
            var localValue = contextSet.GetType().GetProperty("Local").GetValue(contextSet);

            var obj = ((IEnumerable<object>)localValue).FirstOrDefault(local => _entityManager.AreKeysIdentical(local, entity));

            //var obj2 = _context.Set<T>()
            //    .Local
            //    .OfType<object>()
            //    .FirstOrDefault(local => _entityManager.AreKeysIdentical(local, entity));

            return obj;
        }

        private object FindEntityByKey(object associatedEntity)
        {            
            var keyFields = _entityManager.GetPrimaryKeyFieldsFor(associatedEntity);
            var keys = keyFields.Select(key => key.GetValue(associatedEntity, null)).ToArray();
            return _context.Find(associatedEntity.GetType(), keys);
        }
    }
}
