using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorMascotas
    {
        //CRUD Mascotas
        public Task<string> CreateMascota(Mascotas mascotas);
        public Task<PaginationDto<Mascotas>> AllMascotas(QueryParams qParams);
        public Task<string> UpdateMascota(Mascotas mascotas);
        public Task<string> DeleteMascota(Guid iD);
    }
}