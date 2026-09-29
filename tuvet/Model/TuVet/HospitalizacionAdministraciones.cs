namespace tu_vet_back.tuvet.Model.TuVet
{
    public class HospitalizacionAdministraciones
    {
        public Guid IdAdministracion { get; set; }

        public Guid Id_Tratamiento { get; set; }

        public Guid Id_Hospitalizacion { get; set; }

        public Guid Id_UsuarioResponsable { get; set; }

        public DateTime? FechaProgramada { get; set; }

        public DateTime? FechaAdministracion { get; set; }

        public decimal? DosisAdministrada { get; set; }

        public string? UnidadDosis { get; set; }

        public string? ViaAdministracion { get; set; }

        public int EstadoAdministracion { get; set; }

        public string? MotivoOmision { get; set; }

        public string? Observaciones { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
