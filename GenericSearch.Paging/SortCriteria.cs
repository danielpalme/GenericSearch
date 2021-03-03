using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace GenericSearch.Paging
{
    [DataContract]
    public class SortCriteria<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortCriteria{T}"/> class.
        /// </summary>
        public SortCriteria()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortCriteria{T}"/> class.
        /// </summary>
        /// <param name="sortColumn">The name of the property sorting should be applied to.</param>
        public SortCriteria(string sortColumn)
        {
            this.SortColumn = sortColumn;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortCriteria{T}"/> class.
        /// </summary>
        /// <param name="sortColumn">The name of the property sorting should be applied to.</param>
        /// <param name="sortDirection">The <see cref="SortDirection"/>.</param>
        public SortCriteria(string sortColumn, SortDirection sortDirection)
        {
            this.SortColumn = sortColumn;
            this.SortDirection = sortDirection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortCriteria{T}"/> class.
        /// </summary>
        /// <param name="sortDirection">The <see cref="SortDirection"/>.</param>
        public SortCriteria(Expression<Func<T, object>> sortExpression)
        {
            this.SortColumn = PropertyResolver.GetPropertyName(sortExpression);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortCriteria{T}"/> class.
        /// </summary>
        /// <param name="sortExpression">A lambda expression like 'n => n.PropertyName'.</param>
        /// <param name="sortDirection">The <see cref="SortDirection"/>.</param>
        public SortCriteria(Expression<Func<T, object>> sortExpression, SortDirection sortDirection)
        {
            this.SortColumn = PropertyResolver.GetPropertyName(sortExpression);
            this.SortDirection = sortDirection;
        }

        /// <summary>
        /// Gets or sets the name of the property sorting should be applied to.
        /// </summary>
        [DataMember]
        public string SortColumn { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SortDirection"/>.
        /// </summary>
        [DataMember]
        public SortDirection SortDirection { get; set; }

        /// <summary>
        /// Sets the sort expression.
        /// </summary>
        /// <param name="sortExpression">A lambda expression like 'n => n.PropertyName'.</param>
        public void SetSortExpression(Expression<Func<T, object>> sortExpression)
        {
            this.SortColumn = PropertyResolver.GetPropertyName(sortExpression);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"SortColumn: {this.SortColumn}";
        }
    }
}