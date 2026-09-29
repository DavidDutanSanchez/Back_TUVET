namespace tu_vet_back.tuvet.Model.TuVet
{
    public class HospitalizacionTratamientos
    {
        public Guid IdTratamiento { get; set; }

        public Guid Id_Hospitalizacion { get; set; }

        public Guid Id_UsuarioPrescriptor { get; set; }

        public Guid? Id_Producto { get; set; }

        public string NombreMedicamento { get; set; } =
            string.Empty;

        public decimal? Dosis { get; set; }

        public string? UnidadDosis { get; set; }

        public string? ViaAdministracion { get; set; }

        public decimal? FrecuenciaHoras { get; set; }

        public string? IndicacionFrecuencia { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public string? Indicaciones { get; set; }

        public string? Observaciones { get; set; }

        public int EstadoTratamiento { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}