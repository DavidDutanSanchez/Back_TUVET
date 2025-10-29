using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorInventarios
    {
        //CRUD Inventarios
        public Task<string> CreateInventario(Inventarios inventarios);
        public Task<PaginationDto<Inventarios>> AllInventarios(QueryParams qParams);
        public Task<string> UpdateInventario(Inventarios inventarios);
        public Task<string> DeleteInventario(Guid iD);
    }
}