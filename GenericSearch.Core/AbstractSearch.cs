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
                    var displayAttribute = propertyInfo.GetCustomAttributes(true)
                        .OfType<DisplayAttribute>()
                        .Cast<DisplayAttribute>()
                        .FirstOrDefault();

                    if (displayAttribute != null)
                    {
                        return displayAttribute.Name;
                    }
                }

                string[] parts = this.Property.Split('.');

                return parts[parts.Length - 1];
            }
        }

        internal IQueryable<T> ApplyToQuery<T>(IQueryable<T> query)
        {
            var arg = Expression.Parameter(typeof(T), "p");
            var property = this.GetPropertyAccess(arg);

            Expression searchExpression = null;

            if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                searchExpression = this.BuildExpression(Expression.Property(property, "Value"));
            }
            else
            {
                searchExpression = this.BuildExpression(property);
            }

            if (searchExpression == null)
            {
                return query;
            }
            else
            {
                var predicate = CreatePredicateWithNullCheck<T>(searchExpression, arg, property);
                return query.Where(predicate);
            }
        }

        protected abstract Expression BuildExpression(MemberExpression property);

        private MemberExpression GetPropertyAccess(ParameterExpression arg)
        {
            string[] parts = this.Property.Split('.');

            MemberExpression property = Expression.Property(arg, parts[0]);

            for (int i = 1; i < parts.Length; i++)
            {
                property = Expression.Property(property, parts[i]);
            }

            return property;
        }

        private Expression<Func<T, bool>> CreatePredicateWithNullCheck<T>(Expression searchExpression, ParameterExpression arg, MemberExpression targetProperty)
        {
            string[] parts = this.Property.Split('.');

            Expression nullCheckExpression = null;
            if (parts.Length > 1)
            {
                MemberExpression property = Expression.Property(arg, parts[0]);
                nullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));

                for (int i = 1; i < parts.Length - 1; i++)
                {
                    property = Expression.Property(property, parts[i]);
                    Expression innerNullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));

                    nullCheckExpression = Expression.AndAlso(nullCheckExpression, innerNullCheckExpression);
                }
            }

            if (!targetProperty.Type.IsValueType || (targetProperty.Type.IsGenericType && targetProperty.Type.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                var innerNullCheckExpression = Expression.NotEqual(targetProperty, Expression.Constant(null));

                if (nullCheckExpression == null)
                {
                    nullCheckExpression = innerNullCheckExpression;
                }
                else
                {
                    nullCheckExpression = Expression.AndAlso(nullCheckExpression, innerNullCheckExpression);
                }
            }

            if (nullCheckExpression == null)
            {
                return Expression.Lambda<Func<T, bool>>(searchExpression, arg);
            }
            else
            {
                var combinedExpression = Expression.AndAlso(nullCheckExpression, searchExpression);

                var predicate = Expression.Lambda<Func<T, bool>>(combinedExpression, arg);

                return predicate;
            }
        }
    }
}
