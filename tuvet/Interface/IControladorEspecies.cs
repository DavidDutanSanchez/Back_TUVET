using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorEspecies
    {
        //CRUD Especies
        public Task<string> CreateEspecie(Especies especies);
        public Task<PaginationDto<Especies>> AllEspecies(QueryParams qParams);
        public Task<string> UpdateEspecie(Especies especies);
        public Task<string> DeleteEspecie(Guid iD);
    }
}