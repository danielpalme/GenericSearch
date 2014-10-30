using System;
using System.Linq;
using System.Linq.Expressions;
using GenericSearch.Grammar.IronyGrammar;
using Irony.Parsing;

namespace GenericSearch.Grammar
{
    public static class SearchExtensions_Irony
    {
        public static IQueryable<T> FilterUsingIrony<T>(
            this IQueryable<T> query,
            Expression<Func<T, string>> property,
            string searchTerm)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return query;
            }

            SearchGrammar searchGrammar = new SearchGrammar();
            Parser parser = new Parser(searchGrammar);
            ParseTree parseTree = parser.Parse(searchTerm);

            if (parseTree.HasErrors() || parseTree.Root == null)
            {
                string errors = string.Join(
                    "\r\n",
                    parseTree.ParserMessages
                        .Where(m => m.Level == Irony.ErrorLevel.Error)
                        .Select(m => m.Message));

                throw new InvalidSearchException("The search term is invalid:\r\n" + errors);
            }

            LambdaExpression lambda = property as LambdaExpression;
            var arg = lambda.Parameters[0];
            var propertyMemberExpression = lambda.Body as MemberExpression;

            if (propertyMemberExpression == null)
            {
                throw new ArgumentException(
                    "Please provide property member expression like 'o => o.Name'",
                    "property");
            }

            var searchExpression = CreateChildExpression(parseTree.Root, propertyMemberExpression);
            var searchPredicate = Expression.Lambda<Func<T, bool>>(searchExpression, arg);

            return query
                .Where(searchPredicate);
        }

        private static Expression CreateChildExpression(ParseTreeNode node, MemberExpression property)
        {
            switch (node.Term.Name)
            {
                case "OrExpression":
                    if (node.ChildNodes.Count == 1)
                    {
                        return CreateChildExpression(node.ChildNodes[0], property);
                    }
                    else
                    {
                        return Expression.OrElse(
                            CreateChildExpression(node.ChildNodes[0], property),
                            CreateChildExpression(node.ChildNodes[2], property));
                    }

                case "AndExpression":
                    if (node.ChildNodes.Count == 1)
                    {
                        return CreateChildExpression(node.ChildNodes[0], property);
                    }
                    else
                    {
                        return Expression.AndAlso(
                            CreateChildExpression(node.ChildNodes[0], property),
                            CreateChildExpression(node.ChildNodes[2], property));
                    }

                case "NegatedExpression":
                    var childExpression = CreateChildExpression(node.ChildNodes[1], property);
                    return Expression.Not(childExpression);

                case "ParenthesizedExpression":
                case "PrimaryExpression":
                    return CreateChildExpression(node.ChildNodes[0], property);

                case "Phrase":
                case "Term":
                    var nullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));

                    var searchExpression = Expression.GreaterThan(
                        Expression.Call(
                            property,
                            typeof(string).GetMethod("IndexOf", new[] { typeof(string), typeof(StringComparison) }),
                            Expression.Constant(node.Token.Value),
                            Expression.Constant(StringComparison.CurrentCultureIgnoreCase)),
                        Expression.Constant(-1));

                    return Expression.AndAlso(nullCheckExpression, searchExpression);

                default:
                    throw new InvalidOperationException("Grammar element '" + node.Term.Name + "' is not handled correctly. Please investigate.");
            }
        }
    }
}
