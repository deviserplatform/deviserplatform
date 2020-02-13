using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Detached
{
    internal interface IGraphDiffer<T> where T : class
    {
        T Merge(T updating);
    }

    /// <summary>GraphDiff main entry point.</summary>
    /// <typeparam name="T">The root agreggate type</typeparam>
    internal class GraphDiffer<T> : IGraphDiffer<T> where T : class
    {
        private readonly GraphNode _root;
        private readonly DbContext _dbContext;
        private readonly IEntityManager _entityManager;
        ChangeTracker<T> _changeTracker;

        public GraphDiffer(DbContext dbContext, IEntityManager entityManager, GraphNode root)
        {
            _root = root;
            _dbContext = dbContext;
            _entityManager = entityManager;
            _changeTracker = new ChangeTracker<T>(_dbContext, entityManager);
        }

        public T Merge(T updating)
        {
            // temporally disabled autodetect changes
            bool autoDetectChanges = _dbContext.ChangeTracker.AutoDetectChangesEnabled;
            try
            {
                // performance improvement for large graphs
                _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

                // Get our entity with all includes needed, or add a new entity
                var includeStrings = _root.GetIncludeStrings(_entityManager);
                T persisted = _entityManager.LoadPersisted<T>(updating, includeStrings);

                if (persisted == null)
                {
                    // we are always working with 2 graphs, simply add a 'persisted' one if none exists,
                    // this ensures that only the changes we make within the bounds of the mapping are attempted.
                    //persisted = (T)_dbContext.Set<T>().Create();

                    //_dbContext.Set<T>().Add(persisted);

                    // set state.

                    persisted = (T)_changeTracker.AddItem(updating).Entity;

                }

                if (_dbContext.Entry(updating).State != EntityState.Detached)
                {
                    throw new InvalidOperationException(
                            String.Format("Entity of type '{0}' is already in an attached state. GraphDiff supports detached entities only at this time. Please try AsNoTracking() or detach your entites before calling the UpdateGraph method.",
                                          typeof(T).FullName));
                }

                // Perform recursive update
                var entityManager = new EntityManager(_dbContext);

                _root.Update(_changeTracker, entityManager, persisted, updating);

                return persisted;
            }
            finally
            {
                _dbContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;
            }
        }
    }
}
