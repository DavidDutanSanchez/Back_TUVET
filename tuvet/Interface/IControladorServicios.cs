using tu_vet_back.tuvet.Dtos;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorServicios
    {
        Task<List<ServicioDto>>
            FindAllServicios(
                bool soloActivos = false
            );

        Task<ServicioDto?>
            FindServicioById(
                Guid id
            );

        Task<string>
            CreateServicio(
                GuardarServicioDto dto
            );

        Task<string>
            UpdateServicio(
                GuardarServicioDto dto
            );

        Task<string>
            CambiarEstadoServicio(
                Guid id,
                bool activo
            );
    }
}