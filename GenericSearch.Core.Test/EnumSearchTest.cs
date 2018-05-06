using System.Linq;
using GenericSearch.Data;
using Xunit;

namespace GenericSearch.Core
{
    public class EnumSearchTest
    {
        [Fact]
        public void ApplyToQuery_EqualsEnum_CorrectResultReturned()
        {
            var criteria = new EnumSearch();
            criteria.Property = "MyEnum";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;
            criteria.EnumTypeName = typeof(MyEnum).AssemblyQualifiedName;

            criteria.SearchTerm = MyEnum.First.ToString();

            Assert.Equal(4, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }
    }
}
