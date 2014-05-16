using System;
using GenericSearch.Data;

namespace GenericSearch.UI.Models
{
    public class FlatSomeClass
    {
        public DateTime Date { get; set; }

        public DateTime? DateNullable { get; set; }

        public int Integer { get; set; }

        public int? IntegerNullable { get; set; }

        public MyEnum MyEnum { get; set; }

        public string Text { get; set; }

        public string TextNested { get; set; }
    }
}