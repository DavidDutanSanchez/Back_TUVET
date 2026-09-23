using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Model.Parameters;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorUsuario
    {
        // =====================================================
        // CRUD USUARIOS
        // =====================================================

        Task<PaginationDto<UsuarioListadoDto>> AllUsuarios(
            QueryParams qParams
        );

        Task<string> CreateUsuario(
            UsuarioCreateDto usuario
        );

        Task<string> UpdateUsuario(
            UsuarioUpdateDto usuario
        );

        Task<string> DeleteUsuario(
            Guid id
        );

        // =====================================================
        // LOGIN
        // =====================================================

        Task<UsuarioDto?> IniciarSession(
            string usuario,
            byte[] contrasenia
        );
    }
}