using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace GenericSearch.Data
{
    public class SomeClass
    {
        public SomeClass()
        {
            this.CollectionString = new Collection<string>();
            this.CollectionComplex = new Collection<SomeNestedClass>();
        }

        public DateTime Date { get; set; }

        public DateTime? DateNullable { get; set; }

        [Display(Name = "Integer with label")]
        public int Integer { get; set; }

        public int? IntegerNullable { get; set; }

        public MyEnum MyEnum { get; set; }

        public string Text { get; set; }

        public SomeNestedClass Nested { get; set; }

        public Collection<string> CollectionString { get; set; }

        public Collection<SomeNestedClass> CollectionComplex { get; set; }
    }
}
