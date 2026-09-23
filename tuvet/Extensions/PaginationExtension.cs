using Microsoft.EntityFrameworkCore;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Model.Parameters;

namespace tu_vet_back.tuvet.Extensions
{
    public static class PaginationExtension
    {
        // =========================================================
        // PAGINACIÓN SINCRÓNICA
        // =========================================================

        public static PaginationDto<T> GetPaged<T>(
            this IQueryable<T> query,
            QueryParams? qParams = null
        )
        {
            int pageSize = Math.Max(
                1,
                qParams?.pageSize ?? 10
            );

            int total = query.Count();

            int totalPages =
                total == 0
                    ? 0
                    : (int)Math.Ceiling(
                        total / (double)pageSize
                    );

            int requestedPage = Math.Max(
                1,
                qParams?.page ?? 1
            );

            int currentPage =
                totalPages == 0
                    ? 1
                    : Math.Min(
                        requestedPage,
                        totalPages
                    );

            IQueryable<T> data =
                query
                    .Skip(
                        (currentPage - 1)
                        * pageSize
                    )
                    .Take(pageSize);

            return new PaginationDto<T>
            {
                currentPage = currentPage,
                pageSize = pageSize,
                totalPages = totalPages,
                total = total,

                // No realizamos sumatorias automáticas
                // mediante Reflection dentro de EF.
                totalData = default,

                data = data
            };
        }


        // =========================================================
        // PAGINACIÓN SINCRÓNICA COMO LISTA
        // =========================================================

        public static PaginationAsList<T> GetPagedToList<T>(
            this IQueryable<T> query,
            QueryParams? qParams = null
        )
        {
            PaginationDto<T> result =
                query.GetPaged(qParams);

            return new PaginationAsList<T>
            {
                currentPage =
                    result.currentPage,

                pageSize =
                    result.pageSize,

                totalPages =
                    result.totalPages,

                total =
                    result.total,

                totalData =
                    result.totalData,

                data =
                    result.data.ToList()
            };
        }


        // =========================================================
        // PAGINACIÓN ASÍNCRONA
        // =========================================================

        public static async Task<PaginationDto<T>>
            GetPagedAsync<T>(
                this IQueryable<T> query,
                QueryParams qParams
            )
            where T : class
        {
            int pageSize = Math.Max(
                1,
                qParams.pageSize
            );

            int total =
                await query.CountAsync();

            int totalPages =
                total == 0
                    ? 0
                    : (int)Math.Ceiling(
                        total / (double)pageSize
                    );

            int requestedPage =
                Math.Max(
                    1,
                    qParams.page
                );

            int currentPage =
                totalPages == 0
                    ? 1
                    : Math.Min(
                        requestedPage,
                        totalPages
                    );

            IQueryable<T> data =
                query
                    .Skip(
                        (currentPage - 1)
                        * pageSize
                    )
                    .Take(pageSize);

            return new PaginationDto<T>
            {
                currentPage = currentPage,
                pageSize = pageSize,
                totalPages = totalPages,
                total = total,

                // Se elimina el cálculo mediante Reflection
                // porque EF Core no puede traducirlo a SQL.
                totalData = default,

                data = data
            };
        }


        // =========================================================
        // PAGINACIÓN ASÍNCRONA COMO LISTA
        // =========================================================

        public static async Task<PaginationAsList<T>>
            GetPagedToListAsync<T>(
                this IQueryable<T> query,
                QueryParams qParams
            )
            where T : class
        {
            PaginationDto<T> result =
                await query.GetPagedAsync(
                    qParams
                );

            return new PaginationAsList<T>
            {
                currentPage =
                    result.currentPage,

                pageSize =
                    result.pageSize,

                totalPages =
                    result.totalPages,

                total =
                    result.total,

                totalData =
                    result.totalData,

                data =
                    await result.data
                        .ToListAsync()
            };
        }
    }
}