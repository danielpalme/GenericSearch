using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Antlr4.Runtime.Tree;
using GenericSearch.Common;

namespace GenericSearch.Grammar.AntlrGrammar
{
    internal class ExpressionBuilderVisitor : SearchGrammarBaseVisitor<GrammarResult>
    {
        private readonly MemberExpression[] properties;

        private readonly HashSet<string> terms = new HashSet<string>();

        private bool trackTerms = true;

        public ExpressionBuilderVisitor(params MemberExpression[] properties)
        {
            this.properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public override GrammarResult VisitOrExpression(SearchGrammarParser.OrExpressionContext context)
        {
            if (context.ChildCount == 1)
            {
                return base.VisitOrExpression(context);
            }
            else
            {
                var expression = Expression.OrElse(
                    this.Visit(context.children[0]).Expression,
                    this.Visit(context.children[context.ChildCount - 1]).Expression);

                return new GrammarResult(expression, this.terms);
            }
        }

        public override GrammarResult VisitAndExpression(SearchGrammarParser.AndExpressionContext context)
        {
            if (context.ChildCount == 1)
            {
                return base.VisitAndExpression(context);
            }
            else
            {
                var expression = Expression.AndAlso(
                    this.Visit(context.children[0]).Expression,
                    this.Visit(context.children[context.ChildCount - 1]).Expression);

                return new GrammarResult(expression, this.terms);
            }
        }

        public override GrammarResult VisitPrimaryExpression(SearchGrammarParser.PrimaryExpressionContext context)
        {
            if (context.ChildCount == 0)
            {
                throw new InvalidSearchException("The search term is invalid:\r\n" + context.Parent.GetText());
            }

            return base.VisitPrimaryExpression(context);
        }

        public override GrammarResult VisitParenthesizedExpression(SearchGrammarParser.ParenthesizedExpressionContext context)
        {
            this.trackTerms = false;
            this.Visit(context.children[0]);
            this.Visit(context.children[2]);
            this.trackTerms = true;

            return this.Visit(context.children[1]);
        }

        public override GrammarResult VisitTerminal(ITerminalNode node)
        {
            var expression = this.CreateTextExpression(node.GetText());

            return new GrammarResult(expression, this.terms);
        }

        public override GrammarResult VisitPhraseExpression(SearchGrammarParser.PhraseExpressionContext context)
        {
            this.trackTerms = false;
            base.VisitPhraseExpression(context);
            this.trackTerms = true;

            string searchTerm = string.Join(
                string.Empty,
                context.children
                    .Skip(1)
                    .Take(context.ChildCount - 2)
                    .Select(c => c.GetText()));

            var expression = this.CreateTextExpression(searchTerm);

            return new GrammarResult(expression, this.terms);
        }

        public override GrammarResult VisitNegatedExpression(SearchGrammarParser.NegatedExpressionContext context)
        {
            var childExpression = this.Visit(context.children[context.ChildCount - 1]).Expression;
            var expression = Expression.Not(childExpression);

            return new GrammarResult(expression, this.terms);
        }

        public override GrammarResult VisitErrorNode(IErrorNode node)
        {
            throw new InvalidSearchException("The search term is invalid:\r\n" + node);
        }

        private Expression CreateTextExpression(string searchTerm)
        {
            if (this.trackTerms && !string.IsNullOrWhiteSpace(searchTerm))
            {
                this.terms.Add(searchTerm.ToLowerInvariant());
            }

            Expression searchExpression = Expression.Constant(false);

            foreach (var property in this.properties)
            {
                var nullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));

                var containsExpression = Expression.Call(
                    property,
                    typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                    Expression.Constant(searchTerm));

                var combinedExpression = Expression.AndAlso(nullCheckExpression, containsExpression);

                searchExpression = Expression.OrElse(searchExpression, combinedExpression);
            }

            return searchExpression;
        }
    }
}
