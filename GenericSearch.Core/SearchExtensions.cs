using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GenericSearch.Core
{
    public static class SearchExtensions
    {
        public static IQueryable<T> ApplySearchCriterias<T>(this IQueryable<T> query, IEnumerable<AbstractSearch> searchCriterias)
        {
            foreach (var criteria in searchCriterias)
            {
                query = criteria.ApplyToQuery(query);
            }

            var result = query.ToArray();

            return query;
        }

        public static ICollection<AbstractSearch> GetDefaultSearchCriterias(this Type type)
        {
            var properties = type.GetProperties()
                .Where(p => p.CanRead && p.CanWrite)
                .OrderBy(p => p.Name);

            var searchCriterias = properties
                .Select(p => CreateSearchCriteria(type, p.PropertyType, p.Name))
                .Where(s => s != null)
                .ToList();

            return searchCriterias;
        }

        public static ICollection<AbstractSearch> AddCustomSearchCriteria<T>(this ICollection<AbstractSearch> searchCriterias, Expression<Func<T, object>> property)
        {
            Type propertyType = null;
            string fullPropertyPath = GetPropertyPath(property, out propertyType);

            AbstractSearch searchCriteria = CreateSearchCriteria(typeof(T), propertyType, fullPropertyPath);

            if (searchCriteria != null)
            {
                searchCriterias.Add(searchCriteria);
            }

            return searchCriterias;
        }

        private static AbstractSearch CreateSearchCriteria(Type targetType, Type propertyType, string property)
        {
            AbstractSearch result = null;

            if (propertyType.Equals(typeof(string)))
            {
                result = new TextSearch();
            }
            else if (propertyType.Equals(typeof(int)) || propertyType.Equals(typeof(int?)))
            {
                result = new NumericSearch();
            }
            else if (propertyType.Equals(typeof(DateTime)) || propertyType.Equals(typeof(DateTime?)))
            {
                result = new DateSearch();
            }
            else if (propertyType.IsEnum)
            {
                result = new EnumSearch(propertyType);
            }

            if (result != null)
            {
                result.Property = property;
                result.TargetTypeName = targetType.AssemblyQualifiedName;
            }

            return result;
        }

        private static string GetPropertyPath<T>(Expression<Func<T, object>> expression, out Type targetType)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException("Please provide a lambda expression like 'n => n.PropertyName'", "expression");
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            targetType = propertyInfo.PropertyType;

            string property = memberExpression.ToString();

            return property.Substring(property.IndexOf('.') + 1);
        }
    }
}
