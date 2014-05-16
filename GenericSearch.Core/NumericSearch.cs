using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace GenericSearch.Core
{
    public class NumericSearch : AbstractSearch
    {
        public int? SearchTerm { get; set; }

        public NumericComparators Comparator { get; set; }

        protected override Expression BuildExpression(MemberExpression property)
        {
            if (!this.SearchTerm.HasValue)
            {
                return null;
            }

            Expression searchExpression = this.GetFilterExpression(property);

            return searchExpression;
        }

        private Expression GetFilterExpression(MemberExpression property)
        {
            switch (this.Comparator)
            {
                case NumericComparators.Less:
                    return Expression.LessThan(property, Expression.Constant(this.SearchTerm.Value));
                case NumericComparators.LessOrEqual:
                    return Expression.LessThanOrEqual(property, Expression.Constant(this.SearchTerm.Value));
                case NumericComparators.Equal:
                    return Expression.Equal(property, Expression.Constant(this.SearchTerm.Value));
                case NumericComparators.GreaterOrEqual:
                    return Expression.GreaterThanOrEqual(property, Expression.Constant(this.SearchTerm.Value));
                case NumericComparators.Greater:
                    return Expression.GreaterThan(property, Expression.Constant(this.SearchTerm.Value));
                default:
                    throw new InvalidOperationException("Comparator not supported.");
            }
        }
    }

    public enum NumericComparators
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
        Greater
    }
}
