using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorColores
    {
        //CRUD colores
        public Task<string> CreateColor(Colores colores);
        public Task<PaginationDto<Colores>> AllColores(QueryParams qParams);
        public Task<string> UpdateColor(Colores colores);
        public Task<string> DeleteColor(Guid iD);
    }
}