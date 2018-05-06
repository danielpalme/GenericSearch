using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GenericSearch.Core
{
    public abstract class AbstractSearch
    {
        public string Property { get; set; }

        public string TargetTypeName { get; set; }

        public string LabelText
        {
            get
            {
                if (this.Property == null)
                {
                    return null;
                }

                var arg = Expression.Parameter(Type.GetType(this.TargetTypeName), "p");
                var propertyInfo = this.GetPropertyAccess(arg).Member as PropertyInfo;

                if (propertyInfo != null)
                {
                    var displayAttribute = propertyInfo
                        .GetCustomAttribute<DisplayAttribute>();

                    if (displayAttribute != null)
                    {
                        return displayAttribute.GetName();
                    }
                }

                return this.Property.Split('.').Last();
            }
        }

        internal IQueryable<T> ApplyToQuery<T>(IQueryable<T> query)
        {
            string[] parts = this.Property.Split('.');

            var parameter = Expression.Parameter(typeof(T), "p");

            Expression filterExpression = this.BuildFilterExpressionWithNullChecks(null, parameter, null, parts);

            if (filterExpression == null)
            {
                return query;
            }
            else
            {
                var predicate = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                return query.Where(predicate);
            }
        }

        protected abstract Expression BuildFilterExpression(Expression property);

        private static Expression Combine(Expression first, Expression second)
        {
            if (first == null)
            {
                return second;
            }
            else
            {
                return Expression.AndAlso(first, second);
            }
        }

        private Expression BuildFilterExpressionWithNullChecks(
            Expression filterExpression,
            ParameterExpression parameter,
            Expression property,
            string[] remainingPropertyParts)
        {
            property = Expression.Property(property == null ? parameter : property, remainingPropertyParts[0]);

            if (remainingPropertyParts.Length == 1)
            {
                if (!property.Type.IsValueType || property.Type.IsNullableType())
                {
                    var nullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));
                    filterExpression = Combine(filterExpression, nullCheckExpression);
                }

                if (property.Type.IsNullableType())
                {
                    property = Expression.Property(property, "Value");
                }

                Expression searchExpression = null;
                if (property.Type.IsCollectionType())
                {
                    parameter = Expression.Parameter(property.Type.GetGenericArguments().First());

                    searchExpression = ApplySearchExpressionToCollection(
                        parameter,
                        property,
                        this.BuildFilterExpression(parameter));
                }
                else
                {
                    searchExpression = this.BuildFilterExpression(property);
                }

                if (searchExpression == null)
                {
                    return null;
                }
                else
                {
                    return Combine(filterExpression, searchExpression);
                }
            }
            else
            {
                var nullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));
                filterExpression = Combine(filterExpression, nullCheckExpression);

                if (property.Type.IsCollectionType())
                {
                    parameter = Expression.Parameter(property.Type.GetGenericArguments().First());
                    Expression searchExpression = this.BuildFilterExpressionWithNullChecks(
                        null,
                        parameter,
                        null,
                        remainingPropertyParts.Skip(1).ToArray());

                    if (searchExpression == null)
                    {
                        return null;
                    }
                    else
                    {
                        searchExpression = ApplySearchExpressionToCollection(
                            parameter,
                            property,
                            searchExpression);

                        return Combine(filterExpression, searchExpression);
                    }
                }
                else
                {
                    return this.BuildFilterExpressionWithNullChecks(filterExpression, parameter, property, remainingPropertyParts.Skip(1).ToArray());
                }
            }
        }

        private static Expression ApplySearchExpressionToCollection(ParameterExpression parameter, Expression property, Expression searchExpression)
        {
            if (searchExpression != null)
            {
                var asQueryable = typeof(Queryable).GetMethods()
                    .Where(m => m.Name == "AsQueryable")
                    .Single(m => m.IsGenericMethod)
                    .MakeGenericMethod(property.Type.GetGenericArguments());

                var anyMethod = typeof(Queryable).GetMethods()
                    .Where(m => m.Name == "Any")
                    .Single(m => m.GetParameters().Length == 2)
                    .MakeGenericMethod(property.Type.GetGenericArguments());

                searchExpression = Expression.Call(
                    null,
                    anyMethod,
                    Expression.Call(null, asQueryable, property),
                    Expression.Lambda(searchExpression, parameter));
            }

            return searchExpression;
        }

        private MemberExpression GetPropertyAccess(ParameterExpression arg)
        {
            string[] parts = this.Property.Split('.');

            MemberExpression property = Expression.Property(arg, parts[0]);

            for (int i = 1; i < parts.Length; i++)
            {
                if (property.Type.IsCollectionType())
                {
                    property = Expression.Property(
                        Expression.Parameter(property.Type.GetGenericArguments().First()),
                        parts[i]);
                }
                else
                {
                    property = Expression.Property(property, parts[i]);
                }
            }

            return property;
        }
    }
}