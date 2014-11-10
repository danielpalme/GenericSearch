using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericSearch.Grammar.Test
{
    [TestClass]
    public class SearchExtensions_IronyTest : SearchExtensions_TestBase
    {
        public SearchExtensions_IronyTest()
            : base(SearchExtensions_Irony.FilterUsingIrony)
        {
        }
    }
}
