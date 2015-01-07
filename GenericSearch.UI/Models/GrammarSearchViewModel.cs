using System.Collections.Generic;
using GenericSearch.Data;

namespace GenericSearch.UI.Models
{
    public class GrammarSearchViewModel
    {
        public IEnumerable<SomeClass> Data { get; set; }

        public string SearchTerm { get; set; }

        public Grammar Grammar { get; set; }

        public IEnumerable<string> Terms { get; set; }
    }

    public enum Grammar
    {
        Antlr,

        Irony
    }
}