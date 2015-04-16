using System.ComponentModel.DataAnnotations;

namespace GenericSearch.Data
{
    public class SomeNestedClass
    {
        [Display(Name = "TextNested with label")]
        public string TextNested { get; set; }

        public SomeNestedClass Nested { get; set; }
    }
}
