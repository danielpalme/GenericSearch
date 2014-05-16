using System.Collections.Generic;
using GenericSearch.Core;

namespace GenericSearch.UI.Models
{
    public class HomeModel
    {
        public IEnumerable<GenericSearch.Data.SomeClass> Data { get; set; }

        public IEnumerable<AbstractSearch> SearchCriteria { get; set; }

        public IEnumerable<FlatSomeClass> DataNested { get; set; }

        public IEnumerable<AbstractSearch> SearchCriteriaNested { get; set; }

        public int SelectedTab { get; set; }
    }
}