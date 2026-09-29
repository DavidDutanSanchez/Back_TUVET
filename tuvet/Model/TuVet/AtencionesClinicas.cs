namespace tu_vet_back.tuvet.Model.TuVet
{
    public class AtencionesClinicas
    {
        public Guid IdAtencionClinica { get; set; }

        public Guid Id_HistorialClinico { get; set; }

        public Guid Id_Mascota { get; set; }

        public Guid Id_UsuarioResponsable { get; set; }

        public DateTime FechaAtencion { get; set; }

        public int TipoAtencion { get; set; }

        public string? MotivoConsulta { get; set; }

        public string? Anamnesis { get; set; }

        public string? ExamenFisico { get; set; }

        public decimal? PesoKg { get; set; }

        public decimal? Temperatura { get; set; }

        public int? FrecuenciaCardiaca { get; set; }

        public int? FrecuenciaRespiratoria { get; set; }

        public string? Diagnostico { get; set; }

        public string? Procedimiento { get; set; }

        public string? Tratamiento { get; set; }

        public string? Recomendaciones { get; set; }

        public DateTime? ProximoControl { get; set; }

        public string? Observaciones { get; set; }

        public int Estado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}