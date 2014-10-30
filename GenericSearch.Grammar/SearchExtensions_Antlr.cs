using System;
using System.Linq;
using System.Linq.Expressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using GenericSearch.Grammar.AntlrGrammar;

namespace GenericSearch.Grammar
{
    public static class SearchExtensions_Antlr
    {
        public static IQueryable<T> FilterUsingAntlr<T>(
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

            AntlrInputStream input = new AntlrInputStream(searchTerm);
            SearchGrammarLexer lexer = new SearchGrammarLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            SearchGrammarParser parser = new SearchGrammarParser(tokens);
            IParseTree parseTree = parser.prog();

            LambdaExpression lambda = property as LambdaExpression;
            var arg = lambda.Parameters[0];
            var propertyMemberExpression = lambda.Body as MemberExpression;

            if (propertyMemberExpression == null)
            {
                throw new ArgumentException(
                    "Please provide property member expression like 'o => o.Name'",
                    "property");
            }

            ExpressionBuilderVisitor visitor = new ExpressionBuilderVisitor(propertyMemberExpression);
            var searchExpression = visitor.Visit(parseTree);

            var searchPredicate = Expression.Lambda<Func<T, bool>>(searchExpression, arg);

            return query
                .Where(searchPredicate);
        }

        private class ExpressionBuilderVisitor : SearchGrammarBaseVisitor<Expression>
        {
            private readonly MemberExpression property;

            public ExpressionBuilderVisitor(MemberExpression property)
            {
                if (property == null)
                {
                    throw new ArgumentNullException("property");
                }

                this.property = property;
            }

            public override Expression VisitOrExpression(SearchGrammarParser.OrExpressionContext context)
            {
                if (context.ChildCount == 1)
                {
                    return base.VisitOrExpression(context);
                }
                else
                {
                    return Expression.OrElse(
                        base.Visit(context.children[0]),
                        base.Visit(context.children[2]));
                }
            }

            public override Expression VisitAndExpression(SearchGrammarParser.AndExpressionContext context)
            {
                if (context.ChildCount == 1)
                {
                    return base.VisitAndExpression(context);
                }
                else if (context.ChildCount == 2)
                {
                    return Expression.AndAlso(
                        base.Visit(context.children[0]),
                        base.Visit(context.children[1]));
                }
                else
                {
                    return Expression.AndAlso(
                        base.Visit(context.children[0]),
                        base.Visit(context.children[2]));
                }
            }

            public override Expression VisitParenthesizedExpression(SearchGrammarParser.ParenthesizedExpressionContext context)
            {
                base.Visit(context.children[0]);
                base.Visit(context.children[2]);

                return base.Visit(context.children[1]);
            }

            public override Expression VisitTerminal(ITerminalNode node)
            {
                return this.CreateTextExpression(node.GetText());
            }

            public override Expression VisitPhraseExpression(SearchGrammarParser.PhraseExpressionContext context)
            {
                base.Visit(context.children[0]);
                base.Visit(context.children[context.ChildCount - 1]);

                string searchTerm = string.Join(
                    " ",
                    context.children
                        .Skip(1)
                        .Take(context.ChildCount - 2)
                        .Select(c => c.GetText()));

                return this.CreateTextExpression(searchTerm);
            }

            public override Expression VisitNegatedExpression(SearchGrammarParser.NegatedExpressionContext context)
            {
                var childExpression = base.VisitNegatedExpression(context);
                return Expression.Not(childExpression);
            }

            public override Expression VisitErrorNode(IErrorNode node)
            {
                throw new InvalidSearchException("The search term is invalid:\r\n" + node);
            }

            private Expression CreateTextExpression(string searchTerm)
            {
                var nullCheckExpression = Expression.NotEqual(this.property, Expression.Constant(null));

                var searchExpression = Expression.GreaterThan(
                    Expression.Call(
                        this.property,
                        typeof(string).GetMethod("IndexOf", new[] { typeof(string), typeof(StringComparison) }),
                        Expression.Constant(searchTerm),
                        Expression.Constant(StringComparison.CurrentCultureIgnoreCase)),
                    Expression.Constant(-1));

                return Expression.AndAlso(nullCheckExpression, searchExpression);
            }
        }
    }
}
