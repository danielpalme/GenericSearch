using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GenericSearch.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericSearch.Core.Test
{
    [TestClass]
    public class SearchExtensionsTest
    {
        [TestMethod]
        public void ApplySearchCriteria_PassAllCriteriaWithoutSearchterm_NoElementIsFiltered()
        {
            var criteria = typeof(SomeClass).GetDefaultSearchCriteria();

            var query = new Repository().GetQuery();
            int totalElements = query.Count();

            Assert.AreEqual(totalElements, query.ApplySearchCriteria(criteria).Count());
        }

        [TestMethod]
        public void GetDefaultSearchCriteria_PassType_CorrectCriteriaReturned()
        {
            var criteria = typeof(SomeClass).GetDefaultSearchCriteria();
            Assert.AreEqual(7, criteria.Count);
            Assert.AreEqual("Date", criteria.ElementAt(0).Property);
            Assert.AreEqual("DateNullable", criteria.ElementAt(1).Property);
            Assert.AreEqual("Integer", criteria.ElementAt(2).Property);
            Assert.AreEqual("IntegerNullable", criteria.ElementAt(3).Property);
            Assert.AreEqual("MyEnum", criteria.ElementAt(4).Property);
            Assert.AreEqual("Text", criteria.ElementAt(5).Property);
            Assert.AreEqual("CollectionString", criteria.ElementAt(6).Property);

            Assert.AreEqual("Date", criteria.ElementAt(0).LabelText);
            Assert.AreEqual("DateNullable", criteria.ElementAt(1).LabelText);
            Assert.AreEqual("Integer with label", criteria.ElementAt(2).LabelText);
            Assert.AreEqual("IntegerNullable", criteria.ElementAt(3).LabelText);
            Assert.AreEqual("MyEnum", criteria.ElementAt(4).LabelText);
            Assert.AreEqual("Text", criteria.ElementAt(5).LabelText);
            Assert.AreEqual("CollectionString", criteria.ElementAt(6).LabelText);
        }

        [TestMethod]
        public void AddCustomSearchCriterion_PassNotSupportedProperty_CorrectCriterionReturned()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            criteria.AddCustomSearchCriterion<SomeClass>(o => o.CollectionComplex);

            Assert.AreEqual(0, criteria.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddCustomSearchCriterion_PassNotSupportedLambdaExpression_CorrectCriterionReturned()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            criteria.AddCustomSearchCriterion<SomeClass>(o => o);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddCustomSearchCriterion_PassNotSupportedLambdaExpression_ArgumentExceptionThrown()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            criteria.AddCustomSearchCriterion<SomeClass>(o => o.CollectionComplex.Select(i => i.Nested).Select(i => i.TextNested));
        }

        [TestMethod]
        public void AddCustomSearchCriterion_PassUnaryLambdaExpression_CorrectCriterionAdded()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            criteria.AddCustomSearchCriterion<SomeClass>(o => o.Integer);

            Assert.AreEqual(1, criteria.Count);
            Assert.AreEqual("Integer", criteria.ElementAt(0).Property);
        }

        [TestMethod]
        public void AddCustomSearchCriterion_PassPropertyLambdaExpression_CorrectCriterionAdded()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            criteria.AddCustomSearchCriterion<SomeClass>(o => o.Text);

            Assert.AreEqual(1, criteria.Count);
            Assert.AreEqual("Text", criteria.ElementAt(0).Property);
        }

        [TestMethod]
        public void AddCustomSearchCriterion_PassNestedPropertyLambdaExpression_CorrectCriterionAdded()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            criteria.AddCustomSearchCriterion<SomeClass>(o => o.Nested.TextNested);

            Assert.AreEqual(1, criteria.Count);
            Assert.AreEqual("Nested.TextNested", criteria.ElementAt(0).Property);
        }

        [TestMethod]
        public void AddCustomSearchCriterion_PassNestedPropertyInSimpleCollectionLambdaExpression_CorrectCriterionAdded()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            criteria.AddCustomSearchCriterion<SomeClass>(o => o.CollectionString);

            Assert.AreEqual(1, criteria.Count);
            Assert.AreEqual("CollectionString", criteria.ElementAt(0).Property);
        }

        [TestMethod]
        public void AddCustomSearchCriterion_PassNestedPropertyInComplexCollectionLambdaExpression_CorrectCriterionAdded()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            criteria.AddCustomSearchCriterion<SomeClass>(o => o.CollectionComplex.Select(i => i.TextNested));

            Assert.AreEqual(1, criteria.Count);
            Assert.AreEqual("CollectionComplex.TextNested", criteria.ElementAt(0).Property);
            Assert.AreEqual("TextNested with label", criteria.ElementAt(0).LabelText);
        }
    }
}
