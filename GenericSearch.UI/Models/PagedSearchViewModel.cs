using System.Collections.Generic;
using GenericSearch.Core;
using GenericSearch.Data;
using GenericSearch.Paging;

namespace GenericSearch.UI.Models
{
    public class PagedSearchViewModel
    {
        public PagedResult<SomeClass> Data { get; set; }

        public IEnumerable<AbstractSearch> SearchCriteria { get; set; }
    }
}