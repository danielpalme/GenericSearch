using System.Linq;
using GenericSearch.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericSearch.Core
{
    [TestClass]
    public class EnumSearchTest
    {
        [TestMethod]
        public void ApplyToQuery_EqualsEnum_CorrectResultReturned()
        {
            var criteria = new EnumSearch();
            criteria.Property = "MyEnum";
            criteria.TargetTypeName = typeof(SomeClass).AssemblyQualifiedName;
            criteria.EnumTypeName = typeof(MyEnum).AssemblyQualifiedName;

            criteria.SearchTerm = MyEnum.First.ToString();

            Assert.AreEqual(4, criteria.ApplyToQuery(new Repository().GetQuery()).Count());
        }
    }
}
