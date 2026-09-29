using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorHistorialClinico
    {
        Task<HistorialClinicoDto?> ObtenerPorMascota(
            Guid idMascota
        );

        Task<Guid> CrearHistorial(
            HistorialClinicoCreateDto dto
        );

        Task<string> ActualizarHistorial(
            HistorialClinicoUpdateDto dto
        );

        Task<Guid> CrearAtencion(
            AtencionClinicaCreateDto dto
        );

        Task<List<AtencionesClinicas>> ObtenerAtenciones(
            Guid idMascota
        );
    }
}