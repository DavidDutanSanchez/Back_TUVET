namespace tu_vet_back.tuvet.Model.TuVet
{
    public class VentaPago
    {
        public Guid IdVentaPago { get; set; }

        public Guid Id_Venta { get; set; }

        // 1 = Efectivo
        // 2 = Tarjeta
        // 3 = Transferencia
        // 4 = Otro
        public int TipoPago { get; set; }

        public decimal Monto { get; set; }

        public string? Referencia { get; set; }

        public string? Observaciones { get; set; }

        public Guid Id_Usuario { get; set; }

        public DateTime FechaPago { get; set; }
    }
}