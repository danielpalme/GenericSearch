using GenericSearch.Core;
using GenericSearch.Data;
using GenericSearch.Paging;

namespace GenericSearch.UI.Models;

public class PagedSearchViewModel
{
    public PagedSearchViewModel(PagedResult<SomeClass> data, IEnumerable<AbstractSearch> searchCriteria)
    {
        this.Data = data;
        this.SearchCriteria = searchCriteria;
    }

    public PagedResult<SomeClass> Data { get; set; }

    public IEnumerable<AbstractSearch> SearchCriteria { get; set; }
}
