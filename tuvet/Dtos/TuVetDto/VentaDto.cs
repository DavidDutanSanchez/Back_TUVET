namespace tu_vet_back.tuvet.Dtos.TuVetDto
{
    public class CrearVentaDto
    {
        // NULL = Consumidor final
        public Guid? Id_Persona { get; set; }

        public Guid Id_Usuario { get; set; }

        public string? Observaciones { get; set; }

        public List<CrearVentaDetalleDto> Detalles { get; set; }
            = new();

        public List<CrearVentaPagoDto> Pagos { get; set; }
            = new();
    }

    public class CrearVentaDetalleDto
    {
        // Puede ser diferente en cada línea.
        public Guid? Id_Mascota { get; set; }

        // 1 = Producto
        // 2 = Servicio
        // 3 = Varios
        public int TipoDetalle { get; set; }

        public Guid? Id_Producto { get; set; }

        public Guid? Id_Servicio { get; set; }

        /*
         * Para Producto y Servicio la descripción
         * será obtenida por el backend.
         *
         * Para Varios se utiliza esta descripción.
         */
        public string? Descripcion { get; set; }

        public decimal Cantidad { get; set; } = 1;

        /*
         * Para Producto/Servicio utilizaremos
         * inicialmente el precio registrado.
         *
         * Para Varios el usuario ingresa el valor.
         */
        public decimal? PrecioUnitario { get; set; }

        public decimal Descuento { get; set; }

        public decimal PorcentajeImpuesto { get; set; }

        public string? Observaciones { get; set; }
    }

    public class CrearVentaPagoDto
    {
        public int TipoPago { get; set; }

        public decimal Monto { get; set; }

        public string? Referencia { get; set; }

        public string? Observaciones { get; set; }
    }

    public class AnularVentaDto
    {
        public Guid IdVenta { get; set; }

        public Guid Id_Usuario { get; set; }

        public string Motivo { get; set; } = string.Empty;
    }

    public class VentaResultadoDto
    {
        public Guid IdVenta { get; set; }

        public string NumeroVenta { get; set; } = string.Empty;

        public decimal Total { get; set; }

        public decimal MontoPagado { get; set; }

        public decimal Cambio { get; set; }
    }

    public class VentaListadoDto
    {
        public Guid IdVenta { get; set; }

        public string NumeroVenta { get; set; } = string.Empty;

        public DateTime FechaVenta { get; set; }

        public Guid? Id_Persona { get; set; }

        public string Cliente { get; set; } = string.Empty;

        public string Identificacion { get; set; } = string.Empty;

        public string Usuario { get; set; } = string.Empty;

        public decimal Total { get; set; }

        public int EstadoVenta { get; set; }
    }

    public class VentaCompletaDto
    {
        public Guid IdVenta { get; set; }

        public string NumeroVenta { get; set; } = string.Empty;

        public DateTime FechaVenta { get; set; }

        public Guid? Id_Persona { get; set; }

        public string Cliente { get; set; } = string.Empty;

        public string Identificacion { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public decimal Subtotal { get; set; }

        public decimal DescuentoTotal { get; set; }

        public decimal ImpuestoTotal { get; set; }

        public decimal Total { get; set; }

        public decimal MontoPagado { get; set; }

        public decimal Cambio { get; set; }

        public int EstadoVenta { get; set; }

        public string? Observaciones { get; set; }

        public List<VentaDetalleConsultaDto> Detalles { get; set; }
            = new();

        public List<VentaPagoConsultaDto> Pagos { get; set; }
            = new();
    }

    public class VentaDetalleConsultaDto
    {
        public Guid IdVentaDetalle { get; set; }

        public Guid? Id_Mascota { get; set; }

        public string? Mascota { get; set; }

        public int TipoDetalle { get; set; }

        public Guid? Id_Producto { get; set; }

        public Guid? Id_Servicio { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public decimal Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Descuento { get; set; }

        public decimal ValorImpuesto { get; set; }

        public decimal TotalLinea { get; set; }
    }

    public class VentaPagoConsultaDto
    {
        public int TipoPago { get; set; }

        public decimal Monto { get; set; }

        public string? Referencia { get; set; }
    }
}