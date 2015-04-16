using System;
using System.Text;
using Antlr4.Runtime.Tree;

namespace GenericSearch.Grammar.AntlrGrammar
{
    internal class StringBuilderVisitor : SearchGrammarBaseVisitor<string>
    {
        private StringBuilder stringBuilder = new StringBuilder();

        private int currentDepth = 0;

        public override string Visit(IParseTree tree)
        {
            this.stringBuilder = new StringBuilder();

            base.Visit(tree);

            return this.stringBuilder.ToString();
        }

        public override string VisitOrExpression(SearchGrammarParser.OrExpressionContext context)
        {
            this.stringBuilder.AppendLine(new string(' ', this.currentDepth) + "OrExpression");
            this.currentDepth += 2;

            var result = base.VisitOrExpression(context);

            this.currentDepth -= 2;

            return result;
        }

        public override string VisitAndExpression(SearchGrammarParser.AndExpressionContext context)
        {
            this.stringBuilder.AppendLine(new string(' ', this.currentDepth) + "AndExpression");
            this.currentDepth += 2;

            var result = base.VisitAndExpression(context);

            this.currentDepth -= 2;

            return result;
        }

        public override string VisitPrimaryExpression(SearchGrammarParser.PrimaryExpressionContext context)
        {
            this.stringBuilder.AppendLine(new string(' ', this.currentDepth) + "PrimaryExpression");
            this.currentDepth += 2;

            var result = base.VisitPrimaryExpression(context);

            this.currentDepth -= 2;

            return result;
        }

        public override string VisitParenthesizedExpression(SearchGrammarParser.ParenthesizedExpressionContext context)
        {
            this.stringBuilder.AppendLine(new string(' ', this.currentDepth) + "ParenthesizedExpression");
            this.currentDepth += 2;

            var result = base.VisitParenthesizedExpression(context);

            this.currentDepth -= 2;

            return result;
        }

        public override string VisitTerminal(ITerminalNode node)
        {
            this.stringBuilder.AppendLine(new string(' ', this.currentDepth) + "Terminal: '" + node.GetText() + "'");
            return null;
        }

        public override string VisitPhraseExpression(SearchGrammarParser.PhraseExpressionContext context)
        {
            this.stringBuilder.AppendLine(new string(' ', this.currentDepth) + "PhraseExpression");
            this.currentDepth += 2;

            var result = base.VisitPhraseExpression(context);

            this.currentDepth -= 2;

            return result;
        }

        public override string VisitNegatedExpression(SearchGrammarParser.NegatedExpressionContext context)
        {
            this.stringBuilder.AppendLine(new string(' ', this.currentDepth) + "NegatedExpression" + "'");
            this.currentDepth += 2;

            var result = base.VisitNegatedExpression(context);

            this.currentDepth -= 2;

            return result;
        }

        public override string VisitErrorNode(IErrorNode node)
        {
            this.stringBuilder.AppendLine(new string(' ', this.currentDepth) + "Error: '" + node.GetText());
            return null;
        }
    }
}
