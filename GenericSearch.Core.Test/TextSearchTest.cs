using System.Linq;
using GenericSearch.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericSearch.Core
{
    [TestClass]
    public class TextSearchTest
    {
        [TestMethod]
        public void ApplyToQuery_ContainsText_CorrectResultReturned()
        {
            var criteria = new TextSearch();
            criteria.Property = "Text";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = "abcdef";
            criteria.Comparator = TextComparators.Contains;

            Assert.AreEqual(2, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [TestMethod]
        public void ApplyToQuery_EqualsText_CorrectResultReturned()
        {
            var criteria = new TextSearch();
            criteria.Property = "Text";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = "abcdef";
            criteria.Comparator = TextComparators.Equals;

            Assert.AreEqual(1, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [TestMethod]
        public void ApplyToQuery_EqualsNestedText_CorrectResultReturned()
        {
            var criteria = new TextSearch();
            criteria.Property = "Nested.TextNested";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = "qwerty";
            criteria.Comparator = TextComparators.Equals;

            Assert.AreEqual(2, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [TestMethod]
        public void ApplyToQuery_EqualsTextInCollectionSimple_CorrectResultReturned()
        {
            var criteria = new TextSearch();
            criteria.Property = "CollectionString";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = "simple_123";
            criteria.Comparator = TextComparators.Equals;

            Assert.AreEqual(8, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }
    }
}
