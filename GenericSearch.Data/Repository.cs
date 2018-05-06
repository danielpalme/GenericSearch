using System.Collections.Generic;
using System.Linq;

namespace GenericSearch.Data
{
    public class Repository
    {
        private static readonly IEnumerable<SomeClass> Instances = CreateDummyData();

        public IQueryable<SomeClass> GetQuery()
        {
            var query = Instances.ToArray().AsQueryable();

            return query;
        }

        private static IEnumerable<SomeClass> CreateDummyData()
        {
            var dummyData = new List<SomeClass>()
            {
                new SomeClass()
                {
                    Text = "abcdef",
                    Integer = 10,
                    IntegerNullable = 10,
                    Date = new System.DateTime(2000, 1, 1),
                    DateNullable = new System.DateTime(2000, 1, 1),
                    MyEnum = Data.MyEnum.First,
                    Nested = new SomeNestedClass()
                    {
                        TextNested = "qwerty",
                        Nested = new SomeNestedClass()
                        {
                            TextNested = "_qwerty"
                        }
                    }
                },
                new SomeClass()
                {
                    Text = "bcdefg",
                    Integer = 20,
                    IntegerNullable = 20,
                    Date = new System.DateTime(2001, 2, 1),
                    DateNullable = new System.DateTime(2001, 2, 1),
                    MyEnum = Data.MyEnum.Second,
                    Nested = new SomeNestedClass()
                    {
                        TextNested = "wertyu",
                        Nested = new SomeNestedClass()
                        {
                            TextNested = "_wertyu"
                        }
                    }
                },
                new SomeClass()
                {
                    Text = "cdefgh",
                    Integer = 30,
                    Date = new System.DateTime(2002, 3, 1),
                    DateNullable = new System.DateTime(2002, 3, 1),
                    MyEnum = Data.MyEnum.Third,
                    Nested = new SomeNestedClass()
                    {
                        TextNested = "ertyui"
                    }
                },
                new SomeClass()
                {
                    Text = "defghi",
                    Integer = 40,
                    Date = new System.DateTime(2003, 4, 1),
                    DateNullable = new System.DateTime(2003, 4, 1),
                    MyEnum = Data.MyEnum.First,
                    Nested = new SomeNestedClass()
                    {
                        TextNested = "rtyuio"
                    }
                },
                new SomeClass()
                {
                    Text = "efghij",
                    Integer = 50,
                    Date = new System.DateTime(2004, 5, 1),
                    MyEnum = Data.MyEnum.Second,
                },
                new SomeClass()
                {
                    Text = "fghijk",
                    Integer = 60,
                    Date = new System.DateTime(2005, 6, 1),
                    MyEnum = Data.MyEnum.Third,
                },
                new SomeClass()
                {
                    Text = "ghijkl",
                    Integer = 70,
                    Date = new System.DateTime(2006, 7, 1),
                    MyEnum = Data.MyEnum.Second,
                },
                new SomeClass()
                {
                    Text = null,
                    Integer = 80,
                    Date = new System.DateTime(2007, 8, 1),
                    MyEnum = Data.MyEnum.Third,
                },
                new SomeClass()
                {
                    Text = "abcdef2",
                    Integer = 10,
                    IntegerNullable = 10,
                    Date = new System.DateTime(2000, 1, 1),
                    DateNullable = new System.DateTime(2000, 1, 1),
                    MyEnum = Data.MyEnum.First,
                    Nested = new SomeNestedClass()
                    {
                        TextNested = "qwerty",
                        Nested = new SomeNestedClass()
                        {
                            TextNested = "_qwerty"
                        }
                    }
                },
                new SomeClass()
                {
                    Text = "bcdefg2",
                    Integer = 20,
                    IntegerNullable = 20,
                    Date = new System.DateTime(2001, 2, 1),
                    DateNullable = new System.DateTime(2001, 2, 1),
                    MyEnum = Data.MyEnum.Second,
                    Nested = new SomeNestedClass()
                    {
                        TextNested = "wertyu",
                        Nested = new SomeNestedClass()
                        {
                            TextNested = "_wertyu"
                        }
                    }
                },
                new SomeClass()
                {
                    Text = "cdefgh2",
                    Integer = 30,
                    Date = new System.DateTime(2002, 3, 1),
                    DateNullable = new System.DateTime(2002, 3, 1),
                    MyEnum = Data.MyEnum.Third,
                    Nested = new SomeNestedClass()
                    {
                        TextNested = "ertyui"
                    }
                },
                new SomeClass()
                {
                    Text = "defghi2",
                    Integer = 40,
                    Date = new System.DateTime(2003, 4, 1),
                    DateNullable = new System.DateTime(2003, 4, 1),
                    MyEnum = Data.MyEnum.First,
                    Nested = new SomeNestedClass()
                    {
                        TextNested = "rtyuio"
                    }
                },
                new SomeClass()
                {
                    Text = "efghij2",
                    Integer = 50,
                    Date = new System.DateTime(2004, 5, 1),
                    MyEnum = Data.MyEnum.Second,
                },
                new SomeClass()
                {
                    Text = "fghijk2",
                    Integer = 60,
                    Date = new System.DateTime(2005, 6, 1),
                    MyEnum = Data.MyEnum.Third,
                },
                new SomeClass()
                {
                    Text = "ghijkl",
                    Integer = 70,
                    Date = new System.DateTime(2006, 7, 1),
                    MyEnum = Data.MyEnum.Second,
                },
                new SomeClass()
                {
                    Text = null,
                    Integer = 80,
                    Date = new System.DateTime(2007, 8, 1),
                    MyEnum = Data.MyEnum.Third,
                }
            };

            for (int i = 0; i < dummyData.Count; i++)
            {
                if (i < dummyData.Count / 2)
                {
                    dummyData[i].CollectionString.Add("simple_123");
                    dummyData[i].CollectionString.Add("simple_234");

                    dummyData[i].CollectionComplex.Add(new SomeNestedClass()
                    {
                        TextNested = "complex_678"
                    });
                    dummyData[i].CollectionComplex.Add(new SomeNestedClass()
                    {
                        TextNested = "complex_789"
                    });
                }
                else
                {
                    dummyData[i].CollectionString.Add("simple_456");
                    dummyData[i].CollectionString.Add("simple_567");

                    dummyData[i].CollectionComplex.Add(new SomeNestedClass()
                    {
                        TextNested = "complex_876"
                    });
                    dummyData[i].CollectionComplex.Add(new SomeNestedClass()
                    {
                        TextNested = "complex_987"
                    });
                }
            }

            return dummyData;
        }
    }
}
