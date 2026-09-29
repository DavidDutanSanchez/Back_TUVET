namespace tu_vet_back.tuvet.Dtos.TuVetDto
{
    public class HistorialClinicoDto
    {
        public Guid IdHistorialClinico { get; set; }

        public Guid Id_Mascota { get; set; }

        public string NombreMascota { get; set; } = string.Empty;

        public DateTime FechaApertura { get; set; }

        public string? AntecedentesClinicos { get; set; }

        public string? AlergiasConocidas { get; set; }

        public string? EnfermedadesCronicas { get; set; }

        public string? ObservacionesGenerales { get; set; }

        public bool Activo { get; set; }
    }

    public class HistorialClinicoCreateDto
    {
        public Guid Id_Mascota { get; set; }

        public string? AntecedentesClinicos { get; set; }

        public string? AlergiasConocidas { get; set; }

        public string? EnfermedadesCronicas { get; set; }

        public string? ObservacionesGenerales { get; set; }
    }

    public class HistorialClinicoUpdateDto
    {
        public Guid IdHistorialClinico { get; set; }

        public string? AntecedentesClinicos { get; set; }

        public string? AlergiasConocidas { get; set; }

        public string? EnfermedadesCronicas { get; set; }

        public string? ObservacionesGenerales { get; set; }
    }

    public class AtencionClinicaCreateDto
    {
        public Guid Id_Mascota { get; set; }

        public Guid Id_UsuarioResponsable { get; set; }

        public DateTime? FechaAtencion { get; set; }

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
    }
}