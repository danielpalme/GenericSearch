using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace GenericSearch.Paging
{
    /// <summary>
    /// Contains information necessary for paging and sorting.
    /// <seealso cref="PagingExtensions" />.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    [DataContract]
    public class Paging<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Paging{T}"/> class.
        /// </summary>
        public Paging()
            : this(0, int.MaxValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging{T}"/> class.
        /// </summary>
        /// <param name="skip">The number of elements to skip.</param>
        /// <param name="top">The number of elements to retrieve.</param>
        public Paging(int skip, int top)
        {
            this.Skip = skip;
            this.Top = top;
            this.SortCriteria = new SortCriteria<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging{T}"/> class.
        /// </summary>
        /// <param name="skip">The number of elements to skip.</param>
        /// <param name="top">The number of elements to retrieve.</param>
        /// <param name="sortColumn">The name of the property sorting should be applied to.</param>
        public Paging(int skip, int top, string sortColumn)
            : this(skip, top)
        {
            this.SortCriteria = new SortCriteria<T>(sortColumn);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging{T}"/> class.
        /// </summary>
        /// <param name="skip">The number of elements to skip.</param>
        /// <param name="top">The number of elements to retrieve.</param>
        /// <param name="sortColumn">The name of the property sorting should be applied to.</param>
        /// <param name="sortDirection">The <see cref="SortDirection"/>.</param>
        public Paging(int skip, int top, string sortColumn, SortDirection sortDirection)
            : this(skip, top)
        {
            this.SortCriteria = new SortCriteria<T>(sortColumn, sortDirection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging{T}"/> class.
        /// </summary>
        /// <param name="skip">The number of elements to skip.</param>
        /// <param name="top">The number of elements to retrieve.</param>
        /// <param name="sortExpression">A lambda expression like 'n => n.PropertyName'.</param>
        public Paging(int skip, int top, Expression<Func<T, object>> sortExpression)
            : this(skip, top)
        {
            this.SortCriteria = new SortCriteria<T>(sortExpression);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging{T}"/> class.
        /// </summary>
        /// <param name="skip">The number of elements to skip.</param>
        /// <param name="top">The number of elements to retrieve.</param>
        /// <param name="sortExpression">A lambda expression like 'n => n.PropertyName'.</param>
        /// <param name="sortDirection">The <see cref="SortDirection"/>.</param>
        public Paging(int skip, int top, Expression<Func<T, object>> sortExpression, SortDirection sortDirection)
            : this(skip, top)
        {
            this.SortCriteria = new SortCriteria<T>(sortExpression, sortDirection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging{T}"/> class.
        /// </summary>
        /// <param name="skip">The number of elements to skip.</param>
        /// <param name="top">The number of elements to retrieve.</param>
        /// <param name="sortCriteria">The sort criteria.</param>
        public Paging(int skip, int top, SortCriteria<T> sortCriteria)
            : this(skip, top)
        {
            this.SortCriteria = sortCriteria ?? throw new ArgumentNullException(nameof(sortCriteria));
        }

        /// <summary>
        /// Gets or sets the sort criteria.
        /// </summary>
        [DataMember]
        public SortCriteria<T> SortCriteria { get; }

        [DataMember]
        public List<SortCriteria<T>> AddtionalSortCriteria { get; } = new List<SortCriteria<T>>();

        /// <summary>
        /// Gets or sets the number of elements to skip.
        /// </summary>
        [DataMember]
        public int Skip { get; set; }

        /// <summary>
        /// Gets or sets the number of elements to retrieve.
        /// </summary>
        [DataMember]
        public int Top { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"SortColumn: {this.SortCriteria.SortColumn}, Top: {this.Top}, Skip: {this.Skip}";
        }
    }
}
