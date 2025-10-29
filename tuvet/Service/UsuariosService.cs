
using System.Data.Entity;
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Extensions;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class UsuariosService(TuVetContext context) : IControladorUsuario
    {
        private readonly TuVetContext _context = context;

        public async Task<PaginationDto<Usuarios>> AllUsuarios(QueryParams qParams)
        {
            try
            {
                PaginationDto<Usuarios> usuarios = await _context.Usuarios
                .OrderBy(c => c.NombreUsuario)
                .ApplySearch(qParams.search, t => t.NombreUsuario)
                .OrderBy(qParams.orderBy ?? nameof(Usuarios.NombreUsuario), qParams.isOrderByDescending)
                .GetPagedAsync(qParams);
                return usuarios;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message + " mensaje: " + ex.Message);
            }
        }
        public async Task<string> CreateUsuario(Usuarios usuarios)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Usuarios.Add(usuarios);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> DeleteUsuario(Guid iD)
        {
            string response = "Realizado";
            try
            {
                Usuarios? usuario = _context.Usuarios.Where(x => x.IdUsuario == iD).FirstOrDefault();
                if (usuario == null)
                {
                    return "Usuarios no encontrado";
                }

                usuario.Estado = false;
                _ = _context.Usuarios.Update(usuario);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }

        public async Task<UsuarioDto?> IniciarSession(string usuario, byte[] contrasenia)
        {
            try
            {
                Usuarios? user = await _context.Usuarios
                    .Where(u => u.NombreUsuario == usuario)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return null;
                }

                var stored = System.Text.Encoding.UTF8.GetString(user.Contrasenia).Trim();
                var contraseniaStr = System.Text.Encoding.UTF8.GetString(contrasenia).Trim();
                if (!string.Equals(stored, contraseniaStr, StringComparison.Ordinal))
                {
                    return null;
                }

                return new UsuarioDto
                {
                    Permisos = user.Permisos,
                    UserName = user.NombreUsuario,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message + " mensaje: " + ex.Message);
            }
        }
        public async Task<string> UpdateUsuario(Usuarios usuarios)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Usuarios.Update(usuarios);
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