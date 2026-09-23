using Microsoft.EntityFrameworkCore;
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class ServiciosService(
        TuVetContext context)
        : IControladorServicios
    {
        private readonly TuVetContext
            _context = context;


        public async Task<List<ServicioDto>>
            FindAllServicios(
                bool soloActivos = false)
        {
            IQueryable<Servicios> query =
                _context.Servicios
                    .AsNoTracking()
                    .Include(
                        s => s.Tarifas
                    );


            if (soloActivos)
            {
                query =
                    query.Where(
                        s => s.Activo
                    );
            }


            List<Servicios> servicios =
                await query
                    .OrderBy(
                        s =>
                            s.NombreServicio
                    )
                    .ToListAsync();


            return servicios
                .Select(
                    MapearServicio
                )
                .ToList();
        }


        public async Task<ServicioDto?>
            FindServicioById(
                Guid id)
        {
            Servicios? servicio =
                await _context.Servicios
                    .AsNoTracking()
                    .Include(
                        s => s.Tarifas
                    )
                    .FirstOrDefaultAsync(
                        s =>
                            s.IdServicios == id
                    );


            if (servicio == null)
            {
                return null;
            }


            return MapearServicio(
                servicio
            );
        }


        public async Task<string>
            CreateServicio(
                GuardarServicioDto dto)
        {
            string validacion =
                ValidarServicio(dto);


            if (
                validacion !=
                "Realizado"
            )
            {
                return validacion;
            }


            string nombre =
                dto.NombreServicio
                    .Trim();


            bool existe =
                await _context.Servicios
                    .AnyAsync(
                        s =>
                            s.NombreServicio
                                .ToLower() ==
                            nombre.ToLower()
                    );


            if (existe)
            {
                return
                    "Ya existe un servicio con ese nombre.";
            }


            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();


            try
            {
                var servicio =
                    new Servicios
                    {
                        IdServicios =
                            Guid.NewGuid(),

                        NombreServicio =
                            nombre,

                        DescripcionServicio =
                            LimpiarTexto(
                                dto.DescripcionServicio
                            ),

                        PreciosServicio =
                            dto.TipoPrecio == 0
                                ? dto.PreciosServicio
                                : 0,

                        IncluyeIva =
                            dto.IncluyeIva,

                        DescuentoServicio =
                            dto.DescuentoServicio,

                        DuracionMinutos =
                            dto.DuracionMinutos,

                        TipoPrecio =
                            dto.TipoPrecio,

                        Activo =
                            dto.Activo
                    };


                await _context.Servicios
                    .AddAsync(
                        servicio
                    );


                if (
                    dto.TipoPrecio == 1 ||
                    dto.TipoPrecio == 2
                )
                {
                    foreach (
                        GuardarServicioTarifaDto tarifa
                        in dto.Tarifas
                    )
                    {
                        var nuevaTarifa =
                            CrearTarifa(
                                servicio.IdServicios,
                                tarifa
                            );

                        await _context
                            .ServicioTarifas
                            .AddAsync(
                                nuevaTarifa
                            );
                    }
                }


                await _context
                    .SaveChangesAsync();


                await transaction
                    .CommitAsync();


                return "Realizado";
            }
            catch (Exception ex)
            {
                await transaction
                    .RollbackAsync();

                return
                    $"Error al crear el servicio: {ex.Message}";
            }
        }


        public async Task<string>
            UpdateServicio(
                GuardarServicioDto dto)
        {
            if (
                dto.IdServicios == null ||
                dto.IdServicios ==
                    Guid.Empty
            )
            {
                return
                    "El identificador del servicio es obligatorio.";
            }


            string validacion =
                ValidarServicio(dto);


            if (
                validacion !=
                "Realizado"
            )
            {
                return validacion;
            }


            Servicios? servicio =
                await _context.Servicios
                    .Include(
                        s => s.Tarifas
                    )
                    .FirstOrDefaultAsync(
                        s =>
                            s.IdServicios ==
                            dto.IdServicios.Value
                    );


            if (servicio == null)
            {
                return
                    "El servicio no existe.";
            }


            string nombre =
                dto.NombreServicio
                    .Trim();


            bool existeNombre =
                await _context.Servicios
                    .AnyAsync(
                        s =>
                            s.IdServicios !=
                                servicio.IdServicios &&
                            s.NombreServicio
                                .ToLower() ==
                            nombre.ToLower()
                    );


            if (existeNombre)
            {
                return
                    "Ya existe otro servicio con ese nombre.";
            }


            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();


            try
            {
                servicio.NombreServicio =
                    nombre;

                servicio.DescripcionServicio =
                    LimpiarTexto(
                        dto.DescripcionServicio
                    );

                servicio.PreciosServicio =
                    dto.TipoPrecio == 0
                        ? dto.PreciosServicio
                        : 0;

                servicio.IncluyeIva =
                    dto.IncluyeIva;

                servicio.DescuentoServicio =
                    dto.DescuentoServicio;

                servicio.DuracionMinutos =
                    dto.DuracionMinutos;

                servicio.TipoPrecio =
                    dto.TipoPrecio;

                servicio.Activo =
                    dto.Activo;


                /*
                 * Para este módulo administrativo,
                 * las tarifas actuales se reemplazan
                 * por las enviadas desde el formulario.
                 *
                 * Como todavía no existe facturación
                 * histórica relacionada directamente
                 * con IdServicioTarifa, esta operación
                 * es válida en esta etapa.
                 */

                if (
                    servicio.Tarifas.Count > 0
                )
                {
                    _context.ServicioTarifas
                        .RemoveRange(
                            servicio.Tarifas
                        );
                }


                if (
                    dto.TipoPrecio == 1 ||
                    dto.TipoPrecio == 2
                )
                {
                    foreach (
                        GuardarServicioTarifaDto tarifa
                        in dto.Tarifas
                    )
                    {
                        await _context
                            .ServicioTarifas
                            .AddAsync(
                                CrearTarifa(
                                    servicio.IdServicios,
                                    tarifa
                                )
                            );
                    }
                }


                await _context
                    .SaveChangesAsync();


                await transaction
                    .CommitAsync();


                return "Realizado";
            }
            catch (Exception ex)
            {
                await transaction
                    .RollbackAsync();

                return
                    $"Error al actualizar el servicio: {ex.Message}";
            }
        }


        public async Task<string>
            CambiarEstadoServicio(
                Guid id,
                bool activo)
        {
            Servicios? servicio =
                await _context.Servicios
                    .FirstOrDefaultAsync(
                        s =>
                            s.IdServicios == id
                    );


            if (servicio == null)
            {
                return
                    "El servicio no existe.";
            }


            servicio.Activo =
                activo;


            await _context
                .SaveChangesAsync();


            return "Realizado";
        }


        private static string
            ValidarServicio(
                GuardarServicioDto dto)
        {
            if (
                string.IsNullOrWhiteSpace(
                    dto.NombreServicio
                )
            )
            {
                return
                    "El nombre del servicio es obligatorio.";
            }


            if (
                dto.TipoPrecio < 0 ||
                dto.TipoPrecio > 3
            )
            {
                return
                    "El tipo de precio no es válido.";
            }


            if (
                dto.DuracionMinutos <= 0
            )
            {
                return
                    "La duración debe ser mayor a cero.";
            }


            if (
                dto.DescuentoServicio < 0 ||
                dto.DescuentoServicio > 100
            )
            {
                return
                    "El descuento debe estar entre 0 y 100.";
            }


            if (
                dto.TipoPrecio == 0 &&
                dto.PreciosServicio < 0
            )
            {
                return
                    "El precio no puede ser negativo.";
            }


            if (
                (
                    dto.TipoPrecio == 1 ||
                    dto.TipoPrecio == 2
                ) &&
                dto.Tarifas.Count == 0
            )
            {
                return
                    "Debe registrar al menos una tarifa.";
            }


            foreach (
                GuardarServicioTarifaDto tarifa
                in dto.Tarifas
            )
            {
                if (
                    string.IsNullOrWhiteSpace(
                        tarifa.NombreTarifa
                    )
                )
                {
                    return
                        "Todas las tarifas deben tener un nombre.";
                }


                if (
                    tarifa.Precio < 0
                )
                {
                    return
                        "El precio de una tarifa no puede ser negativo.";
                }


                if (
                    tarifa.DuracionMinutos != null &&
                    tarifa.DuracionMinutos <= 0
                )
                {
                    return
                        "La duración de una tarifa debe ser mayor a cero.";
                }


                if (
                    dto.TipoPrecio == 1
                )
                {
                    if (
                        tarifa.PesoMinimo == null
                    )
                    {
                        return
                            "Las tarifas por peso deben indicar el peso mínimo.";
                    }


                    if (
                        tarifa.PesoMaximo != null &&
                        tarifa.PesoMaximo <
                            tarifa.PesoMinimo
                    )
                    {
                        return
                            "El peso máximo no puede ser menor que el peso mínimo.";
                    }
                }


                if (
                    dto.TipoPrecio == 2 &&
                    (
                        tarifa.Tamanio == null ||
                        tarifa.Tamanio < 1 ||
                        tarifa.Tamanio > 4
                    )
                )
                {
                    return
                        "Las tarifas por tamaño deben indicar Pequeño, Mediano, Grande o Gigante.";
                }
            }


            return "Realizado";
        }


        private static ServicioTarifas
            CrearTarifa(
                Guid idServicio,
                GuardarServicioTarifaDto dto)
        {
            return new ServicioTarifas
            {
                IdServicioTarifa =
                    Guid.NewGuid(),

                Id_Servicio =
                    idServicio,

                NombreTarifa =
                    dto.NombreTarifa
                        .Trim(),

                PesoMinimo =
                    dto.PesoMinimo,

                PesoMaximo =
                    dto.PesoMaximo,

                Tamanio =
                    dto.Tamanio,

                Precio =
                    dto.Precio,

                DuracionMinutos =
                    dto.DuracionMinutos,

                Activo =
                    dto.Activo
            };
        }


        private static ServicioDto
            MapearServicio(
                Servicios servicio)
        {
            return new ServicioDto
            {
                IdServicios =
                    servicio.IdServicios,

                NombreServicio =
                    servicio.NombreServicio,

                DescripcionServicio =
                    servicio.DescripcionServicio,

                PreciosServicio =
                    servicio.PreciosServicio,

                IncluyeIva =
                    servicio.IncluyeIva,

                DescuentoServicio =
                    servicio.DescuentoServicio,

                DuracionMinutos =
                    servicio.DuracionMinutos,

                TipoPrecio =
                    servicio.TipoPrecio,

                TipoPrecioDescripcion =
                    ObtenerTipoPrecio(
                        servicio.TipoPrecio
                    ),

                Activo =
                    servicio.Activo,

                Tarifas =
                    servicio.Tarifas
                        .OrderBy(
                            t =>
                                t.Tamanio ??
                                0
                        )
                        .ThenBy(
                            t =>
                                t.PesoMinimo ??
                                0
                        )
                        .Select(
                            t =>
                                new ServicioTarifaDto
                                {
                                    IdServicioTarifa =
                                        t.IdServicioTarifa,

                                    IdServicio =
                                        t.Id_Servicio,

                                    NombreTarifa =
                                        t.NombreTarifa,

                                    PesoMinimo =
                                        t.PesoMinimo,

                                    PesoMaximo =
                                        t.PesoMaximo,

                                    Tamanio =
                                        t.Tamanio,

                                    TamanioDescripcion =
                                        ObtenerTamanio(
                                            t.Tamanio
                                        ),

                                    Precio =
                                        t.Precio,

                                    DuracionMinutos =
                                        t.DuracionMinutos,

                                    Activo =
                                        t.Activo
                                }
                        )
                        .ToList()
            };
        }


        private static string
            ObtenerTipoPrecio(
                int tipo)
        {
            return tipo switch
            {
                0 => "Precio fijo",
                1 => "Por peso",
                2 => "Por tamaño",
                3 => "Por valoración",
                _ => "Desconocido"
            };
        }


        private static string
            ObtenerTamanio(
                int? tamanio)
        {
            return tamanio switch
            {
                1 => "Pequeño",
                2 => "Mediano",
                3 => "Grande",
                4 => "Gigante",
                _ => string.Empty
            };
        }


        private static string?
            LimpiarTexto(
                string? texto)
        {
            return string.IsNullOrWhiteSpace(
                texto
            )
                ? null
                : texto.Trim();
        }
    }
}