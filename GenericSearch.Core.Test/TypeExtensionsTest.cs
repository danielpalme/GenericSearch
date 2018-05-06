using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace GenericSearch.Core.Test
{
    public class TypeExtensionsTest
    {
        [Fact]
        public void IsNullableType_PassNotNullableType_FalseReturned()
        {
            Assert.False(typeof(int).IsNullableType());
        }

        [Fact]
        public void IsNullableType_PassNullableType_TrueReturned()
        {
            Assert.True(typeof(int?).IsNullableType());
        }

        [Fact]
        public void IsCollectionType_PassIEnumerable_TrueReturned()
        {
            Assert.True(typeof(IEnumerable<int>).IsCollectionType());
        }

        [Fact]
        public void IsCollectionType_PassList_TrueReturned()
        {
            Assert.True(typeof(List<int>).IsCollectionType());
        }

        [Fact]
        public void IsCollectionType_PassCollection_TrueReturned()
        {
            Assert.True(typeof(Collection<int>).IsCollectionType());
        }
    }
}
