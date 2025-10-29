using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorCategorias
    {
        //CRUD categorias
        public Task<string> CreateCategorias(Categorias categorias);
        public Task<PaginationDto<Categorias>> AllCategorias(QueryParams qParams);
        public Task<string> UpdateCategorias(Categorias categorias);
        public Task<string> DeleteCategorias(Guid iD);
    }
}