using System.Collections.Generic;
using GenericSearch.Core;
using GenericSearch.Data;

namespace GenericSearch.UI.Models
{
    public class SearchViewModel
    {
        public IEnumerable<SomeClass> Data { get; set; }

        public IEnumerable<AbstractSearch> SearchCriteria { get; set; }
    }
}