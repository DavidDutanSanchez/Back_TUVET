
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Extensions;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class PersonasService(TuVetContext context) : IControladorPersonas
    {
        private readonly TuVetContext _context = context;

        public async Task<PaginationDto<Personas>> AllPersonas(QueryParams qParams)
        {
            try
            {
                PaginationDto<Personas> personas = await _context.Personas
                .OrderBy(c => c.Apellidos)
                .ApplySearch(qParams.search, t => t.Nombres, t => t.Apellidos, t => t.NumeroIdentificacion)
                .OrderBy(qParams.orderBy ?? nameof(Personas.Apellidos), qParams.isOrderByDescending)
                .GetPagedAsync(qParams);
                return personas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message + " mensaje: " + ex.Message);
            }
        }
        public async Task<string> CreatePersona(Personas personas)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Personas.Add(personas);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> DeletePersona(Guid iD)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Personas.Remove(_context.Personas.Where(x => x.IdPersona == iD).First());
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> UpdatePersona(Personas personas)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Personas.Update(personas);
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