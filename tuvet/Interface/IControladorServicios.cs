using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorServicios
    {
        //CRUD Servicios
        public Task<string> CreateServicio(Servicios servicios);
        public Task<PaginationDto<Servicios>> AllServicios(QueryParams qParams);
        public Task<string> UpdateServicio(Servicios servicios);
        public Task<string> DeleteServicio(Guid iD);
    }
}