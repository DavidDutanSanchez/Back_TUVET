using System.Reflection;
using Microsoft.EntityFrameworkCore;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Model.Parameters;


namespace tu_vet_back.tuvet.Extensions
{
    public static class PaginationExtension
    {
        /// <summary>
        /// Paginates a queryable collection based on the provided query parameters.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="query">The collection to paginate.</param>
        /// <param name="qParams">The pagination parameters.</param>
        /// <returns>A <see cref="PaginationDto{T}"/> containing paginated data and metadata.</returns>
        public static PaginationDto<T> GetPaged<T>(this IQueryable<T> query, QueryParams? qParams = null)
        {
            T? totalRow = default;
            if (qParams?.totalize == true)
            {
                // Create an instance of T to hold the total row values
                // This is necessary to avoid null reference exceptions when setting properties
                totalRow = Activator.CreateInstance<T>();
                // Get all numeric properties of T
                PropertyInfo[] numericProperties = typeof(T).GetProperties()
                    .Where(p => p.PropertyType.IsNumericType()).ToArray();

                // Calculate totals for each numeric property
                // Group by a constant (1) to get a single group for the entire collection
                Dictionary<string, decimal>? totalsQuery = query
                        .GroupBy(_ => 1)
                        .Select(g => numericProperties.ToDictionary(
                            prop => prop.Name,
                            prop => g.Sum(x => Convert.ToDecimal(prop.GetValue(x) ?? 0))
                        ))
                        .FirstOrDefault();

                if (totalsQuery != null)
                {
                    foreach (PropertyInfo prop in numericProperties)
                    {
                        // Check if the total for the property exists in the dictionary
                        // If it does, set the value in the totalRow instance
                        if (totalsQuery.TryGetValue(prop.Name, out decimal total))
                        {
                            // Convert the total to the appropriate type and set it in the totalRow instance
                            Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            prop.SetValue(totalRow, Convert.ChangeType(total, targetType));
                        }
                    }
                }
            }

            PaginationDto<T> result = new()
            {
                total = query.Count(),
                // Ensure pageSize is valid
                pageSize = Math.Max(1, qParams?.pageSize ?? 1),
                totalData = totalRow,
            };

            // Calculate total count and total pages
            result.totalPages = (int)Math.Ceiling((double)result.total / result.pageSize);

            // Ensure page doesn't exceed total pages
            int fixedPage = Math.Max(1, qParams?.page ?? 1);
            result.currentPage = Math.Min(fixedPage, result.totalPages == 0 ? 1 : result.totalPages);

            // Fetch the paginated data
            result.data = query
                .Skip((result.currentPage - 1) * result.pageSize)
                .Take(result.pageSize);

            return result;
        }

        public static PaginationAsList<T> GetPagedToList<T>(this IQueryable<T> list, QueryParams? qParams = null)
        {
            PaginationDto<T> result = list.GetPaged(qParams);
            return new PaginationAsList<T>
            {
                currentPage = result.currentPage,
                pageSize = result.pageSize,
                data = result.data.ToList(),
                total = result.total,
                totalPages = result.totalPages,
                totalData = result.totalData,
            };
        }

        public static async Task<PaginationDto<T>> GetPagedAsync<T>(this IQueryable<T> query, QueryParams qParams) where T : class
        {
            T? totalRow = default;
            if (qParams?.totalize == true)
            {
                // Create an instance of T to hold the total row values
                // This is necessary to avoid null reference exceptions when setting properties
                totalRow = Activator.CreateInstance<T>();
                // Get all numeric properties of T
                PropertyInfo[] numericProperties = typeof(T).GetProperties()
                    .Where(p => p.PropertyType.IsNumericType()).ToArray();

                // Calculate totals for each numeric property
                // Group by a constant (1) to get a single group for the entire collection
                Dictionary<string, decimal>? totalsQuery = await query
                        .GroupBy(_ => 1)
                        .Select(g => numericProperties.ToDictionary(
                            prop => prop.Name,
                            prop => g.Sum(x => Convert.ToDecimal(prop.GetValue(x) ?? 0))
                        ))
                        .FirstOrDefaultAsync();

                if (totalsQuery != null)
                {
                    foreach (PropertyInfo prop in numericProperties)
                    {
                        // Check if the total for the property exists in the dictionary
                        // If it does, set the value in the totalRow instance
                        if (totalsQuery.TryGetValue(prop.Name, out decimal total))
                        {
                            // Convert the total to the appropriate type and set it in the totalRow instance
                            Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            prop.SetValue(totalRow, Convert.ChangeType(total, targetType));
                        }
                    }
                }
            }

            PaginationDto<T> result = new()
            {
                total = await query.CountAsync(),
                // Ensure pageSize is valid
                pageSize = Math.Max(1, qParams?.pageSize ?? 1),
                totalData = totalRow,
            };

            // Calculate total count and total pages
            result.totalPages = (int)Math.Ceiling((double)result.total / result.pageSize);

            // Ensure page doesn't exceed total pages
            int fixedPage = Math.Max(1, qParams?.page ?? 1);
            result.currentPage = Math.Min(fixedPage, result.totalPages == 0 ? 1 : result.totalPages);

            // Fetch the paginated data
            result.data = query
                .Skip((result.currentPage - 1) * result.pageSize)
                .Take(result.pageSize);

            return result;
        }

        public static async Task<PaginationAsList<T>> GetPagedToListAsync<T>(this IQueryable<T> query, QueryParams qParams) where T : class
        {
            PaginationDto<T> result = await query.GetPagedAsync(qParams);
            return new PaginationAsList<T>
            {
                currentPage = result.currentPage,
                pageSize = result.pageSize,
                data = result.data.ToList(),
                total = result.total,
                totalPages = result.totalPages,
                totalData = result.totalData,
            };
        }
    }
}
