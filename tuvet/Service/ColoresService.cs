
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Extensions;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class ColoresService(TuVetContext context) : IControladorColores
    {
        private readonly TuVetContext _context = context;

        public async Task<PaginationDto<Colores>> AllColores(QueryParams qParams)
        {
            try
            {
                PaginationDto<Colores> colores = await _context.Colores
                .OrderBy(c => c.NombreColor)
                .ApplySearch(qParams.search, t => t.NombreColor)
                .OrderBy(qParams.orderBy ?? nameof(Colores.NombreColor), qParams.isOrderByDescending)
                .GetPagedAsync(qParams);
                return colores;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message + " mensaje: " + ex.Message);
            }
        }

        public async Task<string> CreateColor(Colores colores)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Colores.Add(colores);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }

        public async Task<string> DeleteColor(Guid iD)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Colores.Remove(_context.Colores.Where(x => x.IdColor == iD).First());
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }

        public async Task<string> UpdateColor(Colores colores)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Colores.Update(colores);
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