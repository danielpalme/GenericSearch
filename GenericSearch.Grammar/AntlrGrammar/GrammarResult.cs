using System.Collections.Generic;
using System.Linq.Expressions;

namespace GenericSearch.Grammar.AntlrGrammar
{
    public class GrammarResult
    {
        public GrammarResult(Expression expression, IEnumerable<string> terms)
        {
            this.Expression = expression;
            this.Terms = terms;
        }

        public Expression Expression { get; private set; }

        public IEnumerable<string> Terms { get; private set; }
    }
}
