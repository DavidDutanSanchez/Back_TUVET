namespace tu_vet_back.tuvet.Model.TuVet
{
    public partial class Servicios
    {
        public Guid IdServicios { get; set; } = Guid.NewGuid();

        public string NombreServicio { get; set; } = string.Empty;

        public string? DescripcionServicio { get; set; }

        public decimal PreciosServicio { get; set; }

        public bool IncluyeIva { get; set; }

        public int DescuentoServicio { get; set; }

        public int DuracionMinutos { get; set; } = 30;

        // 0 = Fijo
        // 1 = Por peso
        // 2 = Por tamaño
        // 3 = Por valoración
        public int TipoPrecio { get; set; }

        public bool Activo { get; set; } = true;

        public virtual List<ServicioTarifas>
            Tarifas { get; set; } = new();
    }
}