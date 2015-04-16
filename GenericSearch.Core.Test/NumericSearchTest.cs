using System.Linq;
using GenericSearch.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericSearch.Core
{
    [TestClass]
    public class NumericSearchTest
    {
        [TestMethod]
        public void ApplyToQuery_EqualInteger_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "Integer";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 10;
            criteria.Comparator = NumericComparators.Equal;

            Assert.AreEqual(2, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [TestMethod]
        public void ApplyToQuery_GreaterInteger_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "Integer";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 10;
            criteria.Comparator = NumericComparators.Greater;

            Assert.AreEqual(14, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [TestMethod]
        public void ApplyToQuery_GreaterOrEqualInteger_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "Integer";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 10;
            criteria.Comparator = NumericComparators.GreaterOrEqual;

            Assert.AreEqual(16, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [TestMethod]
        public void ApplyToQuery_LessInteger_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "Integer";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 80;
            criteria.Comparator = NumericComparators.Less;

            Assert.AreEqual(14, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [TestMethod]
        public void ApplyToQuery_LessOrEqualInteger_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "Integer";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 80;
            criteria.Comparator = NumericComparators.LessOrEqual;

            Assert.AreEqual(16, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [TestMethod]
        public void ApplyToQuery_EqualIntegerNullable_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "IntegerNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 10;
            criteria.Comparator = NumericComparators.Equal;

            Assert.AreEqual(2, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [TestMethod]
        public void ApplyToQuery_GreaterIntegerNullable_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "IntegerNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 10;
            criteria.Comparator = NumericComparators.Greater;

            Assert.AreEqual(2, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [TestMethod]
        public void ApplyToQuery_GreaterOrEqualIntegerNullable_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "IntegerNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 10;
            criteria.Comparator = NumericComparators.GreaterOrEqual;

            Assert.AreEqual(4, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [TestMethod]
        public void ApplyToQuery_LessIntegerNullable_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "IntegerNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 20;
            criteria.Comparator = NumericComparators.Less;

            Assert.AreEqual(2, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }

        [TestMethod]
        public void ApplyToQuery_LessOrEqualIntegerNullable_CorrectResultReturned()
        {
            var criteria = new NumericSearch();
            criteria.Property = "IntegerNullable";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;

            criteria.SearchTerm = 20;
            criteria.Comparator = NumericComparators.LessOrEqual;

            Assert.AreEqual(4, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }
    }
}
