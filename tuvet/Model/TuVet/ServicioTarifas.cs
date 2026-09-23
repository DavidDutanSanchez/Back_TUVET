namespace tu_vet_back.tuvet.Model.TuVet
{
    public class ServicioTarifas
    {
        public Guid IdServicioTarifa { get; set; }
            = Guid.NewGuid();

        public Guid Id_Servicio { get; set; }

        public string NombreTarifa { get; set; }
            = string.Empty;

        public decimal? PesoMinimo { get; set; }

        public decimal? PesoMaximo { get; set; }

        // 1 = Pequeño
        // 2 = Mediano
        // 3 = Grande
        // 4 = Gigante
        public int? Tamanio { get; set; }

        public decimal Precio { get; set; }

        public int? DuracionMinutos { get; set; }

        public bool Activo { get; set; } = true;

        public virtual Servicios? Servicio { get; set; }
    }
}