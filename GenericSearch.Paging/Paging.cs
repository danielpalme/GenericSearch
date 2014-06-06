using System.Runtime.Serialization;

namespace GenericSearch.Paging
{
    /// <summary>
    /// Contains information necessary for paging and sorting.
    /// <seealso cref="PagingExtensions"/>.
    /// </summary>
    [DataContract]
    public class Paging
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Paging"/> class.
        /// </summary>
        public Paging()
            : this(0, 5)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging" /> class.
        /// </summary>
        /// <param name="pageIndex">The current page index.</param>
        /// <param name="pageSize">Number of elements per page.</param>
        public Paging(int pageIndex, int pageSize)
        {
            this.SortDirection = SortDirection.Ascending;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging" /> class.
        /// </summary>
        /// <param name="pageIndex">The current page index.</param>
        /// <param name="pageSize">Number of elements per page.</param>
        /// <param name="sortColumn">The name of the property sorting should be applied to.</param>
        public Paging(int pageIndex, int pageSize, string sortColumn)
            : this(pageIndex, pageSize)
        {
            this.SortColumn = sortColumn;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Paging" /> class.
        /// </summary>
        /// <param name="pageIndex">The current page index.</param>
        /// <param name="pageSize">Number of elements per page.</param>
        /// <param name="sortColumn">The name of the property sorting should be applied to.</param>
        /// <param name="sortDirection">The <see cref="SortDirection"/>.</param>
        public Paging(int pageIndex, int pageSize, string sortColumn, SortDirection sortDirection)
            : this(pageIndex, pageSize, sortColumn)
        {
            this.SortDirection = sortDirection;
        }

        /// <summary>
        /// Gets or sets the <see cref="SortDirection"/>.
        /// </summary>
        [DataMember]
        public SortDirection SortDirection { get; set; }

        /// <summary>
        /// Gets or sets the name of the property sorting should be applied to.
        /// </summary>
        [DataMember]
        public string SortColumn { get; set; }

        /// <summary>
        /// Gets or sets the current page index.
        /// </summary>
        [DataMember]
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of elements per page.
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "SortColumn: " + this.SortColumn + ", PageIndex: " + this.PageIndex + ", PageSize: " + this.PageSize;
        }
    }
}
