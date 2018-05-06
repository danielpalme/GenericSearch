using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GenericSearch.UI
{
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public PagerTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public dynamic PagedResult { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var listBuilder = new StringBuilder();

            int totalPages = (int)Math.Ceiling((double)this.PagedResult.TotalNumberOfItems / this.PagedResult.Paging.Top);

            var pagingIndexes = GetPagingIndexes(
                this.PagedResult.Paging.Skip / this.PagedResult.Paging.Top,
                totalPages);

            if (totalPages > 1)
            {
                for (int i = 0; i < pagingIndexes.Length; i++)
                {
                    if (i > 0 && pagingIndexes[i - 1] != pagingIndexes[i] - 1)
                    {
                        listBuilder.AppendLine("<li class=\"page-item disabled\"><a href=\"#\" class=\"page-link\">&hellip;</a></li>");
                    }

                    if ((this.PagedResult.Paging.Skip / this.PagedResult.Paging.Top) == pagingIndexes[i])
                    {
                        listBuilder.AppendLine("<li class=\"page-item active\"><a href=\"#\" class=\"page-link\">" + (pagingIndexes[i] + 1) + "</a></li>");
                    }
                    else
                    {
                        string url = this.httpContextAccessor.HttpContext.Request.QueryString.Value;
                        string skip = (pagingIndexes[i] * this.PagedResult.Paging.Top).ToString();
                        url = url.SetParameters(KeyValuePair.Create("skip", skip));

                        listBuilder.AppendLine("<li class=\"page-item\"><a href=\"" + url + "\" class=\"page-link\">" + (pagingIndexes[i] + 1) + "</a></li>");
                    }
                }
            }

            output.TagName = "ul";
            output.Attributes.Add("class", "pagination");
            output.Content.SetHtmlContent(listBuilder.ToString());

            output.PreElement.SetHtmlContent("<nav>");
            output.PostElement.SetHtmlContent("</nav>");
        }

        private static int[] GetPagingIndexes(int currentIndex, int totalPages)
        {
            var result = new HashSet<int>();

            for (int i = 0; i < 2; i++)
            {
                if (i <= totalPages)
                {
                    result.Add(i);
                }
            }

            int current = currentIndex - 2;

            while (current <= currentIndex + 2)
            {
                if (current > 0 && current < totalPages)
                {
                    result.Add(current);
                }

                current++;
            }

            for (int i = totalPages - 2; i < totalPages; i++)
            {
                result.Add(i);
            }

            return result.ToArray();
        }
    }
}