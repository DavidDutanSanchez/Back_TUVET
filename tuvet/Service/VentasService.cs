using Microsoft.EntityFrameworkCore;
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class VentasService(
        TuVetContext context
    ) : IControladorVentas
    {
        private readonly TuVetContext _context = context;

        // =====================================================
        // LISTAR
        // =====================================================

        public async Task<List<VentaListadoDto>>
            ObtenerVentas()
        {
            return await (
                from v in _context.VentaCabeceras

                join p0 in _context.Personas
                    on v.Id_Persona equals
                    (Guid?)p0.IdPersona into personas

                from p in personas.DefaultIfEmpty()

                join u in _context.Usuarios
                    on v.Id_Usuario equals u.IdUsuario

                orderby v.FechaVenta descending

                select new VentaListadoDto
                {
                    IdVenta = v.IdVenta,

                    NumeroVenta = v.NumeroVenta,

                    FechaVenta = v.FechaVenta,

                    Id_Persona = v.Id_Persona,

                    Cliente = p == null
                        ? "CONSUMIDOR FINAL"
                        : p.Nombres + " " + p.Apellidos,

                    Identificacion = p == null
                        ? string.Empty
                        : p.NumeroIdentificacion,

                    Usuario = u.NombreUsuario,

                    Total = v.Total,

                    EstadoVenta = v.EstadoVenta
                }
            )
            .AsNoTracking()
            .ToListAsync();
        }

        // =====================================================
        // VENTA COMPLETA
        // =====================================================

        public async Task<VentaCompletaDto?>
            ObtenerVenta(Guid idVenta)
        {
            var venta = await _context.VentaCabeceras
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.IdVenta == idVenta
                );

            if (venta == null)
            {
                return null;
            }

            Personas? persona = null;

            if (venta.Id_Persona.HasValue)
            {
                persona = await _context.Personas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        x =>
                            x.IdPersona ==
                            venta.Id_Persona.Value
                    );
            }

            var detalles = await (
                from d in _context.VentaDetalles

                join m0 in _context.Mascotas
                    on d.Id_Mascota equals
                    (Guid?)m0.IdMascota into mascotas

                from m in mascotas.DefaultIfEmpty()

                where d.Id_Venta == idVenta

                select new VentaDetalleConsultaDto
                {
                    IdVentaDetalle =
                        d.IdVentaDetalle,

                    Id_Mascota =
                        d.Id_Mascota,

                    Mascota =
                        m == null ? null : m.Nombre,

                    TipoDetalle =
                        d.TipoDetalle,

                    Id_Producto =
                        d.Id_Producto,

                    Id_Servicio =
                        d.Id_Servicio,

                    Descripcion =
                        d.Descripcion,

                    Cantidad =
                        d.Cantidad,

                    PrecioUnitario =
                        d.PrecioUnitario,

                    Descuento =
                        d.Descuento,

                    ValorImpuesto =
                        d.ValorImpuesto,

                    TotalLinea =
                        d.TotalLinea
                }
            )
            .AsNoTracking()
            .ToListAsync();

            var pagos = await _context.VentaPagos
                .AsNoTracking()
                .Where(x => x.Id_Venta == idVenta)
                .Select(x => new VentaPagoConsultaDto
                {
                    TipoPago = x.TipoPago,
                    Monto = x.Monto,
                    Referencia = x.Referencia
                })
                .ToListAsync();

            return new VentaCompletaDto
            {
                IdVenta = venta.IdVenta,

                NumeroVenta = venta.NumeroVenta,

                FechaVenta = venta.FechaVenta,

                Id_Persona = venta.Id_Persona,

                Cliente = persona == null
                    ? "CONSUMIDOR FINAL"
                    : persona.Nombres + " " +
                      persona.Apellidos,

                Identificacion =
                    persona?.NumeroIdentificacion
                    ?? string.Empty,

                Telefono =
                    persona?.Telefono
                    ?? string.Empty,

                Direccion =
                    persona?.Direccion
                    ?? string.Empty,

                Subtotal = venta.Subtotal,

                DescuentoTotal =
                    venta.DescuentoTotal,

                ImpuestoTotal =
                    venta.ImpuestoTotal,

                Total = venta.Total,

                MontoPagado =
                    venta.MontoPagado,

                Cambio =
                    venta.Cambio,

                EstadoVenta =
                    venta.EstadoVenta,

                Observaciones =
                    venta.Observaciones,

                Detalles = detalles,

                Pagos = pagos
            };
        }

        // =====================================================
        // POR PERSONA
        // =====================================================

        public async Task<List<VentaListadoDto>>
            ObtenerVentasPorPersona(Guid idPersona)
        {
            var ventas = await ObtenerVentas();

            return ventas
                .Where(
                    x => x.Id_Persona == idPersona
                )
                .ToList();
        }

        // =====================================================
        // POR MASCOTA
        // =====================================================

        public async Task<List<VentaListadoDto>>
            ObtenerVentasPorMascota(Guid idMascota)
        {
            var ids = await _context.VentaDetalles
                .AsNoTracking()
                .Where(
                    x => x.Id_Mascota == idMascota
                )
                .Select(x => x.Id_Venta)
                .Distinct()
                .ToListAsync();

            var ventas = await ObtenerVentas();

            return ventas
                .Where(x => ids.Contains(x.IdVenta))
                .ToList();
        }

        // =====================================================
        // STOCK
        // =====================================================

        public async Task<decimal>
            ObtenerStockProducto(Guid idProducto)
        {
            /*
             * Inventarios actual:
             *
             * TipoMovimiento = 1 -> Entrada
             * TipoMovimiento = 2 -> Salida
             */

            var entradas = await _context.Inventarios
                .Where(
                    x =>
                        x.Id_Producto == idProducto
                        && x.TipoMovimiento == 1
                )
                .SumAsync(x => (decimal?)x.Cantidad)
                ?? 0;

            var salidas = await _context.Inventarios
                .Where(
                    x =>
                        x.Id_Producto == idProducto
                        && x.TipoMovimiento == 2
                )
                .SumAsync(x => (decimal?)x.Cantidad)
                ?? 0;

            return entradas - salidas;
        }


        // =====================================================
