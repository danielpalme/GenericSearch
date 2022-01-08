using GenericSearch.Core;
using GenericSearch.Data;

namespace GenericSearch.UI.Models;

public class SearchViewModel
{
    public IEnumerable<SomeClass> Data { get; set; } = new List<SomeClass>();

    public IEnumerable<AbstractSearch> SearchCriteria { get; set; } = new List<AbstractSearch>();
}
