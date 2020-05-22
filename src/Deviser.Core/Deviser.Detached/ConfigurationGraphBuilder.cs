using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Deviser.Detached
{
    internal class ConfigurationGraphBuilder : ExpressionVisitor
    {
        private GraphNode _currentMember;
        private string _currentMethod = "";

        public GraphNode BuildGraph<T>(Expression<Func<IUpdateConfiguration<T>, object>> expression)
        {
            var initialNode = new GraphNode();
            _currentMember = initialNode;
            Visit(expression);
            return initialNode;
        }

        protected override Expression VisitMember(MemberExpression memberExpression)
        {
            var accessor = GetMemberAccessor(memberExpression);
            var newMember = CreateNewMember(accessor);

            _currentMember.Members.Push(newMember);
            _currentMember = newMember;

            return base.VisitMember(memberExpression);
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            _currentMethod = expression.Method.Name;

            // go left to right in the subtree (ignore first argument for now)
            for (var i = 1; i < expression.Arguments.Count; i++)
            {
                Visit(expression.Arguments[i]);
            }

            // go back up the tree and continue
            _currentMember = _currentMember.Parent;
            return Visit(expression.Arguments[0]);
        }

        private GraphNode CreateNewMember(PropertyInfo accessor)
        {
            GraphNode newMember;
            switch (_currentMethod)
            {
                case "OwnedEntity":
                    newMember = new GraphNode(_currentMember, accessor, false, true);// GraphNodeFactory.Create(_currentMember, accessor, false, true);
                    break;
                case "AssociatedEntity":
                    newMember = new GraphNode(_currentMember, accessor, false, false); //GraphNodeFactory.Create(_currentMember, accessor, false, false);
                    break;
                case "OwnedCollection":
                    newMember = new GraphNode(_currentMember, accessor, true, true);//GraphNodeFactory.Create(_currentMember, accessor, true, true);
                    break;
                case "AssociatedCollection":
                    newMember = new GraphNode(_currentMember, accessor, true,false);//GraphNodeFactory.Create(_currentMember, accessor, true, false);
                    break;
                default:
                    throw new NotSupportedException("The method used in the update mapping is not supported");
            }
            return newMember;
        }

        private static PropertyInfo GetMemberAccessor(MemberExpression memberExpression)
        {
            PropertyInfo accessor = null;
            var expression = memberExpression.Expression;
            var constantExpression = expression as ConstantExpression;

            //if (constantExpression != null)
            //{
            //    var container = constantExpression.Value;
            //    var member = memberExpression.Member;

            //    var fieldInfo = member as FieldInfo;
            //    if (fieldInfo != null)
            //    {
            //        dynamic value = fieldInfo.GetValue(container);
            //        accessor = (PropertyInfo)value.Body.Member;
            //    }

            //    var info = member as PropertyInfo;
            //    if (info != null)
            //    {
            //        dynamic value = info.GetValue(container, null);
            //        accessor = (PropertyInfo)value.Body.Member;
            //    }
            //}
            //else
            //{
                accessor = (PropertyInfo)memberExpression.Member;
            //}

            if (accessor == null)
                throw new NotSupportedException("Unknown accessor type found!");

            return accessor;
        }
    }
}
