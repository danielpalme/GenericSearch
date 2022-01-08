using GenericSearch.Data;

namespace GenericSearch.UI.Models;

public enum Grammar
{
    Antlr,

    Irony
}

public class GrammarSearchViewModel
{
    public IEnumerable<SomeClass> Data { get; set; } = new List<SomeClass>();

    public string? SearchTerm { get; set; }

    public Grammar Grammar { get; set; }

    public IEnumerable<string> Terms { get; set; } = new List<string>();
}