// VERIFICAR SI PRODUCTO MANEJA INVENTARIO
// =====================================================

private async Task<bool> ProductoTieneInventario(
    Guid idProducto
)
{
    return await _context.Inventarios
        .AsNoTracking()
        .AnyAsync(
            x => x.Id_Producto == idProducto
        );
}

        // =====================================================
        // CREAR VENTA
        // =====================================================

        public async Task<VentaResultadoDto>
            CrearVenta(CrearVentaDto dto)
        {
            if (dto.Detalles.Count == 0)
            {
                throw new InvalidOperationException(
                    "Debe agregar al menos un detalle."
                );
            }

            await ValidarUsuario(dto.Id_Usuario);

            if (dto.Id_Persona.HasValue)
            {
                var personaExiste =
                    await _context.Personas.AnyAsync(
                        x =>
                            x.IdPersona ==
                            dto.Id_Persona.Value
                    );

                if (!personaExiste)
                {
                    throw new InvalidOperationException(
                        "El cliente seleccionado no existe."
                    );
                }
            }

            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();

            try
            {
                var idVenta = Guid.NewGuid();

                var numeroVenta =
                    await GenerarNumeroVenta();

                var detalles =
                    new List<VentaDetalle>();

                decimal subtotalSinImpuestos = 0;
                decimal subtotalConImpuestos = 0;
                decimal descuentoTotal = 0;
                decimal impuestoTotal = 0;
                decimal subtotal = 0;
                decimal total = 0;

                foreach (var item in dto.Detalles)
                {
                    if (item.Cantidad <= 0)
                    {
                        throw new InvalidOperationException(
                            "La cantidad debe ser mayor a cero."
                        );
                    }

                    await ValidarMascotaCliente(
                        dto.Id_Persona,
                        item.Id_Mascota
                    );

                    var detalle =
                        await ConstruirDetalle(
                            idVenta,
                            item
                        );

                    detalles.Add(detalle);

                    descuentoTotal +=
                        detalle.Descuento;

                    impuestoTotal +=
                        detalle.ValorImpuesto;

                    subtotal +=
                        detalle.BaseImponible;

                    total +=
                        detalle.TotalLinea;

                    if (detalle.ValorImpuesto > 0)
                    {
                        subtotalConImpuestos +=
                            detalle.BaseImponible;
                    }
                    else
                    {
                        subtotalSinImpuestos +=
                            detalle.BaseImponible;
                    }
                }

                total = Math.Round(total, 2);

                var montoPagado =
                    dto.Pagos.Sum(x => x.Monto);

                if (montoPagado < total)
                {
                    throw new InvalidOperationException(
                        $"El monto pagado ({montoPagado:F2}) " +
                        $"es menor al total ({total:F2})."
                    );
                }

                foreach (var pago in dto.Pagos)
                {
                    if (pago.Monto <= 0)
                    {
                        throw new InvalidOperationException(
                            "Los pagos deben ser mayores a cero."
                        );
                    }

                    if (pago.TipoPago < 1 ||
                        pago.TipoPago > 4)
                    {
                        throw new InvalidOperationException(
                            "Forma de pago no válida."
                        );
                    }
                }

                var cambio =
                    Math.Round(
                        montoPagado - total,
                        2
                    );

                var cabecera = new VentaCabecera
                {
                    IdVenta = idVenta,

                    NumeroVenta = numeroVenta,

                    FechaVenta = DateTime.Now,

                    Id_Persona = dto.Id_Persona,

                    Id_Usuario = dto.Id_Usuario,

                    SubtotalSinImpuestos =
                        Math.Round(
                            subtotalSinImpuestos,
                            2
                        ),

                    SubtotalConImpuestos =
                        Math.Round(
                            subtotalConImpuestos,
                            2
                        ),

                    DescuentoTotal =
                        Math.Round(
                            descuentoTotal,
                            2
                        ),

                    ImpuestoTotal =
                        Math.Round(
                            impuestoTotal,
                            2
                        ),

                    Subtotal =
                        Math.Round(subtotal, 2),

                    Total = total,

                    MontoPagado =
                        Math.Round(
                            montoPagado,
                            2
                        ),

                    Cambio = cambio,

                    EstadoVenta = 1,

                    Observaciones =
                        dto.Observaciones,

                    FechaCreacion =
                        DateTime.Now
                };

                _context.VentaCabeceras.Add(
                    cabecera
                );

                _context.VentaDetalles.AddRange(
                    detalles
                );

                /*
                 * Guardamos primero para que existan
                 * cabecera y detalles antes de crear
                 * movimientos relacionados.
                 */
                await _context.SaveChangesAsync();

                // =============================================
                // DESCONTAR PRODUCTOS
                // =============================================
foreach (
    var detalle in detalles.Where(
        x =>
            x.TipoDetalle == 1 &&
            x.Id_Producto.HasValue
    )
)
{
    /*
     * El inventario actual trabaja con
     * cantidades enteras.
     */
    if (
        detalle.Cantidad !=
        decimal.Truncate(
            detalle.Cantidad
        )
    )
    {
        throw new InvalidOperationException(
            $"El producto '{detalle.Descripcion}' " +
            "solo permite cantidades enteras."
        );
    }


    var cantidad =
        (int)detalle.Cantidad;


    var idProducto =
        detalle.Id_Producto!.Value;


    /*
     * IMPORTANTE:
     *
     * Un producto puede existir en el catálogo
     * sin que todavía se haya realizado su
     * inventario inicial.
     *
     * Si nunca tuvo movimientos de inventario,
     * permitimos venderlo sin afectar stock.
     *
     * Cuando el producto tenga al menos un
     * movimiento, automáticamente empezará
     * a trabajar con control de existencias.
     */
    var manejaInventario =
        await ProductoTieneInventario(
            idProducto
        );


    if (!manejaInventario)
    {
        /*
         * Inventario todavía no inicializado.
         *
         * NO generamos una salida ficticia.
         * NO bloqueamos la venta.
         */
        continue;
    }


    var stockAnterior =
        await ObtenerStockProducto(
            idProducto
        );


    if (stockAnterior < cantidad)
    {
        throw new InvalidOperationException(
            $"Stock insuficiente para " +
            $"'{detalle.Descripcion}'. " +
            $"Disponible: {stockAnterior}. " +
            $"Solicitado: {cantidad}."
        );
    }


    var stockNuevo =
        stockAnterior - cantidad;


    /*
     * Inventario principal.
     *
     * TipoMovimiento = 2
     * SALIDA POR VENTA
     */
    _context.Inventarios.Add(
        new Inventarios
        {
            IdInventario =
                Guid.NewGuid(),

            Id_Producto =
                idProducto,

            FechaMovimiento =
                DateTime.Now,

            TipoMovimiento =
                2,

            Cantidad =
                cantidad,

            FechaCaducidad =
                null
        }
    );


    /*
     * Auditoría específica de Caja.
     */
    _context.MovimientosInventario.Add(
        new MovimientoInventario
        {
            IdMovimientoInventario =
                Guid.NewGuid(),

            Id_Producto =
                idProducto,

            Id_Venta =
                idVenta,

            Id_VentaDetalle =
                detalle.IdVentaDetalle,

            FechaMovimiento =
                DateTime.Now,

            TipoMovimiento =
                2,

            Cantidad =
                cantidad,

            StockAnterior =
                stockAnterior,

            StockNuevo =
                stockNuevo,

            Motivo =
                "Salida por venta " +
                numeroVenta,

            Id_Usuario =
                dto.Id_Usuario,

            FechaCreacion =
                DateTime.Now
        }
    );
}
                // =============================================
                // PAGOS
                // =============================================

                foreach (var pagoDto in dto.Pagos)
                {
                    _context.VentaPagos.Add(
                        new VentaPago
                        {
                            IdVentaPago =
                                Guid.NewGuid(),

                            Id_Venta =
                                idVenta,

                            TipoPago =
                                pagoDto.TipoPago,

                            Monto =
                                pagoDto.Monto,

                            Referencia =
                                pagoDto.Referencia,

                            Observaciones =
                                pagoDto.Observaciones,

                            Id_Usuario =
                                dto.Id_Usuario,

                            FechaPago =
                                DateTime.Now
                        }
                    );
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new VentaResultadoDto
                {
                    IdVenta = idVenta,

                    NumeroVenta = numeroVenta,

                    Total = total,

                    MontoPagado =
                        montoPagado,

                    Cambio = cambio
                };
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }

        // =====================================================
        // ANULAR
        // =====================================================

        public async Task<string>
            AnularVenta(AnularVentaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Motivo))
            {
                throw new InvalidOperationException(
                    "Debe indicar el motivo de la anulación."
                );
            }

            await ValidarUsuario(dto.Id_Usuario);

            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();

            try
            {
                var venta =
                    await _context.VentaCabeceras
                        .FirstOrDefaultAsync(
                            x =>
                                x.IdVenta ==
                                dto.IdVenta
                        );

                if (venta == null)
                {
                    throw new InvalidOperationException(
                        "Venta no encontrada."
                    );
                }

                if (venta.EstadoVenta == 2)
                {
                    throw new InvalidOperationException(
                        "La venta ya está anulada."
                    );
                }

                var detalles =
                    await _context.VentaDetalles
                        .Where(
                            x =>
                                x.Id_Venta ==
                                dto.IdVenta
                        )
                        .ToListAsync();

               foreach (
    var detalle in detalles.Where(
        x =>
            x.TipoDetalle == 1 &&
            x.Id_Producto.HasValue
    )
)
{
    var idProducto =
        detalle.Id_Producto!.Value;


    /*
     * Buscamos específicamente si esta venta
     * produjo una salida de inventario.
     *
     * No basta con preguntar si el producto
     * actualmente tiene inventario, porque
     * pudo haber sido inicializado después
     * de realizar esta venta.
     */
    var movimientoVenta =
        await _context
            .MovimientosInventario
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x =>
                    x.Id_Venta ==
                        venta.IdVenta &&
                    x.Id_VentaDetalle ==
                        detalle.IdVentaDetalle &&
                    x.Id_Producto ==
                        idProducto &&
                    x.TipoMovimiento == 2
            );


    /*
     * Si la venta original no descontó
     * inventario, la anulación tampoco
     * debe devolver unidades.
     */
    if (movimientoVenta == null)
    {
        continue;
    }


    if (
        detalle.Cantidad !=
        decimal.Truncate(
            detalle.Cantidad
        )
    )
    {
        throw new InvalidOperationException(
            $"La cantidad del producto " +
            $"'{detalle.Descripcion}' " +
            "no es válida para inventario."
        );
    }


    var cantidad =
        (int)detalle.Cantidad;


    var stockAnterior =
        await ObtenerStockProducto(
            idProducto
        );


    /*
     * Devolución al inventario.
     *
     * TipoMovimiento = 1
     * ENTRADA
     */
    _context.Inventarios.Add(
        new Inventarios
        {
            IdInventario =
                Guid.NewGuid(),

            Id_Producto =
                idProducto,

            FechaMovimiento =
                DateTime.Now,

            TipoMovimiento =
                1,

            Cantidad =
                cantidad,

            FechaCaducidad =
                null
        }
    );


    /*
     * Auditoría de la devolución.
     */
    _context.MovimientosInventario.Add(
        new MovimientoInventario
        {
            IdMovimientoInventario =
                Guid.NewGuid(),

            Id_Producto =
                idProducto,

            Id_Venta =
                venta.IdVenta,

            Id_VentaDetalle =
                detalle.IdVentaDetalle,

            FechaMovimiento =
                DateTime.Now,

            TipoMovimiento =
                5,

            Cantidad =
                cantidad,

            StockAnterior =
                stockAnterior,

            StockNuevo =
                stockAnterior +
                cantidad,

            Motivo =
                "Devolución por anulación: " +
                dto.Motivo,

            Id_Usuario =
                dto.Id_Usuario,

            FechaCreacion =
                DateTime.Now
        }
    );
}

                venta.EstadoVenta = 2;

                venta.Id_UsuarioAnulacion =
                    dto.Id_Usuario;

                venta.FechaAnulacion =
                    DateTime.Now;

                venta.MotivoAnulacion =
                    dto.Motivo.Trim();

                venta.FechaModificacion =
                    DateTime.Now;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return "Realizado";
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }

        // =====================================================
        // CONSTRUIR DETALLE
        // =====================================================

        private async Task<VentaDetalle>
            ConstruirDetalle(
                Guid idVenta,
                CrearVentaDetalleDto item
            )
        {
            string descripcion;
            decimal precio;

            switch (item.TipoDetalle)
            {
                // PRODUCTO
                case 1:
                {
                    if (!item.Id_Producto.HasValue)
                    {
                        throw new InvalidOperationException(
                            "Debe seleccionar un producto."
                        );
                    }

                    var producto =
                        await _context.Productos
                            .AsNoTracking()
                            .FirstOrDefaultAsync(
                                x =>
                                    x.IdProductos ==
                                    item.Id_Producto.Value
                            );

                    if (producto == null)
                    {
                        throw new InvalidOperationException(
                            "Producto no encontrado."
                        );
                    }

                    descripcion =
                        producto.NombreProducto;

                    precio =
                        producto.PrecioVenta;

                    break;
                }

                // SERVICIO
                case 2:
                {
                    if (!item.Id_Servicio.HasValue)
                    {
                        throw new InvalidOperationException(
                            "Debe seleccionar un servicio."
                        );
                    }

                    var servicio =
                        await _context.Servicios
                            .AsNoTracking()
                            .FirstOrDefaultAsync(
                                x =>
                                    x.IdServicios ==
                                    item.Id_Servicio.Value
                            );

                    if (servicio == null)
                    {
                        throw new InvalidOperationException(
                            "Servicio no encontrado."
                        );
                    }

                    descripcion =
                        servicio.NombreServicio;

                    /*
                     * Según la estructura que mostraste:
                     * PreciosServicio.
                     */
                    precio =
                        servicio.PreciosServicio;

                    break;
                }

                // VARIOS
                case 3:
                {
                    if (string.IsNullOrWhiteSpace(
                        item.Descripcion
                    ))
                    {
                        throw new InvalidOperationException(
                            "Debe ingresar la descripción " +
                            "del concepto varios."
                        );
                    }

                    if (!item.PrecioUnitario.HasValue ||
                        item.PrecioUnitario <= 0)
                    {
                        throw new InvalidOperationException(
                            "Debe ingresar el valor del concepto."
                        );
                    }

                    descripcion =
                        item.Descripcion.Trim();

                    precio =
                        item.PrecioUnitario.Value;

                    break;
                }

                default:
                    throw new InvalidOperationException(
                        "Tipo de detalle no válido."
                    );
            }

            var bruto =
                item.Cantidad * precio;

            if (item.Descuento < 0 ||
                item.Descuento > bruto)
            {
                throw new InvalidOperationException(
                    $"Descuento no válido para '{descripcion}'."
                );
            }

            if (item.PorcentajeImpuesto < 0)
            {
                throw new InvalidOperationException(
                    "El porcentaje de impuesto no es válido."
                );
            }

            var baseImponible =
                bruto - item.Descuento;

            var impuesto =
                baseImponible *
                (item.PorcentajeImpuesto / 100m);

            var totalLinea =
                baseImponible + impuesto;

            return new VentaDetalle
            {
                IdVentaDetalle =
                    Guid.NewGuid(),

                Id_Venta =
                    idVenta,

                Id_Mascota =
                    item.Id_Mascota,

                TipoDetalle =
                    item.TipoDetalle,

                Id_Producto =
                    item.TipoDetalle == 1
                        ? item.Id_Producto
                        : null,

                Id_Servicio =
                    item.TipoDetalle == 2
                        ? item.Id_Servicio
                        : null,

                Descripcion =
                    descripcion,

                Cantidad =
                    item.Cantidad,

                PrecioUnitario =
                    precio,

                Descuento =
                    Math.Round(
                        item.Descuento,
                        2
                    ),

                PorcentajeImpuesto =
                    item.PorcentajeImpuesto,

                BaseImponible =
                    Math.Round(
                        baseImponible,
                        2
                    ),

                ValorImpuesto =
                    Math.Round(
                        impuesto,
                        2
                    ),

                TotalLinea =
                    Math.Round(
                        totalLinea,
                        2
                    ),

                Observaciones =
                    item.Observaciones,

                FechaCreacion =
                    DateTime.Now
            };
        }

        // =====================================================
        // VALIDAR MASCOTA / CLIENTE
        // =====================================================

        private async Task ValidarMascotaCliente(
            Guid? idPersona,
            Guid? idMascota
        )
        {
            if (!idMascota.HasValue)
            {
                return;
            }

            var mascota =
                await _context.Mascotas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        x =>
                            x.IdMascota ==
                            idMascota.Value
                    );

            if (mascota == null)
            {
                throw new InvalidOperationException(
                    "La mascota seleccionada no existe."
                );
            }

            /*
             * Si se selecciona una mascota,
             * la venta debe pertenecer a su propietario.
             */
            if (!idPersona.HasValue)
            {
                throw new InvalidOperationException(
                    "Para asociar una mascota debe " +
                    "seleccionar a su propietario."
                );
            }

            if (mascota.Id_Persona != idPersona.Value)
            {
                throw new InvalidOperationException(
                    $"La mascota '{mascota.Nombre}' " +
                    "no pertenece al cliente seleccionado."
                );
            }
        }

        // =====================================================
        // USUARIO
        // =====================================================

        private async Task ValidarUsuario(
            Guid idUsuario
        )
        {
            var existe =
                await _context.Usuarios
                    .AnyAsync(
                        x =>
                            x.IdUsuario ==
                            idUsuario &&
                            x.Estado
                    );

            if (!existe)
            {
                throw new InvalidOperationException(
                    "El usuario no existe o está inactivo."
                );
            }
        }

        // =====================================================
        // NUMERACIÓN
        // =====================================================

        private async Task<string>
            GenerarNumeroVenta()
        {
            var ultima =
                await _context.VentaCabeceras
                    .AsNoTracking()
                    .OrderByDescending(
                        x => x.FechaCreacion
                    )
                    .Select(x => x.NumeroVenta)
                    .FirstOrDefaultAsync();

            var siguiente = 1;

            if (!string.IsNullOrWhiteSpace(ultima))
            {
                var partes =
                    ultima.Split('-');

                if (partes.Length == 2 &&
                    int.TryParse(
                        partes[1],
                        out var numero
                    ))
                {
                    siguiente =
                        numero + 1;
                }
            }

            return $"V-{siguiente:D6}";
        }
    }
}