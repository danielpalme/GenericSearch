using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace GenericSearch.Core
{
    public class DateSearch : AbstractSearch
    {
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? SearchTerm { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? SearchTerm2 { get; set; }

        public DateComparators Comparator { get; set; }

        protected override Expression BuildFilterExpression(Expression property)
        {
            Expression searchExpression1 = null;
            Expression searchExpression2 = null;

            if (this.SearchTerm.HasValue)
            {
                searchExpression1 = this.GetFilterExpression(property);
            }

            if (this.Comparator == DateComparators.InRange && this.SearchTerm2.HasValue)
            {
                searchExpression2 = Expression.LessThanOrEqual(property, Expression.Constant(this.SearchTerm2.Value));
            }

            if (searchExpression1 == null && searchExpression2 == null)
            {
                return null;
            }
            else if (searchExpression1 != null && searchExpression2 != null)
            {
                var combinedExpression = Expression.AndAlso(searchExpression1, searchExpression2);
                return combinedExpression;
            }
            else if (searchExpression1 != null)
            {
                return searchExpression1;
            }
            else
            {
                return searchExpression2;
            }
        }

        private Expression GetFilterExpression(Expression property)
        {
            switch (this.Comparator)
            {
                case DateComparators.Less:
                    return Expression.LessThan(property, Expression.Constant(this.SearchTerm.Value));
                case DateComparators.LessOrEqual:
                    return Expression.LessThanOrEqual(property, Expression.Constant(this.SearchTerm.Value));
                case DateComparators.Equal:
                    return Expression.Equal(property, Expression.Constant(this.SearchTerm.Value));
                case DateComparators.GreaterOrEqual:
                case DateComparators.InRange:
                    return Expression.GreaterThanOrEqual(property, Expression.Constant(this.SearchTerm.Value));
                case DateComparators.Greater:
                    return Expression.GreaterThan(property, Expression.Constant(this.SearchTerm.Value));
                default:
                    throw new InvalidOperationException("Comparator not supported.");
            }
        }
    }

    public enum DateComparators
    {
        [Display(Name = "<")]
        Less,

        [Display(Name = "<=")]
        LessOrEqual,

        [Display(Name = "==")]
        Equal,

        [Display(Name = ">=")]
        GreaterOrEqual,

        [Display(Name = ">")]
        Greater,

        [Display(Name = "Range")]
        InRange
    }
}
