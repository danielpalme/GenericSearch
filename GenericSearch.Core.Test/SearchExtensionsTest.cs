using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GenericSearch.Data;
using Xunit;

namespace GenericSearch.Core.Test
{
    public class SearchExtensionsTest
    {
        [Fact]
        public void ApplySearchCriteria_PassAllCriteriaWithoutSearchterm_NoElementIsFiltered()
        {
            var criteria = typeof(SomeClass).GetDefaultSearchCriteria();

            var query = new Repository().GetQuery();
            int totalElements = query.Count();

            Assert.Equal(totalElements, query.ApplySearchCriteria(criteria).Count());
        }

        [Fact]
        public void GetDefaultSearchCriteria_PassType_CorrectCriteriaReturned()
        {
            var criteria = typeof(SomeClass).GetDefaultSearchCriteria();
            Assert.Equal(7, criteria.Count);
            Assert.Equal("Date", criteria.ElementAt(0).Property);
            Assert.Equal("DateNullable", criteria.ElementAt(1).Property);
            Assert.Equal("Integer", criteria.ElementAt(2).Property);
            Assert.Equal("IntegerNullable", criteria.ElementAt(3).Property);
            Assert.Equal("MyEnum", criteria.ElementAt(4).Property);
            Assert.Equal("Text", criteria.ElementAt(5).Property);
            Assert.Equal("CollectionString", criteria.ElementAt(6).Property);

            Assert.Equal("Date", criteria.ElementAt(0).LabelText);
            Assert.Equal("DateNullable", criteria.ElementAt(1).LabelText);
            Assert.Equal("Integer with label", criteria.ElementAt(2).LabelText);
            Assert.Equal("IntegerNullable", criteria.ElementAt(3).LabelText);
            Assert.Equal("MyEnum", criteria.ElementAt(4).LabelText);
            Assert.Equal("Text", criteria.ElementAt(5).LabelText);
            Assert.Equal("CollectionString", criteria.ElementAt(6).LabelText);
        }

        [Fact]
        public void AddCustomSearchCriterion_PassNotSupportedProperty_CorrectCriterionReturned()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            criteria.AddCustomSearchCriterion<SomeClass>(o => o.CollectionComplex);

            Assert.Equal(0, criteria.Count);
        }

        [Fact]
        public void AddCustomSearchCriterion_PassNotSupportedLambdaExpression_CorrectCriterionReturned()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            Assert.Throws<ArgumentException>(() => criteria.AddCustomSearchCriterion<SomeClass>(o => o));
        }

        [Fact]
        public void AddCustomSearchCriterion_PassNotSupportedLambdaExpression_ArgumentExceptionThrown()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            Assert.Throws<ArgumentException>(
                () => criteria.AddCustomSearchCriterion<SomeClass>(o => o.CollectionComplex.Select(i => i.Nested).Select(i => i.TextNested)));
        }

        [Fact]
        public void AddCustomSearchCriterion_PassUnaryLambdaExpression_CorrectCriterionAdded()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            criteria.AddCustomSearchCriterion<SomeClass>(o => o.Integer);

            Assert.Equal(1, criteria.Count);
            Assert.Equal("Integer", criteria.ElementAt(0).Property);
        }

        [Fact]
        public void AddCustomSearchCriterion_PassPropertyLambdaExpression_CorrectCriterionAdded()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            criteria.AddCustomSearchCriterion<SomeClass>(o => o.Text);

            Assert.Equal(1, criteria.Count);
            Assert.Equal("Text", criteria.ElementAt(0).Property);
        }

        [Fact]
        public void AddCustomSearchCriterion_PassNestedPropertyLambdaExpression_CorrectCriterionAdded()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            criteria.AddCustomSearchCriterion<SomeClass>(o => o.Nested.TextNested);

            Assert.Equal(1, criteria.Count);
            Assert.Equal("Nested.TextNested", criteria.ElementAt(0).Property);
            Assert.Equal("TextNested with label", criteria.ElementAt(0).LabelText);
        }

        [Fact]
        public void AddCustomSearchCriterion_PassNestedPropertyInSimpleCollectionLambdaExpression_CorrectCriterionAdded()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            criteria.AddCustomSearchCriterion<SomeClass>(o => o.CollectionString);

            Assert.Equal(1, criteria.Count);
            Assert.Equal("CollectionString", criteria.ElementAt(0).Property);
        }

        [Fact]
        public void AddCustomSearchCriterion_PassNestedPropertyInComplexCollectionLambdaExpression_CorrectCriterionAdded()
        {
            ICollection<AbstractSearch> criteria = new Collection<AbstractSearch>();

            criteria.AddCustomSearchCriterion<SomeClass>(o => o.CollectionComplex.Select(i => i.TextNested));

            Assert.Equal(1, criteria.Count);
            Assert.Equal("CollectionComplex.TextNested", criteria.ElementAt(0).Property);
            Assert.Equal("TextNested with label", criteria.ElementAt(0).LabelText);
        }
    }
}
