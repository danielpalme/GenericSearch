using System.Linq;
using GenericSearch.Data;
using Xunit;

namespace GenericSearch.Core
{
    public class TextSearchTest
    {
        [Fact]
        public void ApplyToQuery_ContainsText_CorrectResultReturned()
        {
            var criteria = new TextSearch();
            criteria.Property = "Text";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = "abcdef";
            criteria.Comparator = TextComparators.Contains;

            Assert.Equal(2, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_EqualsText_CorrectResultReturned()
        {
            var criteria = new TextSearch();
            criteria.Property = "Text";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = "abcdef";
            criteria.Comparator = TextComparators.Equals;

            Assert.Equal(1, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_EqualsNestedText_CorrectResultReturned()
        {
            var criteria = new TextSearch();
            criteria.Property = "Nested.TextNested";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = "qwerty";
            criteria.Comparator = TextComparators.Equals;

            Assert.Equal(2, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_EqualsNestedNestedText_CorrectResultReturned()
        {
            var criteria = new TextSearch();
            criteria.Property = "Nested.Nested.TextNested";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = "qwerty";
            criteria.Comparator = TextComparators.Equals;

            Assert.Equal(0, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_EqualsTextInCollectionSimple_CorrectResultReturned()
        {
            var criteria = new TextSearch();
            criteria.Property = "CollectionString";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = "simple_123";
            criteria.Comparator = TextComparators.Equals;

            Assert.Equal(8, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_EqualsTextInCollectionComplex_CorrectResultReturned()
        {
            var criteria = new TextSearch();
            criteria.Property = "CollectionComplex.TextNested";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = "complex_678";
            criteria.Comparator = TextComparators.Equals;

            Assert.Equal(8, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }
    }
}
