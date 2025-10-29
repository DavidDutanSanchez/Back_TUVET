
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Extensions;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;


namespace tu_vet_back.tuvet.Service
{
    public class CategoriasService(TuVetContext context) : IControladorCategorias
    {
        private readonly TuVetContext _context = context;

        public async Task<PaginationDto<Categorias>> AllCategorias(QueryParams qParams)
        {
            try
            {
                PaginationDto<Categorias> categorias = await _context.Categorias
                .OrderBy(c => c.NombreCategoria)
                .ApplySearch(qParams.search, t => t.NombreCategoria)
                .OrderBy(qParams.orderBy ?? nameof(Categorias.NombreCategoria), qParams.isOrderByDescending)
                .GetPagedAsync(qParams);
                return categorias;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message + " mensaje: " + ex.Message);
            }
        }

        public async Task<string> CreateCategorias(Categorias categorias)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Categorias.Add(categorias);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }

        public async Task<string> DeleteCategorias(Guid iD)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Categorias.Remove(_context.Categorias.Where(x => x.IdCategoria == iD).First());
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }

        public async Task<string> UpdateCategorias(Categorias categorias)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Categorias.Update(categorias);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        /*
public async Task<Usuarios?> LoginUsuarioAsync(UsuarioLoginDto usuarioLoginDto)
{
   try
   {
       var user = await _context.Usuarios
           .Where(u => u.Usuario == usuarioLoginDto.Usuario)
           .Select(u => new
           {
               u.IdUsuarios,
               u.Usuario,
               u.ContraseniaUsuarios,
               u.permisosUsuarios,
               u._persona_id
           })
           .SingleOrDefaultAsync();
       if (user == null)
       {
           return null;
       }

       var stored = System.Text.Encoding.UTF8.GetString(user.ContraseniaUsuarios).Trim();
       if (!string.Equals(stored, usuarioLoginDto.Contrasenia.Trim(), StringComparison.Ordinal))
           return null;

       return new Usuarios
       {
           IdUsuarios = user.IdUsuarios,
           Usuario = user.Usuario,
           ContraseniaUsuarios = Array.Empty<byte>(),
           permisosUsuarios = user.permisosUsuarios,
           _persona_id = user._persona_id
       };
   }
   catch (Exception ex)
   {
       throw new Exception(ex.InnerException?.Message + " mensaje: " + ex.Message);
   }
}
*/
    }
}