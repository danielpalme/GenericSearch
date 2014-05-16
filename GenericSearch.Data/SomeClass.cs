using System;
using System.ComponentModel.DataAnnotations;

namespace GenericSearch.Data
{
    public class SomeClass
    {
        public DateTime Date { get; set; }

        public DateTime? DateNullable { get; set; }

        [Display(Name = "Integer with label")]
        public int Integer { get; set; }

        public int? IntegerNullable { get; set; }

        public MyEnum MyEnum { get; set; }

        public string Text { get; set; }

        public SomeNestedClass Nested { get; set; }
    }
}
