using System;
using System.Linq;
using System.Linq.Expressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using GenericSearch.Common;
using GenericSearch.Grammar.AntlrGrammar;

namespace GenericSearch.Grammar
{
    public static class SearchExtensions_Antlr
    {
        public static SearchResult<T> FilterUsingAntlr<T>(
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

            AntlrInputStream input = new AntlrInputStream(queryString);
            SearchGrammarLexer lexer = new SearchGrammarLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            SearchGrammarParser parser = new SearchGrammarParser(tokens);
            IParseTree parseTree = parser.prog();

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

#if DEBUG
            string treeText = new StringBuilderVisitor().Visit(parseTree);
            System.Diagnostics.Trace.WriteLine(treeText);
#endif

            GrammarResult searchResult = null;

            try
            {
                ExpressionBuilderVisitor visitor = new ExpressionBuilderVisitor(memberExpressions);
                searchResult = visitor.Visit(parseTree);
            }
            catch (InvalidSearchException)
            {
                string failedTreeText = new StringBuilderVisitor().Visit(parseTree);
                System.Diagnostics.Trace.WriteLine("Failed to parse search grammar '" + queryString + "': \r\n" + failedTreeText);

                throw;
            }

            var searchPredicate = Expression.Lambda<Func<T, bool>>(searchResult.Expression, arg);

            return new SearchResult<T>(query.Where(searchPredicate), searchResult.Terms);
        }
    }
}
