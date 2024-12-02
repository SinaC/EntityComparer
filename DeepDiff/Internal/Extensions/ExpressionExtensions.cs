using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DeepDiff.Internal.Extensions
{
    internal static class ExpressionExtensions
    {
        public static PropertyPath GetSimplePropertyAccess(this LambdaExpression propertyAccessExpression)
        {
            var propertyPath
                = propertyAccessExpression
                    .Parameters
                    .Single()
                    .MatchSimplePropertyAccess(propertyAccessExpression.Body);

            if (propertyPath == null)
            {
                throw new InvalidOperationException(string.Format("The expression '{0}' is not a valid property expression. The expression should represent a property: C#: 't => t.MyProperty'  VB.Net: 'Function(t) t.MyProperty'.", propertyAccessExpression));
            }

            return propertyPath;
        }

        public static IEnumerable<PropertyPath> GetSimplePropertyAccessList(this LambdaExpression propertyAccessExpression)
        {
            var propertyPaths
                = propertyAccessExpression.MatchPropertyAccessList((p, e) => e.MatchSimplePropertyAccess(p));

            if (propertyPaths == null)
            {
                throw new InvalidOperationException(string.Format("The properties expression '{0}' is not valid. The expression should represent a property: C#: 't => t.MyProperty'  VB.Net: 'Function(t) t.MyProperty'. When specifying multiple properties use an anonymous type: C#: 't => new {{ t.MyProperty1, t.MyProperty2 }}'  VB.Net: 'Function(t) New With {{ t.MyProperty1, t.MyProperty2 }}'.", propertyAccessExpression));
            }

            return propertyPaths;
        }

        private static IEnumerable<PropertyPath> MatchPropertyAccessList(
           this LambdaExpression lambdaExpression, Func<Expression, Expression, PropertyPath> propertyMatcher)
        {
            var newExpression
                = lambdaExpression.Body.RemoveConvert() as NewExpression;

            if (newExpression != null)
            {
                var parameterExpression
                    = lambdaExpression.Parameters.Single();

                var propertyPaths
                    = newExpression.Arguments
                                   .Select(a => propertyMatcher(a, parameterExpression))
                                   .Where(p => p != null);

                if (propertyPaths.Count()
                    == newExpression.Arguments.Count())
                {
                    return newExpression.HasDefaultMembersOnly(propertyPaths) ? propertyPaths : null;
                }
            }

            var propertyPath = propertyMatcher(lambdaExpression.Body, lambdaExpression.Parameters.Single());

            return propertyPath != null ? new[] { propertyPath } : null;
        }

        private static PropertyPath MatchSimplePropertyAccess(
            this Expression parameterExpression, Expression propertyAccessExpression)
        {
            var propertyPath = parameterExpression.MatchPropertyAccess(propertyAccessExpression);

            return propertyPath != null && propertyPath.Count == 1 ? propertyPath : null;
        }

        public static Expression RemoveConvert(this Expression expression)
        {
            while (expression.NodeType == ExpressionType.Convert
                   || expression.NodeType == ExpressionType.ConvertChecked)
            {
                expression = ((UnaryExpression)expression).Operand;
            }

            return expression;
        }

        private static bool HasDefaultMembersOnly(
            this NewExpression newExpression, IEnumerable<PropertyPath> propertyPaths)
        {
            return newExpression.Members == null
                   || !newExpression.Members
                       .Where(
                           (t, i) =>
                               !string.Equals(t.Name, propertyPaths.ElementAt(i).Last().Name, StringComparison.Ordinal))
                       .Any();
        }

        private static PropertyPath MatchPropertyAccess(
            this Expression parameterExpression, Expression propertyAccessExpression)
        {
            var propertyInfos = new List<PropertyInfo>();

            MemberExpression memberExpression;

            do
            {
                memberExpression = propertyAccessExpression.RemoveConvert() as MemberExpression;

                if (memberExpression == null)
                {
                    return null;
                }

                var propertyInfo = memberExpression.Member as PropertyInfo;

                if (propertyInfo == null)
                {
                    return null;
                }

                propertyInfos.Insert(0, propertyInfo);

                propertyAccessExpression = memberExpression.Expression;
            }
            while (memberExpression.Expression != parameterExpression);

            return new PropertyPath(propertyInfos);
        }
    }
}