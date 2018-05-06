using System.Linq;
using GenericSearch.Data;
using Xunit;

namespace GenericSearch.Core
{
    public class DateSearchTest
    {
        [Fact]
        public void ApplyToQuery_EqualDate_CorrectResultReturned()
        {
            var criteria = new DateSearch();
            criteria.Property = "Date";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = new System.DateTime(2000, 1, 1);
            criteria.SearchTerm2 = null;
            criteria.Comparator = DateComparators.Equal;

            Assert.Equal(2, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_GreaterDate_CorrectResultReturned()
        {
            var criteria = new DateSearch();
            criteria.Property = "Date";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = new System.DateTime(2000, 1, 1);
            criteria.SearchTerm2 = null;
            criteria.Comparator = DateComparators.Greater;

            Assert.Equal(14, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_GreaterOrEqualDate_CorrectResultReturned()
        {
            var criteria = new DateSearch();
            criteria.Property = "Date";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = new System.DateTime(2000, 1, 1);
            criteria.SearchTerm2 = null;
            criteria.Comparator = DateComparators.GreaterOrEqual;

            Assert.Equal(16, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_LessDate_CorrectResultReturned()
        {
            var criteria = new DateSearch();
            criteria.Property = "Date";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = new System.DateTime(2007, 8, 1);
            criteria.SearchTerm2 = null;
            criteria.Comparator = DateComparators.Less;

            Assert.Equal(14, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_LessOrEqualDate_CorrectResultReturned()
        {
            var criteria = new DateSearch();
            criteria.Property = "Date";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = new System.DateTime(2007, 8, 1);
            criteria.SearchTerm2 = null;
            criteria.Comparator = DateComparators.LessOrEqual;

            Assert.Equal(16, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_InRangeDate_CorrectResultReturned()
        {
            var criteria = new DateSearch();
            criteria.Property = "Date";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = new System.DateTime(2001, 2, 1);
            criteria.SearchTerm2 = new System.DateTime(2006, 7, 1);
            criteria.Comparator = DateComparators.InRange;

            Assert.Equal(12, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_EqualDateNullable_CorrectResultReturned()
        {
            var criteria = new DateSearch();
            criteria.Property = "DateNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = new System.DateTime(2000, 1, 1);
            criteria.SearchTerm2 = null;
            criteria.Comparator = DateComparators.Equal;

            Assert.Equal(2, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_GreaterDateNullable_CorrectResultReturned()
        {
            var criteria = new DateSearch();
            criteria.Property = "DateNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = new System.DateTime(2000, 1, 1);
            criteria.SearchTerm2 = null;
            criteria.Comparator = DateComparators.Greater;

            Assert.Equal(6, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_GreaterOrEqualDateNullable_CorrectResultReturned()
        {
            var criteria = new DateSearch();
            criteria.Property = "DateNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = new System.DateTime(2000, 1, 1);
            criteria.SearchTerm2 = null;
            criteria.Comparator = DateComparators.GreaterOrEqual;

            Assert.Equal(8, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_LessDateNullable_CorrectResultReturned()
        {
            var criteria = new DateSearch();
            criteria.Property = "DateNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = new System.DateTime(2007, 8, 1);
            criteria.SearchTerm2 = null;
            criteria.Comparator = DateComparators.Less;

            Assert.Equal(8, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_LessOrEqualDateNullable_CorrectResultReturned()
        {
            var criteria = new DateSearch();
            criteria.Property = "DateNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = new System.DateTime(2007, 8, 1);
            criteria.SearchTerm2 = null;
            criteria.Comparator = DateComparators.LessOrEqual;

            Assert.Equal(8, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [Fact]
        public void ApplyToQuery_InRangeDateNullable_CorrectResultReturned()
        {
            var criteria = new DateSearch();
            criteria.Property = "DateNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = new System.DateTime(2001, 2, 1);
            criteria.SearchTerm2 = new System.DateTime(2006, 7, 1);
            criteria.Comparator = DateComparators.InRange;

            Assert.Equal(6, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }
    }
}
