
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Extensions;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class ServiciosService(TuVetContext context) : IControladorServicios
    {
        private readonly TuVetContext _context = context;

        public async Task<PaginationDto<Servicios>> AllServicios(QueryParams qParams)
        {
            try
            {
                PaginationDto<Servicios> servicios = await _context.Servicios
                .OrderBy(c => c.NombreServicio)
                .ApplySearch(qParams.search, t => t.NombreServicio)
                .OrderBy(qParams.orderBy ?? nameof(Servicios.NombreServicio), qParams.isOrderByDescending)
                .GetPagedAsync(qParams);
                return servicios;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message + " mensaje: " + ex.Message);
            }
        }
        public async Task<string> CreateServicio(Servicios servicios)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Servicios.Add(servicios);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> DeleteServicio(Guid iD)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Servicios.Remove(_context.Servicios.Where(x => x.IdServicios == iD).First());
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> UpdateServicio(Servicios servicios)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Servicios.Update(servicios);
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