using System.Linq;
using GenericSearch.Data;
using Xunit;

namespace GenericSearch.Core
{
    public class NumericSearchTest
    {
        [Fact]
        public void ApplyToQuery_EqualInteger_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "Integer";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 10;
            criteria.Comparator = NumericComparators.Equal;

            Assert.Equal(2, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_GreaterInteger_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "Integer";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 10;
            criteria.Comparator = NumericComparators.Greater;

            Assert.Equal(14, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_GreaterOrEqualInteger_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "Integer";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 10;
            criteria.Comparator = NumericComparators.GreaterOrEqual;

            Assert.Equal(16, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_LessInteger_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "Integer";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 80;
            criteria.Comparator = NumericComparators.Less;

            Assert.Equal(14, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_LessOrEqualInteger_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "Integer";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 80;
            criteria.Comparator = NumericComparators.LessOrEqual;

            Assert.Equal(16, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_EqualIntegerNullable_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "IntegerNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 10;
            criteria.Comparator = NumericComparators.Equal;

            Assert.Equal(2, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_GreaterIntegerNullable_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "IntegerNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 10;
            criteria.Comparator = NumericComparators.Greater;

            Assert.Equal(2, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_GreaterOrEqualIntegerNullable_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "IntegerNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 10;
            criteria.Comparator = NumericComparators.GreaterOrEqual;

            Assert.Equal(4, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_LessIntegerNullable_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "IntegerNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 20;
            criteria.Comparator = NumericComparators.Less;

            Assert.Equal(2, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_LessOrEqualIntegerNullable_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "IntegerNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 20;
            criteria.Comparator = NumericComparators.LessOrEqual;

            Assert.Equal(4, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }
    }
}
