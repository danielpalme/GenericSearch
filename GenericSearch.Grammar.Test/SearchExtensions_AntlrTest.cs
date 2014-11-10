using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericSearch.Grammar.Test
{
    [TestClass]
    public class SearchExtensions_AntlrTest : SearchExtensions_TestBase
    {
        public SearchExtensions_AntlrTest()
            : base(SearchExtensions_Antlr.FilterUsingAntlr)
        {
        }
    }
}
