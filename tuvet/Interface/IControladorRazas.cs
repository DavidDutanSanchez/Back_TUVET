using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorRazas
    {
        //CRUD razas
        public Task<string> CreateRaza(Razas razas);
        public Task<PaginationDto<Razas>> AllRazas(QueryParams qParams);
        public Task<string> UpdateRaza(Razas razas);
        public Task<string> DeleteRaza(Guid iD);
    }
}