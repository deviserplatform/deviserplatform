using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Deviser.Detached
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Merges a graph of entities with the data store.
        /// </summary>
        /// <typeparam name="T">The type of the root entity</typeparam>
        /// <param name="context">The database context to attach / detach.</param>
        /// <param name="entity">The root entity.</param>
        /// <param name="mapping">The mapping configuration to define the bounds of the graph</param>
        /// <param name="updateParams">Update configuration overrides</param>
        /// <returns>The attached entity graph</returns>
        public static T UpdateGraph<T>(this DbContext context, T entity, Expression<Func<IUpdateConfiguration<T>, object>> mapping) where T : class
        {
            return UpdateGraph(context, entity, mapping, null, null);
        }

        public static T UpdateGraph<T>(this DbContext context, T entity, List<GraphConfig> graphConfigs) where T : class
        {
            return UpdateGraph(context, entity, null, graphConfigs, null);
        }

        /// <summary>
        /// Merges a graph of entities with the data store.
        /// </summary>
        /// <typeparam name="T">The type of the root entity</typeparam>
        /// <param name="context">The database context to attach / detach.</param>
        /// <param name="entity">The root entity.</param>
        /// <param name="mappingScheme">Pre-configured mappingScheme</param>
        /// <param name="updateParams">Update configuration overrides</param>
        /// <returns>The attached entity graph</returns>
        public static T UpdateGraph<T>(this DbContext context, T entity, string mappingScheme) where T : class
        {
            return UpdateGraph(context, entity, null, null, mappingScheme);
        }

        /// <summary>
        /// Merges a graph of entities with the data store.
        /// </summary>
        /// <typeparam name="T">The type of the root entity</typeparam>
        /// <param name="context">The database context to attach / detach.</param>
        /// <param name="entity">The root entity.</param>
        /// <param name="updateParams">Update configuration overrides</param>
        /// <returns>The attached entity graph</returns>
        public static T UpdateGraph<T>(this DbContext context, T entity) where T : class
        {
            return UpdateGraph(context, entity, null, null, null);
        }

        ///// <summary>
        ///// Load an aggregate type from the database (including all related entities)
        ///// </summary>
        ///// <typeparam name="T">Type of the entity</typeparam>
        ///// <param name="context">DbContext</param>
        ///// <param name="keyPredicate">The predicate used to find the aggregate by key</param>
        ///// <param name="queryMode">Load all objects at once, or perform multiple queries</param>
        ///// <returns>The aggregate loaded from the database</returns>
        //public static T LoadAggregate<T>(this DbContext context, Expression<Func<T, bool>> keyPredicate) where T : class
        //{
        //    var entityManager = new EntityManager(context);
        //    var graph = new AggregateRegister(new CacheProvider()).GetEntityGraph<T>();
        //    var queryLoader = new QueryLoader(context, entityManager);

        //    if (graph == null)
        //    {
        //        throw new NotSupportedException("Type: '" + typeof(T).FullName + "' is not a known aggregate");
        //    }

        //    var includeStrings = graph.GetIncludeStrings(entityManager);
        //    return queryLoader.LoadEntity(keyPredicate, includeStrings, queryMode);
        //}

        // other methods are convenience wrappers around this.
        private static T UpdateGraph<T>(this DbContext context, T entity, Expression<Func<IUpdateConfiguration<T>, object>> mapping,
            List<GraphConfig> graphConfigs, string mappingScheme) where T : class
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var entityManager = new EntityManager(context);
            //var queryLoader = new QueryLoader(context, entityManager);
            var register = new AggregateRegister(new CacheProvider<GraphNode>());

            var root = GetRootNode(mapping, graphConfigs, mappingScheme, register);
            var differ = new GraphDiffer<T>(context, entityManager, root);

            return differ.Merge(entity);
        }

        private static GraphNode GetRootNode<T>(Expression<Func<IUpdateConfiguration<T>, object>> mapping, List<GraphConfig> graphConfigs,
            string mappingScheme, AggregateRegister register) where T : class
        {
            GraphNode root = null;
            if (mapping != null)
            {
                // mapping configuration
                root = register.GetEntityGraph(mapping);
            }
            else if (graphConfigs != null)
            {
                root = register.GetEntityGraph<T>(graphConfigs);
            }
            else if (mappingScheme != null)
            {
                // names scheme
                root = register.GetEntityGraph<T>(mappingScheme);
            }
            else
            {
                // attributes or null
                root = register.GetEntityGraph<T>();
            }
            return root;
        }
    }
}
