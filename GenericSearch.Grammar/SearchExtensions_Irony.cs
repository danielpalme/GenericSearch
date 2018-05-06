using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GenericSearch.Common;
using GenericSearch.Grammar.IronyGrammar;
using Irony.Parsing;

namespace GenericSearch.Grammar
{
    public static class SearchExtensions_Irony
    {
        public static SearchResult<T> FilterUsingIrony<T>(
            this IQueryable<T> query,
            string queryString,
            params Expression<Func<T, string>>[] properties)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            if (properties.Length == 0)
            {
                throw new ArgumentException("At least one property is expected", nameof(properties));
            }

            if (string.IsNullOrWhiteSpace(queryString))
            {
                return new SearchResult<T>(query, Enumerable.Empty<string>());
            }

            SearchGrammar searchGrammar = new SearchGrammar();
            Parser parser = new Parser(searchGrammar);
            ParseTree parseTree = parser.Parse(queryString);

            if (parseTree.HasErrors() || parseTree.Root == null)
            {
                System.Diagnostics.Trace.WriteLine("Failed to parse search grammar '" + queryString);
                string errors = string.Join(
                    "\r\n",
                    parseTree.ParserMessages
                        .Where(m => m.Level == Irony.ErrorLevel.Error)
                        .Select(m => m.Message));

                throw new InvalidSearchException("The search term is invalid:\r\n" + errors);
            }

            var arg = Expression.Parameter(typeof(T), "p");
            MemberExpression[] memberExpressions = new MemberExpression[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                var propertyMemberExpression = properties[i].Body as MemberExpression;

                if (propertyMemberExpression == null)
                {
                    throw new ArgumentException(
                        "The " + i + "th property is invalid. Please provide property member expression like 'o => o.Name'",
                        nameof(properties));
                }

                string propertyString = propertyMemberExpression.ToString();
                string name = propertyString.Substring(propertyString.IndexOf('.') + 1);

                memberExpressions[i] = Expression.Property(arg, name);
            }

            var terms = new HashSet<string>();

            var searchExpression = CreateChildExpression(parseTree.Root, memberExpressions, terms);
            var searchPredicate = Expression.Lambda<Func<T, bool>>(searchExpression, arg);

            return new SearchResult<T>(query.Where(searchPredicate), terms);
        }

        private static Expression CreateChildExpression(ParseTreeNode node, MemberExpression[] properties, HashSet<string> terms)
        {
            switch (node.Term.Name)
            {
                case "OrExpression":
                    if (node.ChildNodes.Count == 1)
                    {
                        return CreateChildExpression(node.ChildNodes[0], properties, terms);
                    }
                    else
                    {
                        return Expression.OrElse(
                            CreateChildExpression(node.ChildNodes[0], properties, terms),
                            CreateChildExpression(node.ChildNodes[2], properties, terms));
                    }

                case "AndExpression":
                    if (node.ChildNodes.Count == 1)
                    {
                        return CreateChildExpression(node.ChildNodes[0], properties, terms);
                    }
                    else
                    {
                        return Expression.AndAlso(
                            CreateChildExpression(node.ChildNodes[0], properties, terms),
                            CreateChildExpression(node.ChildNodes[2], properties, terms));
                    }

                case "NegatedExpression":
                    var childExpression = CreateChildExpression(node.ChildNodes[1], properties, terms);
                    return Expression.Not(childExpression);

                case "ParenthesizedExpression":
                case "PrimaryExpression":
                    return CreateChildExpression(node.ChildNodes[0], properties, terms);

                case "Phrase":
                case "Term":
                    terms.Add(node.Token.Value.ToString());

                    Expression searchExpression = Expression.Constant(false);

                    foreach (var property in properties)
                    {
                        var nullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));

                        var containsExpression = Expression.Call(
                            property,
                            typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                            Expression.Constant(node.Token.Value));

                        var combinedExpression = Expression.AndAlso(nullCheckExpression, containsExpression);

                        searchExpression = Expression.OrElse(searchExpression, combinedExpression);
                    }

                    return searchExpression;

                default:
                    throw new InvalidOperationException("Grammar element '" + node.Term.Name + "' is not handled correctly. Please investigate.");
            }
        }
    }
}
