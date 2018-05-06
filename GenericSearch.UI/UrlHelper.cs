using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;

namespace GenericSearch.UI
{
    public static class UrlHelper
    {
        public static string SetParameters(this string url, params KeyValuePair<string, string>[] values)
        {
            var query = QueryHelpers.ParseQuery(url);

            var items = new Dictionary<string, string>(query.SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)));

            foreach (var value in values.Where(v => !string.IsNullOrEmpty(v.Value)))
            {
                if (string.IsNullOrEmpty(value.Value))
                {
                    items.Remove(value.Key);
                }
                else
                {
                    items[value.Key] = value.Value;
                }
            }

            return new QueryBuilder(items).ToQueryString().Value;
        }
    }
}
