
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Extensions;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class InventariosService(TuVetContext context) : IControladorInventarios
    {
        private readonly TuVetContext _context = context;

        public async Task<PaginationDto<Inventarios>> AllInventarios(QueryParams qParams)
        {
            try
            {
                PaginationDto<Inventarios> inventario = await _context.Inventarios
                .OrderBy(c => c.FechaMovimiento)
                //.ApplySearch(qParams.search, t => t.)
                .OrderBy(qParams.orderBy ?? nameof(Inventarios.FechaMovimiento), qParams.isOrderByDescending)
                .GetPagedAsync(qParams);
                return inventario;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message + " mensaje: " + ex.Message);
            }
        }
        public async Task<string> CreateInventario(Inventarios inventarios)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Inventarios.Add(inventarios);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> DeleteInventario(Guid iD)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Inventarios.Remove(_context.Inventarios.Where(x => x.IdInventario == iD).First());
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> UpdateInventario(Inventarios inventarios)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Inventarios.Update(inventarios);
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