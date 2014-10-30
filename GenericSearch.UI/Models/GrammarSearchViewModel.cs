using System.Collections.Generic;
using GenericSearch.Data;

namespace GenericSearch.UI.Models
{
    public class GrammarSearchViewModel
    {
        public IEnumerable<SomeClass> Data { get; set; }

        public string SearchTerm { get; set; }
    }
}