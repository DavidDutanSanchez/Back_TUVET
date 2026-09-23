using Microsoft.EntityFrameworkCore;
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class MascotasService(TuVetContext context)
        : IControladorMascotas
    {
        private readonly TuVetContext _context = context;

        // =========================================================
        // VALIDAR DATOS ADICIONALES
        // =========================================================

        private static string? ValidarDatosAdicionales(
            Mascotas mascota
        )
        {
            // Tamaño:
            // 1 = Pequeño
            // 2 = Mediano
            // 3 = Grande
            // 4 = Gigante

            if (
                mascota.Tamanio.HasValue &&
                (
                    mascota.Tamanio.Value < 1 ||
                    mascota.Tamanio.Value > 4
                )
            )
            {
                return "Debe seleccionar un tamaño válido.";
            }

            // El peso es opcional, pero si se registra
            // debe ser mayor a cero.

            if (
                mascota.PesoKg.HasValue &&
                mascota.PesoKg.Value <= 0
            )
            {
                return "El peso debe ser mayor a cero.";
            }

            // Límite permitido por DECIMAL(7,2).

            if (
                mascota.PesoKg.HasValue &&
                mascota.PesoKg.Value > 99999.99m
            )
            {
                return "El peso excede el valor permitido.";
            }

            // Alimentación:
            // 1 = Casera
            // 2 = Mixta
            // 3 = Balanceado

            if (
                mascota.TipoAlimentacion.HasValue &&
                (
                    mascota.TipoAlimentacion.Value < 1 ||
                    mascota.TipoAlimentacion.Value > 3
                )
            )
            {
                return "Debe seleccionar un tipo de alimentación válido.";
            }

            // VacunasAlDia es bool?:
            // null = Sin registrar
            // false = No
            // true = Sí

            return null;
        }

        // =========================================================
        // VALIDAR DATOS GENERALES
        // =========================================================

        private static string? ValidarMascota(
            Mascotas mascota
        )
        {
            if (string.IsNullOrWhiteSpace(mascota.Nombre))
            {
                return "El nombre de la mascota es obligatorio.";
            }

            if (mascota.Id_Persona == Guid.Empty)
            {
                return "Debe seleccionar un propietario.";
            }

            if (mascota.Id_Especie == Guid.Empty)
            {
                return "Debe seleccionar una especie.";
            }

            if (mascota.Id_Raza == Guid.Empty)
            {
                return "Debe seleccionar una raza.";
            }

            if (mascota.Id_Color == Guid.Empty)
            {
                return "Debe seleccionar un color.";
            }

            // =====================================================
            // VALIDAR EDAD
            // =====================================================

            if (mascota.EdadEsAproximada)
            {
                if (
                    mascota.EdadAproximada == null ||
                    mascota.EdadAproximada < 0
                )
                {
                    return "Debe ingresar una edad aproximada válida.";
                }

                // No inventamos una fecha de nacimiento.
                mascota.FechaDeNacimiento = null;
            }
            else
            {
                if (mascota.FechaDeNacimiento == null)
                {
                    return "Debe ingresar la fecha de nacimiento.";
                }

                // La edad se calculará a partir de la fecha.
                mascota.EdadAproximada = null;
            }

            // =====================================================
            // VALIDAR CAMPOS NUEVOS
            // =====================================================

            return ValidarDatosAdicionales(mascota);
        }

        // =========================================================
        // NORMALIZAR TEXTOS
        // =========================================================

        private static void NormalizarMascota(
            Mascotas mascota
        )
        {
            mascota.Nombre = mascota.Nombre.Trim();

            mascota.CodigoMicrochip =
                string.IsNullOrWhiteSpace(
                    mascota.CodigoMicrochip
                )
                    ? null
                    : mascota.CodigoMicrochip.Trim();

            mascota.EnfermedadesPreexistentes =
                string.IsNullOrWhiteSpace(
                    mascota.EnfermedadesPreexistentes
                )
                    ? null
                    : mascota.EnfermedadesPreexistentes.Trim();
        }

        // =========================================================
        // LISTAR MASCOTAS
        // =========================================================

        public async Task<PaginationDto<Mascotas>> AllMascotas(
            QueryParams qParams
        )
        {
            try
            {
                IQueryable<Mascotas> query =
                    _context.Mascotas
                        .AsNoTracking();

                // =================================================
                // BÚSQUEDA
                // =================================================

                if (!string.IsNullOrWhiteSpace(qParams.search))
                {
                    string search =
                        qParams.search.Trim();

                    query = query.Where(m =>
                        m.Nombre.Contains(search) ||
                        (
                            m.CodigoMicrochip != null &&
                            m.CodigoMicrochip.Contains(search)
                        ) ||
                        (
                            m.EnfermedadesPreexistentes != null &&
                            m.EnfermedadesPreexistentes.Contains(search)
                        )
                    );
                }

                // =================================================
                // TOTAL
                // =================================================

                int total =
                    await query.CountAsync();

                // =================================================
                // ORDENAMIENTO
                // =================================================

                string campoOrdenamiento =
                    qParams.orderBy?
                        .Trim()
                        .ToLower()
                    ?? "nombre";

                query = campoOrdenamiento switch
                {
                    "fechadenacimiento" =>
                        qParams.isOrderByDescending
                            ? query.OrderByDescending(
                                m => m.FechaDeNacimiento
                            )
                            : query.OrderBy(
                                m => m.FechaDeNacimiento
                            ),

                    "edadaproximada" =>
                        qParams.isOrderByDescending
                            ? query.OrderByDescending(
                                m => m.EdadAproximada
                            )
                            : query.OrderBy(
                                m => m.EdadAproximada
                            ),

                    "codigomicrochip" =>
                        qParams.isOrderByDescending
                            ? query.OrderByDescending(
                                m => m.CodigoMicrochip
                            )
                            : query.OrderBy(
                                m => m.CodigoMicrochip
                            ),

                    "tamanio" =>
                        qParams.isOrderByDescending
                            ? query.OrderByDescending(
                                m => m.Tamanio
                            )
                            : query.OrderBy(
                                m => m.Tamanio
                            ),

                    "pesokg" =>
                        qParams.isOrderByDescending
                            ? query.OrderByDescending(
                                m => m.PesoKg
                            )
                            : query.OrderBy(
                                m => m.PesoKg
                            ),

                    "tipoalimentacion" =>
                        qParams.isOrderByDescending
                            ? query.OrderByDescending(
                                m => m.TipoAlimentacion
                            )
                            : query.OrderBy(
                                m => m.TipoAlimentacion
                            ),

                    _ =>
                        qParams.isOrderByDescending
                            ? query.OrderByDescending(
                                m => m.Nombre
                            )
                            : query.OrderBy(
                                m => m.Nombre
                            )
                };

                // =================================================
                // PAGINACIÓN
                // =================================================

                int page =
                    qParams.page <= 0
                        ? 1
                        : qParams.page;

                int pageSize =
                    qParams.pageSize <= 0
                        ? 10
                        : qParams.pageSize;

                List<Mascotas> lista =
                    await query
                        .Skip(
                            (page - 1) * pageSize
                        )
                        .Take(pageSize)
                        .ToListAsync();

                int totalPages =
                    total == 0
                        ? 0
                        : (int)Math.Ceiling(
                            total / (double)pageSize
                        );

                // Mantener compatibilidad con PaginationDto.

                IQueryable<Mascotas> data =
                    lista.AsQueryable();

                PaginationDto<Mascotas> resultado =
                    new PaginationDto<Mascotas>
                    {
                        currentPage = page,
                        pageSize = pageSize,
                        totalPages = totalPages,
                        total = total,
                        data = data
                    };

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error al obtener las mascotas: " +
                    (
                        ex.InnerException?.Message
                        ?? ex.Message
                    ),
                    ex
                );
            }
        }

        // =========================================================
        // CREAR MASCOTA
        // =========================================================

        public async Task<string> CreateMascota(
            Mascotas mascota
        )
        {
            try
            {
                if (mascota.IdMascota == Guid.Empty)
                {
                    mascota.IdMascota =
                        Guid.NewGuid();
                }

                // =================================================
                // VALIDACIONES
                // =================================================

                string? errorValidacion =
                    ValidarMascota(mascota);

                if (errorValidacion != null)
                {
                    return errorValidacion;
                }

                // =================================================
                // NORMALIZACIÓN
                // =================================================

                NormalizarMascota(mascota);

                // =================================================
                // GUARDAR
                // =================================================

                _context.Mascotas.Add(
                    mascota
                );

                await _context
                    .SaveChangesAsync();

                return "Realizado";
            }
            catch (Exception ex)
            {
                return (
                    ex.InnerException?.Message
                    ?? ex.Message
                );
            }
        }

        // =========================================================
        // ACTUALIZAR MASCOTA
        // =========================================================

        public async Task<string> UpdateMascota(
            Mascotas mascota
        )
        {
            try
            {
                Mascotas? mascotaExistente =
                    await _context.Mascotas
                        .FindAsync(
                            mascota.IdMascota
                        );

                if (mascotaExistente == null)
                {
                    return "La mascota no existe.";
                }

                // =================================================
                // VALIDACIONES
                // =================================================

                string? errorValidacion =
                    ValidarMascota(mascota);

                if (errorValidacion != null)
                {
                    return errorValidacion;
                }

                // =================================================
                // NORMALIZACIÓN
                // =================================================

                NormalizarMascota(mascota);

                // =================================================
                // ACTUALIZAR
                // =================================================

                _context
                    .Entry(mascotaExistente)
                    .CurrentValues
                    .SetValues(mascota);

                await _context
                    .SaveChangesAsync();

                return "Realizado";
            }
            catch (Exception ex)
            {
                return (
                    ex.InnerException?.Message
                    ?? ex.Message
                );
            }
        }

        // =========================================================
        // ELIMINAR MASCOTA
        // =========================================================

        public async Task<string> DeleteMascota(
            Guid iD
        )
        {
            try
            {
                Mascotas? mascota =
                    await _context.Mascotas
                        .FindAsync(iD);

                if (mascota == null)
                {
                    return "La mascota no existe.";
                }

                _context.Mascotas.Remove(
                    mascota
                );

                await _context
                    .SaveChangesAsync();

                return "Realizado";
            }
            catch (Exception ex)
            {
                return (
                    ex.InnerException?.Message
                    ?? ex.Message
                );
            }
        }
    }
}