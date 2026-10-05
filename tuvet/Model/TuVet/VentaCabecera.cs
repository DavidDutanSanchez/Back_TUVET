namespace tu_vet_back.tuvet.Model.TuVet
{
    public class VentaCabecera
    {
        public Guid IdVenta { get; set; }

        public string NumeroVenta { get; set; } = string.Empty;

        public DateTime FechaVenta { get; set; }

        // NULL = Consumidor final
        public Guid? Id_Persona { get; set; }

        public Guid Id_Usuario { get; set; }

        public decimal SubtotalSinImpuestos { get; set; }

        public decimal SubtotalConImpuestos { get; set; }

        public decimal DescuentoTotal { get; set; }

        public decimal ImpuestoTotal { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }

        public decimal MontoPagado { get; set; }

        public decimal Cambio { get; set; }

        // 0 = Borrador
        // 1 = Confirmada
        // 2 = Anulada
        public int EstadoVenta { get; set; }

        public string? Observaciones { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public Guid? Id_UsuarioAnulacion { get; set; }

        public DateTime? FechaAnulacion { get; set; }

        public string? MotivoAnulacion { get; set; }
    }
}