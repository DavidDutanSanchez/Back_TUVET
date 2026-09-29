namespace tu_vet_back.tuvet.Model.TuVet
{
    public class HistorialesClinicos
    {
        public Guid IdHistorialClinico { get; set; }

        public Guid Id_Mascota { get; set; }

        public DateTime FechaApertura { get; set; }

        public string? AntecedentesClinicos { get; set; }

        public string? AlergiasConocidas { get; set; }

        public string? EnfermedadesCronicas { get; set; }

        public string? ObservacionesGenerales { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}