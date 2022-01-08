using Irony.Parsing;

namespace GenericSearch.Grammar.IronyGrammar
{
    [Language("Search Grammar", "1.0", "A search grammar")]
    public class SearchGrammar : Irony.Parsing.Grammar
    {
        public SearchGrammar()
            : base(false)
        {
            // Terminals
            var term = new IdentifierTerminal(
                "Term",
                "!@#$%^*_'.?",
                "!@#$%^*_'.?0123456789üöäÜÖÄß");

            term.Priority = TerminalPriority.Low;
            var phrase = new StringLiteral("Phrase", "\"");

            // Keywords
            KeyTerm and = this.ToTerm("AND");
            KeyTerm or = this.ToTerm("OR");
            KeyTerm not = this.ToTerm("NOT");

            // Set precedence of operators.
            this.RegisterOperators(1, not);
            this.RegisterOperators(2, and);
            this.RegisterOperators(3, or);

            // NonTerminals
            var orExpression = new NonTerminal("OrExpression");
            var andExpression = new NonTerminal("AndExpression");
            var andOperator = new NonTerminal("AndOperator");
            var orOperator = new NonTerminal("OrOperator");
            var notOperator = new NonTerminal("NotOperator");
            var negatedExpression = new NonTerminal("NegatedExpression");
            var primaryExpression = new NonTerminal("PrimaryExpression");
            var parenthesizedExpression = new NonTerminal("ParenthesizedExpression");

            this.Root = orExpression;

            orExpression.Rule = andExpression
                | orExpression + orOperator + andExpression;

            andExpression.Rule = primaryExpression
                | andExpression + andOperator + primaryExpression;

            andOperator.Rule = this.Empty
                | and
                | "&";

            orOperator.Rule = or
                | "|";

            notOperator.Rule = not
                | "-";

            negatedExpression.Rule = notOperator + primaryExpression;

            primaryExpression.Rule = term
                | negatedExpression
                | parenthesizedExpression
                | phrase;

            parenthesizedExpression.Rule = "(" + orExpression + ")";

            this.MarkPunctuation("(", ")");
        }
    }
}