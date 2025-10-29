
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Extensions;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class MascotasService(TuVetContext context) : IControladorMascotas
    {
        private readonly TuVetContext _context = context;

        public async Task<PaginationDto<Mascotas>> AllMascotas(QueryParams qParams)
        {
            try
            {
                PaginationDto<Mascotas> mascotas = await _context.Mascotas
                .OrderBy(c => c.FechaDeNacimiento)
                .ApplySearch(qParams.search, t => t.Nombre)
                .OrderBy(qParams.orderBy ?? nameof(Mascotas.FechaDeNacimiento), qParams.isOrderByDescending)
                .GetPagedAsync(qParams);
                return mascotas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message + " mensaje: " + ex.Message);
            }
        }
        public async Task<string> CreateMascota(Mascotas mascotas)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Mascotas.Add(mascotas);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> DeleteMascota(Guid iD)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Mascotas.Remove(_context.Mascotas.Where(x => x.IdMascota == iD).First());
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> UpdateMascota(Mascotas mascotas)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Mascotas.Update(mascotas);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
    }
}