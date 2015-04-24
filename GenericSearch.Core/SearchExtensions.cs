using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GenericSearch.Core
{
    public static class SearchExtensions
    {
        public static IQueryable<T> ApplySearchCriteria<T>(this IQueryable<T> query, IEnumerable<AbstractSearch> searchCriterias)
        {
            foreach (var criteria in searchCriterias)
            {
                query = criteria.ApplyToQuery(query);
            }

            return query;
        }

        public static ICollection<AbstractSearch> GetDefaultSearchCriteria(this Type type)
        {
            var properties = type.GetProperties()
                .Where(p => p.CanRead && p.CanWrite)
                .OrderBy(p => p.PropertyType.IsCollectionType())
                .ThenBy(p => p.Name);

            var searchCriterias = properties
                .Select(p => CreateSearchCriterion(type, p.PropertyType, p.Name))
                .Where(s => s != null)
                .ToList();

            return searchCriterias;
        }

        public static ICollection<AbstractSearch> AddCustomSearchCriterion<T>(this ICollection<AbstractSearch> searchCriterias, Expression<Func<T, object>> property)
        {
            Type propertyType = null;
            string fullPropertyPath = GetPropertyPath(property, out propertyType);

            AbstractSearch searchCriteria = CreateSearchCriterion(typeof(T), propertyType, fullPropertyPath);

            if (searchCriteria != null)
            {
                searchCriterias.Add(searchCriteria);
            }

            return searchCriterias;
        }

        private static AbstractSearch CreateSearchCriterion(Type targetType, Type propertyType, string property)
        {
            AbstractSearch result = null;

            if (propertyType.IsCollectionType())
            {
                propertyType = propertyType.GetGenericArguments().First();
            }

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
            MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;

            if (methodCallExpression != null)
            {
                if (methodCallExpression.Arguments.Count == 2)
                {
                    MemberExpression memberExpression1 = methodCallExpression.Arguments[0] as MemberExpression;
                    LambdaExpression lambdaExpression = methodCallExpression.Arguments[1] as LambdaExpression;

                    if (memberExpression1 != null && lambdaExpression != null)
                    {
                        MemberExpression memberExpression2 = lambdaExpression.Body as MemberExpression;

                        if (memberExpression2 != null)
                        {
                            targetType = memberExpression2.Type;

                            return string.Format(
                                "{0}.{1}",
                                GetPropertyPath(memberExpression1),
                                GetPropertyPath(memberExpression2));
                        }
                    }
                }

                throw new ArgumentException("Please provide a lambda expression like 'n => n.Collection.Select(c => c.PropertyName)'", "expression");
            }
            else
            {
                UnaryExpression unaryExpression = expression.Body as UnaryExpression;
                MemberExpression memberExpression = null;

                if (unaryExpression != null)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;
                }
                else
                {
                    memberExpression = expression.Body as MemberExpression;
                }

                if (memberExpression != null)
                {
                    targetType = memberExpression.Type;

                    return GetPropertyPath(memberExpression);
                }

                throw new ArgumentException("Please provide a lambda expression like 'n => n.PropertyName'", "expression");
            }
        }

        private static string GetPropertyPath(MemberExpression memberExpression)
        {
            string property = memberExpression.ToString();
            return property.Substring(property.IndexOf('.') + 1);
        }
    }
}
