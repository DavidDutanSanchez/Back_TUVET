
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Extensions;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class RazaService(TuVetContext context) : IControladorRazas
    {
        private readonly TuVetContext _context = context;

        public async Task<PaginationDto<Razas>> AllRazas(QueryParams qParams)
        {
            try
            {
                PaginationDto<Razas> razas = await _context.Razas
                .OrderBy(c => c.NombreRaza)
                .ApplySearch(qParams.search, t => t.NombreRaza)
                .OrderBy(qParams.orderBy ?? nameof(Razas.NombreRaza), qParams.isOrderByDescending)
                .GetPagedAsync(qParams);
                return razas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message + " mensaje: " + ex.Message);
            }
        }
        public async Task<string> CreateRaza(Razas razas)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Razas.Add(razas);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> DeleteRaza(Guid iD)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Razas.Remove(_context.Razas.Where(x => x.IdRaza == iD).First());
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> UpdateRaza(Razas razas)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Razas.Update(razas);
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