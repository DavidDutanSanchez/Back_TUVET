namespace tu_vet_back.tuvet.Model.TuVet
{
    public class HospitalizacionMonitoreos
    {
        public Guid IdMonitoreo { get; set; }

        public Guid Id_Hospitalizacion { get; set; }

        public Guid Id_UsuarioResponsable { get; set; }

        public DateTime FechaMonitoreo { get; set; }

        public decimal? Temperatura { get; set; }

        public int? FrecuenciaCardiaca { get; set; }

        public int? FrecuenciaRespiratoria { get; set; }

        public decimal? PesoKg { get; set; }

        public int? PresionArterialSistolica { get; set; }

        public int? PresionArterialDiastolica { get; set; }

        public decimal? SaturacionOxigeno { get; set; }

        public string? EstadoHidratacion { get; set; }

        public string? ColorMucosas { get; set; }

        public decimal? TiempoLlenadoCapilar { get; set; }

        public int? NivelDolor { get; set; }

        public string? Apetito { get; set; }

        public string? ConsumoAgua { get; set; }

        public string? Vomitos { get; set; }

        public string? Miccion { get; set; }

        public string? Defecacion { get; set; }

        public string? Comportamiento { get; set; }

        public string? EvolucionClinica { get; set; }

        public string? Observaciones { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}