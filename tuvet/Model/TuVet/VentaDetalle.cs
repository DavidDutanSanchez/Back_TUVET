namespace tu_vet_back.tuvet.Model.TuVet
{
    public class VentaDetalle
    {
        public Guid IdVentaDetalle { get; set; }

        public Guid Id_Venta { get; set; }

        // Puede cambiar entre líneas.
        // Así una venta puede tener varias mascotas.
        public Guid? Id_Mascota { get; set; }

        // 1 = Producto
        // 2 = Servicio
        // 3 = Varios
        public int TipoDetalle { get; set; }

        public Guid? Id_Producto { get; set; }

        public Guid? Id_Servicio { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public decimal Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Descuento { get; set; }

        public decimal PorcentajeImpuesto { get; set; }

        public decimal BaseImponible { get; set; }

        public decimal ValorImpuesto { get; set; }

        public decimal TotalLinea { get; set; }

        public string? Observaciones { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}