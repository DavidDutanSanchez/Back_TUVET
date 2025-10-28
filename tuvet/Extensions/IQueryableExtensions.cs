using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using LinqKit;
using tu_vet_back.tuvet.Dtos;

namespace tu_vet_back.tuvet.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, object>> orderByProperty, bool desc) => desc ? source.OrderByDescending(orderByProperty) : source.OrderBy(orderByProperty);
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                          bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            Type type = typeof(TEntity);
            PropertyInfo property = type.GetProperty(orderByProperty)!;
            ParameterExpression parameter = Expression.Parameter(type, "p");
            MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
            LambdaExpression orderByExpression = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName, bool isDescending)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression property = Expression.Property(parameter, propertyName);
            LambdaExpression selector = Expression.Lambda(property, parameter);

            string methodName = isDescending ? "ThenByDescending" : "ThenBy";
            MethodInfo method = typeof(Queryable).GetMethods().Single(
                m => m.Name == methodName && m.IsGenericMethodDefinition && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type);

            return (IOrderedQueryable<T>)method.Invoke(null, new object[] { source, selector })!;
        }

        public static IOrderedQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string propertyName, bool ascending)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName));
            }

            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            PropertyInfo property = GetProperty(typeof(T), propertyName);
            MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
            LambdaExpression lambda = Expression.Lambda(propertyAccess, parameter);
            string methodName = ascending ? "OrderBy" : "OrderByDescending";
            MethodInfo method = typeof(Queryable).GetMethods()
                .Single(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.PropertyType);

            return (IOrderedQueryable<T>)method.Invoke(null, new object[] { source, lambda });
        }


        public static IQueryable<T> ThenByDynamic<T>(this IOrderedQueryable<T> source, string propertyName, bool ascending)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName));
            }

            PropertyInfo property = GetProperty(typeof(T), propertyName);
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
            LambdaExpression lambda = Expression.Lambda(propertyAccess, parameter);
            string methodName = ascending ? "ThenBy" : "ThenByDescending";
            MethodInfo method = typeof(Queryable).GetMethods()
                .Single(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.PropertyType);

            return (IQueryable<T>)method.Invoke(null, new object[] { source, lambda })!;
        }

        private static PropertyInfo GetProperty(Type type, string propertyName)
        {
            PropertyInfo? property = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{type}'.");
            }

            return property;
        }
        public static IQueryable<T> ApplySearch<T>(
            this IQueryable<T> query,
            string? search,
            params Expression<Func<T, object>>[] searchColumns)
        {
            if (string.IsNullOrEmpty(search) || searchColumns == null || searchColumns.Length == 0)
            {
                return query;
            }

            // Build the predicate dynamically
            ExpressionStarter<T> condition = PredicateBuilder.New<T>(false);

            foreach (Expression<Func<T, object>> column in searchColumns)
            {
                // Dynamically build the condition for each column
                Expression<Func<T, bool>> searchCondition = BuildSearchCondition(column, search);
                if (searchCondition != null)
                {
                    condition = condition.Or(searchCondition);
                }
            }

            // Apply the condition to the query
            return query.Where(condition);
        }

        private static Expression<Func<T, bool>> BuildSearchCondition<T>(
            Expression<Func<T, object>> column,
            string search)
        {
            // Extract the property type and member access
            Expression body = column.Body is UnaryExpression unary ? unary.Operand : column.Body;

            if (body is MemberExpression member)
            {
                Type propertyType = member.Type;

                // Handle string fields
                if (propertyType == typeof(string))
                {
                    return Expression.Lambda<Func<T, bool>>(
                        Expression.AndAlso(
                            Expression.NotEqual(member, Expression.Constant(null)), // Check for null
                            Expression.Call(
                                Expression.Call(member, nameof(string.ToLower), Type.EmptyTypes), // .ToLower()
                                nameof(string.Contains), // .Contains()
                                Type.EmptyTypes,
                                Expression.Constant(search.ToLower()) // search.ToLower()
                            )
                        ),
                        column.Parameters
                    );
                }

                // Handle GUID fields
                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                {
                    if (Guid.TryParse(search, out Guid guidValue))
                    {
                        return Expression.Lambda<Func<T, bool>>(
                            Expression.Equal(member, Expression.Constant(guidValue)),
                            column.Parameters
                        );
                    }
                    return null; // Invalid GUID search, skip this field
                }

                // Handle numeric fields (int, decimal, etc.)
                if (propertyType.IsNumericType())
                {
                    if (decimal.TryParse(search, out decimal numericValue))
                    {
                        return Expression.Lambda<Func<T, bool>>(
                            Expression.Equal(
                                Expression.Convert(member, typeof(decimal)),
                                Expression.Constant(numericValue)
                            ),
                            column.Parameters
                        );
                    }
                    return null; // Invalid numeric search, skip this field
                }
            }

            return null; // Unsupported type, skip this field
        }

        public static IQueryable<T> ApplyDynamicFilters<T>(this IQueryable<T> query, FilterGroup? filters = null)
        {
            if (filters == null || filters.filters == null || filters.filters.Count == 0)
            {
                return query;
            }

            Expression<Func<T, bool>>? lambda = getDynamicFilterLambda<T>(filters);

            return query.Where(lambda);
        }

        public static Expression<Func<T, bool>> getDynamicFilterLambda<T>(FilterGroup filters)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            Expression expression = BuildExpression<T>(filters, parameter);

            return Expression.Lambda<Func<T, bool>>(expression, parameter);
        }

        // Recursive method to build expressions
        private static Expression BuildExpression<T>(FilterGroup filterGroup, ParameterExpression parameter)
        {
            // Base case: process individual filters
            if (filterGroup?.@operator != null && filterGroup.filters != null)
            {
                List<Expression> expressions = filterGroup.filters.Select(filter =>
                {
                    if (filter.@operator == null)
                    {
                        return BuildCondition<T>(filter, parameter);
                    }
                    else
                    {
                        // Nested filter group (AND/OR)
                        return BuildExpression<T>(filter, parameter);
                    }
                }).ToList();

                // Combine expressions based on the operator
                if (filterGroup.@operator == "and")
                {
                    return expressions.Aggregate(Expression.AndAlso);
                }
                if (filterGroup.@operator == "or")
                {
                    return expressions.Aggregate(Expression.OrElse);
                }
                throw new InvalidOperationException($"Unsupported operator: {filterGroup.@operator}");
            }

            throw new InvalidOperationException("Invalid filter format.");
        }

        private static Expression BuildCondition<T>(FilterGroup criteria, ParameterExpression parameter)
        {
            if (criteria.column == null || criteria.condition == null || criteria.value == null)
            {
                throw new ArgumentNullException("One or more required properties of 'criteria' are null.");
            }

            string[] propertyPath = criteria.column.Split('.'); // Soporta propiedades anidadas
            Expression propertyExpression = parameter;

            foreach (string propertyName in propertyPath)
            {
                PropertyInfo property = GetProperty(propertyExpression.Type, propertyName);

                // Si la propiedad es una colección, usa Any para navegar dentro de ella
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
                {
                    Type elementType = property.PropertyType.IsGenericType
                        ? property.PropertyType.GetGenericArguments()[0]
                        : property.PropertyType.GetElementType()!;

                    ParameterExpression elementParameter = Expression.Parameter(elementType, "e");
                    Expression innerCondition = BuildCondition<T>(new FilterGroup
                    {
                        column = string.Join('.', propertyPath.Skip(1)), // Resto del path
                        condition = criteria.condition,
                        value = criteria.value
                    }, elementParameter);

                    MethodInfo anyMethod = typeof(Enumerable).GetMethods()
                        .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
                        .MakeGenericMethod(elementType);

                    return Expression.Call(anyMethod, Expression.Property(propertyExpression, property), Expression.Lambda(innerCondition, elementParameter));
                }

                propertyExpression = Expression.Property(propertyExpression, property);
            }

            string conditionType = criteria.condition;
            dynamic value = criteria.value;

            dynamic constantExpression = Expression.Constant(value is IEnumerable<object> ? value : ConvertValueToTargetType(value, propertyExpression.Type));

            return conditionType switch
            {
                "contains" => (Expression)Expression.Call(propertyExpression, typeof(string).GetMethod("Contains", new[] { typeof(string) }), constantExpression),
                "notContains" => (Expression)Expression.Not(Expression.Call(propertyExpression, typeof(string).GetMethod("Contains", new[] { typeof(string) }), constantExpression)),
                "startsWith" => (Expression)Expression.Call(propertyExpression, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), constantExpression),
                "endsWith" => (Expression)Expression.Call(propertyExpression, typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), constantExpression),
                "is" => (Expression)Expression.Equal(propertyExpression, constantExpression),
                "isNot" => (Expression)Expression.NotEqual(propertyExpression, constantExpression),
                "includes" => (Expression)BuildIncludeExpression(propertyExpression, value, propertyExpression.Type),
                "notIncludes" => (Expression)Expression.Not(BuildIncludeExpression(propertyExpression, value, propertyExpression.Type)),
                "greaterThan" => (Expression)Expression.GreaterThan(propertyExpression, constantExpression),
                "lessThan" => (Expression)Expression.LessThan(propertyExpression, constantExpression),
                "between" => (Expression)BuildBetweenExpression(propertyExpression, value, propertyExpression.Type),
                _ => throw new NotImplementedException($"Condition '{conditionType}' is not supported."),
            };
        }

        // Helper to build 'include' expressions
        private static Expression BuildIncludeExpression(Expression property, object value, Type targetType)
        {
            if (value is IEnumerable<object> list)
            {
                // Try to convert the provided list to the target type
                List<object> convertedList = list.Select(item => ConvertValueToTargetType(item, targetType)).ToList();
                // Ensure that we cast the list to the correct property type
                dynamic castedList = Expression.Constant(convertedList.Cast(targetType));
                MethodInfo containsMethod = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(targetType);

                // Use Contains to check if the property value is in the list
                return Expression.Call(containsMethod, castedList, property);
            }

            throw new InvalidOperationException($"'include' condition requires a list or array of values.");
        }

        // Helper to build 'between' expressions
        private static Expression BuildBetweenExpression(Expression property, object value, Type targetType)
        {
            if (value is IEnumerable<object> list && list.Count() == 2)
            {
                List<object> values = list.Select(v => ConvertValueToTargetType(v, targetType)).ToList();
                // Convert values to the target property type before creating the expressions
                BinaryExpression lowerBound = Expression.GreaterThanOrEqual(property, Expression.Constant(values[0]));
                BinaryExpression upperBound = Expression.LessThanOrEqual(property, Expression.Constant(values[1]));
                return Expression.AndAlso(lowerBound, upperBound);
            }

            throw new InvalidOperationException($"'between' condition requires a collection with exactly two elements.");
        }

        // Utility method to convert value to the target type
        private static object ConvertValueToTargetType(object value, Type targetType)
        {
            if (value.GetType() == targetType)
            {
                return value;
            }

            // Handle nullable types
            Type nonNullableType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            try
            {
                if (nonNullableType == typeof(DateTime))
                {
                    return Convert.ToDateTime(value);
                }

                if (nonNullableType == typeof(double))
                {
                    return Convert.ToDouble(value);
                }

                if (nonNullableType == typeof(decimal))
                {
                    return Convert.ToDecimal(value);
                }

                if (nonNullableType == typeof(int))
                {
                    return Convert.ToInt32(value);
                }

                if (nonNullableType == typeof(bool))
                {
                    return Convert.ToBoolean(value);
                }

                if (nonNullableType.IsEnum)
                {
                    return Enum.Parse(nonNullableType, value.ToString() ?? "");
                }
                // Handle default case for strings, etc.
                return Convert.ChangeType(value, nonNullableType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to convert value '{value}' to type '{targetType}'", ex);
            }
        }
    }
}
