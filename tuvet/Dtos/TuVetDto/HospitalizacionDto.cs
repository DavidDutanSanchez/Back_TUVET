namespace tu_vet_back.tuvet.Dtos.TuVetDto
{
    public class HospitalizacionCreateDto
    {
        public Guid Id_Mascota { get; set; }

        public Guid Id_UsuarioResponsable { get; set; }

        public Guid? Id_AtencionClinica { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public decimal? PesoIngresoKg { get; set; }

        public string? EdadAlIngreso { get; set; }

        public string? MotivoIngreso { get; set; }

        public string? DiagnosticoIngreso { get; set; }

        public string? Procedimiento { get; set; }

        public string? PlanTerapeutico { get; set; }

        public string? ObservacionesIngreso { get; set; }
    }

    public class HospitalizacionUpdateDto
    {
        public Guid IdHospitalizacion { get; set; }

        public Guid Id_UsuarioResponsable { get; set; }

        public decimal? PesoIngresoKg { get; set; }

        public string? EdadAlIngreso { get; set; }

        public string? MotivoIngreso { get; set; }

        public string? DiagnosticoIngreso { get; set; }

        public string? Procedimiento { get; set; }

        public string? PlanTerapeutico { get; set; }

        public string? ObservacionesIngreso { get; set; }
    }

    public class HospitalizacionListadoDto
    {
        public Guid IdHospitalizacion { get; set; }

        public Guid Id_Mascota { get; set; }

        public string NombreMascota { get; set; } = string.Empty;

        public string NombrePropietario { get; set; } = string.Empty;

        public Guid Id_UsuarioResponsable { get; set; }

        public string VeterinarioResponsable { get; set; } =
            string.Empty;

        public DateTime FechaIngreso { get; set; }

        public decimal? PesoIngresoKg { get; set; }

        public string? DiagnosticoIngreso { get; set; }

        public int EstadoHospitalizacion { get; set; }

        public DateTime? FechaAlta { get; set; }
    }

    public class TratamientoCreateDto
    {
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

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public string? Indicaciones { get; set; }

        public string? Observaciones { get; set; }
    }

    public class AdministracionCreateDto
    {
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
    }

    public class MonitoreoCreateDto
    {
        public Guid Id_Hospitalizacion { get; set; }

        public Guid Id_UsuarioResponsable { get; set; }

        public DateTime? FechaMonitoreo { get; set; }

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
    }

    public class AltaHospitalizacionDto
    {
        public Guid IdHospitalizacion { get; set; }

        public Guid Id_UsuarioAlta { get; set; }

        public DateTime? FechaAlta { get; set; }

        public string? DiagnosticoAlta { get; set; }

        public string? ResumenEvolucion { get; set; }

        public string? CondicionAlta { get; set; }

        public string? TratamientoDomiciliario { get; set; }

        public string? CuidadosDomiciliarios { get; set; }

        public string? RecomendacionesAlta { get; set; }

        public DateTime? FechaProximoControl { get; set; }

        public string? ObservacionesAlta { get; set; }
    }
}