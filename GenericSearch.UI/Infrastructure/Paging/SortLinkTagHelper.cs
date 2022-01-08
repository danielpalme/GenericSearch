using GenericSearch.Paging;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GenericSearch.UI.Infrastructure.Paging;

[HtmlTargetElement("a", Attributes = "paging,property-name")]
public class SortLinkTagHelper : TagHelper
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public SortLinkTagHelper(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public dynamic Paging { get; set; } = null!;

    public string PropertyName { get; set; } = null!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (this.Paging == null)
        {
            throw new InvalidOperationException("'Paging' must be not null.");
        }

        if (this.PropertyName == null)
        {
            throw new InvalidOperationException("'PropertyName' must be not null.");
        }

        string? url = this.httpContextAccessor.HttpContext?.Request.QueryString.Value;

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
            && this.PropertyName.Equals(this.Paging!.SortColumn)
            && this.Paging!.SortDirection == SortDirection.Ascending)
        {
            sortDirection = SortDirection.Descending;
        }

        return sortDirection;
    }
}