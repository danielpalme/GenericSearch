using System.Collections.Generic;
using GenericSearch.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GenericSearch.UI
{
    [HtmlTargetElement("a", Attributes = "paging,property-name")]
    public class SortLinkTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public SortLinkTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public dynamic Paging { get; set; }

        public string PropertyName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string url = this.httpContextAccessor.HttpContext.Request.QueryString.Value;

            string extendedUrl = url.SetParameters(
                KeyValuePair.Create("sortColumn", this.PropertyName),
                KeyValuePair.Create("sortDirection", this.GetSortDirection().ToString()),
                KeyValuePair.Create("skip", "0"));

            output.Attributes.Add("href", extendedUrl);
        }

        private SortDirection GetSortDirection()
        {
            SortDirection sortDirection = SortDirection.Ascending;

            if (this.Paging != null
                && this.PropertyName.Equals(this.Paging.SortColumn)
                && this.Paging.SortDirection == SortDirection.Ascending)
            {
                sortDirection = SortDirection.Descending;
            }

            return sortDirection;
        }
    }
}