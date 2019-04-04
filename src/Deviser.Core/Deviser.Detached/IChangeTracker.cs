using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Detached
{
    internal interface IChangeTracker
    {
        /// <summary>Adds a new entity to the change tracker</summary>
        /// <param name="entity">The new entity</param>
        EntityEntry AddItem(object entity);

        /// <summary>Updates the values of an existing tracked entity</summary>
        /// <param name="from">The old item</param>
        /// <param name="to">The new item values to apply</param>
        /// <param name="doConcurrencyCheck">Perform a concurrency check when updating</param>
        void UpdateItem(object from, object to);

        /// <summary>Marks an entity as requiring removal from the database</summary>
        /// <param name="entity">The entity to be removed</param>
        void RemoveItem(object entity);

        /// <summary>Returns the current state of an entity (detached, attached, etc)</summary>
        EntityState GetItemState(object entity);

        /// <summary>Attach the associated entity to the change tracker and reload the entity.</summary>
        object AttachAndReloadAssociatedEntity(object entity);

        /// <summary>Ensure references back to the parent from the child are kept in sync</summary>
        void AttachCyclicNavigationProperty(object parent, object child, List<string> mappedNavigationProperties);

        /// <summary>Ensures all required navigation properties are attached</summary>
        void AttachRequiredNavigationProperties(object updating, object persisted);
    }
}
