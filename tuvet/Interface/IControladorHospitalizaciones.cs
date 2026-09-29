using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorHospitalizaciones
    {
        Task<List<HospitalizacionListadoDto>>
            ObtenerHospitalizaciones();

        Task<Hospitalizaciones?>
            ObtenerPorId(Guid id);

        Task<List<Hospitalizaciones>>
            ObtenerPorMascota(Guid idMascota);

        Task<Guid> CrearHospitalizacion(
            HospitalizacionCreateDto dto
        );

        Task<string> ActualizarHospitalizacion(
            HospitalizacionUpdateDto dto
        );

        Task<Guid> RegistrarTratamiento(
            TratamientoCreateDto dto
        );

        Task<Guid> RegistrarAdministracion(
            AdministracionCreateDto dto
        );

        Task<Guid> RegistrarMonitoreo(
            MonitoreoCreateDto dto
        );

        Task<string> RegistrarAlta(
            AltaHospitalizacionDto dto
        );

        Task<List<HospitalizacionTratamientos>>
            ObtenerTratamientos(Guid idHospitalizacion);

        Task<List<HospitalizacionAdministraciones>>
            ObtenerAdministraciones(Guid idHospitalizacion);

        Task<List<HospitalizacionMonitoreos>>
            ObtenerMonitoreos(Guid idHospitalizacion);
    }
}