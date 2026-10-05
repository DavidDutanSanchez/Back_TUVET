namespace tu_vet_back.tuvet.Model.TuVet
{
    public class MovimientoInventario
    {
        public Guid IdMovimientoInventario { get; set; }

        public Guid Id_Producto { get; set; }

        public Guid? Id_Venta { get; set; }

        public Guid? Id_VentaDetalle { get; set; }

        public DateTime FechaMovimiento { get; set; }

        // 1 = Entrada
        // 2 = Salida por venta
        // 3 = Ajuste positivo
        // 4 = Ajuste negativo
        // 5 = Devolución por anulación
        public int TipoMovimiento { get; set; }

        public decimal Cantidad { get; set; }

        public decimal StockAnterior { get; set; }

        public decimal StockNuevo { get; set; }

        public string? Motivo { get; set; }

        public Guid Id_Usuario { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}