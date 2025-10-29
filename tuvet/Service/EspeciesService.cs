
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Extensions;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class EspeciesService(TuVetContext context) : IControladorEspecies
    {
        private readonly TuVetContext _context = context;

        public async Task<PaginationDto<Especies>> AllEspecies(QueryParams qParams)
        {
            try
            {
                PaginationDto<Especies> especie = await _context.Especies
                .OrderBy(c => c.NombreEspecie)
                .ApplySearch(qParams.search, t => t.NombreEspecie)
                .OrderBy(qParams.orderBy ?? nameof(Especies.NombreEspecie), qParams.isOrderByDescending)
                .GetPagedAsync(qParams);
                return especie;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message + " mensaje: " + ex.Message);
            }
        }
        public async Task<string> CreateEspecie(Especies especies)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Especies.Add(especies);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        } 
        public async Task<string> DeleteEspecie(Guid iD)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Especies.Remove(_context.Especies.Where(x => x.IdEspecies == iD).First());
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> UpdateEspecie(Especies especies)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Especies.Update(especies);
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