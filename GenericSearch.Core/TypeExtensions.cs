using System;
using System.Collections.Generic;

namespace GenericSearch.Core
{
    internal static class TypeExtensions
    {
        public static bool IsNullableType(this Type propertyType)
        {
            return propertyType.IsGenericType
                && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsCollectionType(this Type propertyType)
        {
            return propertyType.IsGenericType
                && typeof(IEnumerable<>)
                    .MakeGenericType(propertyType.GetGenericArguments())
                    .IsAssignableFrom(propertyType);
        }
    }
}
