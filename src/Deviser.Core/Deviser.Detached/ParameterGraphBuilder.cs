using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Deviser.Detached
{
    internal class ParameterGraphBuilder
    {
        public GraphNode BuildGraph(List<GraphConfig> graphConfigs)
        {
            var parent = new GraphNode();

            foreach (var config in graphConfigs)
            {
                GraphNode newMember;
                var accessor = (PropertyInfo)config.FieldExpression.Member;
                switch (config.GraphConfigType)
                {
                    case GraphConfigType.OwnedEntity:
                        newMember = new GraphNode(parent, accessor, false, true);// GraphNodeFactory.Create(_currentMember, accessor, false, true);
                        break;
                    case GraphConfigType.AssociatedEntity:
                        newMember = new GraphNode(parent, accessor, false, false); //GraphNodeFactory.Create(_currentMember, accessor, false, false);
                        break;
                    case GraphConfigType.OwnedCollection:
                        newMember = new GraphNode(parent, accessor, true, true);//GraphNodeFactory.Create(_currentMember, accessor, true, true);
                        break;
                    case GraphConfigType.AssociatedCollection:
                        newMember = new GraphNode(parent, accessor, true, false);//GraphNodeFactory.Create(_currentMember, accessor, true, false);
                        break;
                    default:
                        throw new NotSupportedException("The method used in the update mapping is not supported");
                }
                parent.Members.Push(newMember);
            }
            return parent;
        }
    }

    public class GraphConfig
    {
        public GraphConfigType GraphConfigType { get; set; }
        public MemberExpression FieldExpression { get; set; }
    }

    public enum GraphConfigType
    {
        OwnedEntity,
        AssociatedEntity,
        OwnedCollection,
        AssociatedCollection
    }
}
