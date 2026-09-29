namespace tu_vet_back.tuvet.Model.TuVet
{
    public class Hospitalizaciones
    {
        public Guid IdHospitalizacion { get; set; }

        public Guid Id_HistorialClinico { get; set; }

        public Guid Id_Mascota { get; set; }

        public Guid Id_UsuarioResponsable { get; set; }

        public Guid? Id_AtencionClinica { get; set; }

        public DateTime FechaIngreso { get; set; }

        public decimal? PesoIngresoKg { get; set; }

        public string? EdadAlIngreso { get; set; }

        public string? MotivoIngreso { get; set; }

        public string? DiagnosticoIngreso { get; set; }

        public string? Procedimiento { get; set; }

        public string? PlanTerapeutico { get; set; }

        public string? ObservacionesIngreso { get; set; }

        public int EstadoHospitalizacion { get; set; }

        public DateTime? FechaAlta { get; set; }

        public Guid? Id_UsuarioAlta { get; set; }

        public string? DiagnosticoAlta { get; set; }

        public string? ResumenEvolucion { get; set; }

        public string? CondicionAlta { get; set; }

        public string? TratamientoDomiciliario { get; set; }

        public string? CuidadosDomiciliarios { get; set; }

        public string? RecomendacionesAlta { get; set; }

        public DateTime? FechaProximoControl { get; set; }

        public string? ObservacionesAlta { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}