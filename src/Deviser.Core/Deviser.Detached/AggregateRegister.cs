using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Deviser.Detached
{

    internal interface IAggregateRegister
    {
        void ClearAll();
        void Register<T>(GraphNode rootNode, string scheme = null);
        GraphNode GetEntityGraph<T>(string scheme = null);
        GraphNode GetEntityGraph<T>(Expression<Func<IUpdateConfiguration<T>, object>> expression);
    }

    internal class AggregateRegister : IAggregateRegister
    {
        private readonly ICacheProvider<GraphNode> _cache;
        private readonly IAttributeGraphBuilder _attributeGraphBuilder;

        public AggregateRegister(ICacheProvider<GraphNode> cache)
        {
            _cache = cache;
            _attributeGraphBuilder = new AttributeGraphBuilder();
        }

        public void ClearAll()
        {
            _cache.Clear();
        }

        public void Register<T>(GraphNode rootNode, string scheme = null)
        {
            _cache.Insert(GenerateCacheKey<T>(), rootNode);
        }

        public GraphNode GetEntityGraph<T>()
        {
            var node = _attributeGraphBuilder.CanBuild<T>() ? _attributeGraphBuilder.BuildGraph<T>() : new GraphNode();
            return _cache.GetOrAdd(GenerateCacheKey<T>(), node);
        }

        public GraphNode GetEntityGraph<T>(string scheme)
        {
            GraphNode node;
            if (_cache.TryGet(GenerateCacheKey<T>(), out node))
            {
                return node;
            }

            throw new ArgumentOutOfRangeException("Could not find a mapping scheme with name: '" + scheme + "'");
        }

        public GraphNode GetEntityGraph<T>(List<GraphConfig> graphConfigs)
        {
            var key = typeof(T).FullName + "_" + graphConfigs;
            return _cache.GetOrAdd(key, new ParameterGraphBuilder().BuildGraph(graphConfigs));
        }

        public GraphNode GetEntityGraph<T>(Expression<Func<IUpdateConfiguration<T>, object>> expression)
        {
            var key = typeof(T).FullName + "_" + expression;
            return _cache.GetOrAdd(key, new ConfigurationGraphBuilder().BuildGraph(expression));
        }

        private static string GenerateCacheKey<T>(string scheme = null)
        {
            var key = typeof(T).FullName;
            if (!String.IsNullOrEmpty(scheme))
            {
                key += scheme;
            }
            return key;
        }
    }
}
