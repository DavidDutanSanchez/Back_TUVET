using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorPersonas
    {
        //CRUD personas
        public Task<string> CreatePersona(Personas personas);
        public Task<PaginationDto<Personas>> AllPersonas(QueryParams qParams);
        public Task<string> UpdatePersona(Personas personas);
        public Task<string> DeletePersona(Guid iD);
    }
}