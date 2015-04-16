using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericSearch.Core.Test
{
    [TestClass]
    public class TypeExtensionsTest
    {
        [TestMethod]
        public void IsNullableType_PassNotNullableType_FalseReturned()
        {
            Assert.IsFalse(typeof(int).IsNullableType());
        }

        [TestMethod]
        public void IsNullableType_PassNullableType_TrueReturned()
        {
            Assert.IsTrue(typeof(int?).IsNullableType());
        }

        [TestMethod]
        public void IsCollectionType_PassIEnumerable_TrueReturned()
        {
            Assert.IsTrue(typeof(IEnumerable<int>).IsCollectionType());
        }

        [TestMethod]
        public void IsCollectionType_PassList_TrueReturned()
        {
            Assert.IsTrue(typeof(List<int>).IsCollectionType());
        }

        [TestMethod]
        public void IsCollectionType_PassCollection_TrueReturned()
        {
            Assert.IsTrue(typeof(Collection<int>).IsCollectionType());
        }
    }
}
