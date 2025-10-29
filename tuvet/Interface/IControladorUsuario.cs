using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorUsuario
    {
        //CRUD Usuarios
        public Task<string> CreateUsuario(Usuarios usuarios);
        public Task<PaginationDto<Usuarios>> AllUsuarios(QueryParams qParams);
        public Task<string> UpdateUsuario(Usuarios usuarios);
        public Task<string> DeleteUsuario(Guid iD);
        public Task<UsuarioDto?> IniciarSession(string usuario, byte[] contrasenia);
    }
}