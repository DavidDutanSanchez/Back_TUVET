using Microsoft.EntityFrameworkCore;
using System.Text;
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Extensions;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class UsuariosService(TuVetContext context)
        : IControladorUsuario
    {
        private readonly TuVetContext _context = context;


        // =====================================================
        // OBTENER TODOS LOS USUARIOS
        // =====================================================

        public async Task<PaginationDto<UsuarioListadoDto>>
            AllUsuarios(QueryParams qParams)
        {
            try
            {
                // Primero aplicamos búsqueda, ordenamiento
                // y paginación sobre la entidad Usuarios.

                var usuariosPaginados =
                    await _context.Usuarios
                        .AsNoTracking()
                        .Include(u => u.Persona)
                        .ApplySearch(
                            qParams.search,
                            u => u.NombreUsuario,
                            u => u.Permisos
                        )
                        .OrderBy(
                            qParams.orderBy
                                ?? nameof(Usuarios.NombreUsuario),
                            qParams.isOrderByDescending
                        )
                        .GetPagedAsync(qParams);


                // Después de paginar hacemos la conversión
                // al DTO que se enviará al frontend.
                //
                // Tu PaginationDto utiliza IQueryable<T>,
                // por eso utilizamos AsEnumerable() y
                // finalmente AsQueryable().

                var datos =
                    usuariosPaginados.data
                        .AsEnumerable()
                        .Select(u =>
                            new UsuarioListadoDto
                            {
                                IdUsuario =
                                    u.IdUsuario,

                                NombreUsuario =
                                    u.NombreUsuario,

                                Permisos =
                                    u.Permisos,

                                Estado =
                                    u.Estado,

                                IdPersona =
                                    u.Id_Persona,

                                FotoPerfil =
                                    u.FotoPerfil,

                                Nombres =
                                    u.Persona != null
                                        ? u.Persona.Nombres
                                        : string.Empty,

                                Apellidos =
                                    u.Persona != null
                                        ? u.Persona.Apellidos
                                        : string.Empty,

                                NumeroIdentificacion =
                                    u.Persona != null
                                        ? u.Persona.NumeroIdentificacion
                                        : string.Empty,

                                Telefono =
                                    u.Persona != null
                                        ? u.Persona.Telefono
                                        : string.Empty,

                                CorreoElectronico =
                                    u.Persona != null
                                        ? u.Persona.CorreoElectronico
                                        : string.Empty
                            }
                        )
                        .AsQueryable();


                return new PaginationDto<UsuarioListadoDto>
                {
                    currentPage =
                        usuariosPaginados.currentPage,

                    pageSize =
                        usuariosPaginados.pageSize,

                    totalPages =
                        usuariosPaginados.totalPages,

                    total =
                        usuariosPaginados.total,

                    data =
                        datos
                };
            }
            catch (Exception ex)
            {
                throw new Exception(
                    ex.InnerException?.Message
                    ?? ex.Message
                );
            }
        }


        // =====================================================
        // CREAR USUARIO
        // =====================================================

        public async Task<string> CreateUsuario(
            UsuarioCreateDto datos
        )
        {
            try
            {
                // -------------------------------------------------
                // VALIDAR NOMBRE DE USUARIO
                // -------------------------------------------------

                if (string.IsNullOrWhiteSpace(
                    datos.NombreUsuario))
                {
                    return
                        "El nombre de usuario es obligatorio.";
                }


                // -------------------------------------------------
                // VALIDAR CONTRASEÑA
                // -------------------------------------------------

                if (string.IsNullOrWhiteSpace(
                    datos.Contrasenia))
                {
                    return
                        "La contraseña es obligatoria.";
                }


                // -------------------------------------------------
                // VALIDAR PERSONA
                // -------------------------------------------------

                if (datos.IdPersona == Guid.Empty)
                {
                    return
                        "Debe seleccionar una persona.";
                }


                var personaExiste =
                    await _context.Personas
                        .AnyAsync(
                            p =>
                                p.IdPersona ==
                                datos.IdPersona
                        );


                if (!personaExiste)
                {
                    return
                        "La persona seleccionada no existe.";
                }


                // -------------------------------------------------
                // NORMALIZAR NOMBRE DE USUARIO
                // -------------------------------------------------

                var nombreUsuario =
                    datos.NombreUsuario.Trim();


                // -------------------------------------------------
                // VALIDAR USUARIO DUPLICADO
                // -------------------------------------------------

                var usuarioDuplicado =
                    await _context.Usuarios
                        .AnyAsync(
                            u =>
                                u.NombreUsuario
                                    .ToLower() ==
                                nombreUsuario
                                    .ToLower()
                        );


                if (usuarioDuplicado)
                {
                    return
                        "El nombre de usuario ya existe.";
                }


                // -------------------------------------------------
                // VALIDAR QUE LA PERSONA NO TENGA OTRO USUARIO
                // -------------------------------------------------

                var personaConUsuario =
                    await _context.Usuarios
                        .AnyAsync(
                            u =>
                                u.Id_Persona ==
                                datos.IdPersona
                        );


                if (personaConUsuario)
                {
                    return
                        "La persona seleccionada ya tiene un usuario.";
                }


                // -------------------------------------------------
                // VALIDAR ROL
                // -------------------------------------------------

                var rol =
                    NormalizarRol(
                        datos.Permisos
                    );


                if (rol == null)
                {
                    return
                        "El permiso debe ser ADMINISTRADOR, VETERINARIO o RECEPCION.";
                }


                // -------------------------------------------------
                // CREAR USUARIO
                // -------------------------------------------------

                var usuario =
                    new Usuarios
                    {
                        IdUsuario =
                            Guid.NewGuid(),

                        NombreUsuario =
                            nombreUsuario,

                        // IMPORTANTE:
                        // Conservamos por ahora el formato
                        // que ya utiliza tu sistema para no
                        // romper el login existente.
                        Contrasenia =
                            Encoding.UTF8.GetBytes(
                                datos.Contrasenia
                            ),

                        Permisos =
                            rol,

                        Estado =
                            datos.Estado,

                        Id_Persona =
                            datos.IdPersona,

                        FotoPerfil =
                            LimpiarFoto(
                                datos.FotoPerfil
                            )
                    };


                _context.Usuarios.Add(
                    usuario
                );


                await _context.SaveChangesAsync();


                return "Realizado";
            }
            catch (Exception ex)
            {
                return
                    ex.InnerException?.Message
                    ?? ex.Message;
            }
        }


        // =====================================================
        // ACTUALIZAR USUARIO
        // =====================================================

        public async Task<string> UpdateUsuario(
            UsuarioUpdateDto datos
        )
        {
            try
            {
                // -------------------------------------------------
                // BUSCAR USUARIO
                // -------------------------------------------------

                var usuario =
                    await _context.Usuarios
                        .FirstOrDefaultAsync(
                            u =>
                                u.IdUsuario ==
                                datos.IdUsuario
                        );


                if (usuario == null)
                {
                    return
                        "Usuario no encontrado.";
                }


                // -------------------------------------------------
                // VALIDAR NOMBRE
                // -------------------------------------------------

                if (string.IsNullOrWhiteSpace(
                    datos.NombreUsuario))
                {
                    return
                        "El nombre de usuario es obligatorio.";
                }


                // -------------------------------------------------
                // VALIDAR PERSONA
                // -------------------------------------------------

                if (datos.IdPersona == Guid.Empty)
                {
                    return
                        "Debe seleccionar una persona.";
                }


                var personaExiste =
                    await _context.Personas
                        .AnyAsync(
                            p =>
                                p.IdPersona ==
                                datos.IdPersona
                        );


                if (!personaExiste)
                {
                    return
                        "La persona seleccionada no existe.";
                }


                var nombreUsuario =
                    datos.NombreUsuario.Trim();


                // -------------------------------------------------
                // VALIDAR NOMBRE DUPLICADO
                // -------------------------------------------------

                var nombreDuplicado =
                    await _context.Usuarios
                        .AnyAsync(
                            u =>
                                u.IdUsuario !=
                                    datos.IdUsuario
                                &&
                                u.NombreUsuario
                                    .ToLower() ==
                                nombreUsuario
                                    .ToLower()
                        );


                if (nombreDuplicado)
                {
                    return
                        "El nombre de usuario ya existe.";
                }


                // -------------------------------------------------
                // VALIDAR PERSONA DUPLICADA
                // -------------------------------------------------

                var personaDuplicada =
                    await _context.Usuarios
                        .AnyAsync(
                            u =>
                                u.IdUsuario !=
                                    datos.IdUsuario
                                &&
                                u.Id_Persona ==
                                    datos.IdPersona
                        );


                if (personaDuplicada)
                {
                    return
                        "La persona seleccionada ya tiene otro usuario.";
                }


                // -------------------------------------------------
                // VALIDAR ROL
                // -------------------------------------------------

                var rol =
                    NormalizarRol(
                        datos.Permisos
                    );


                if (rol == null)
                {
                    return
                        "El permiso debe ser ADMINISTRADOR, VETERINARIO o RECEPCION.";
                }


                // -------------------------------------------------
                // ACTUALIZAR
                // -------------------------------------------------

                usuario.NombreUsuario =
                    nombreUsuario;

                usuario.Permisos =
                    rol;

                usuario.Estado =
                    datos.Estado;

                usuario.Id_Persona =
                    datos.IdPersona;

                usuario.FotoPerfil =
                    LimpiarFoto(
                        datos.FotoPerfil
                    );


                // -------------------------------------------------
                // CAMBIAR CONTRASEÑA SOLO SI ESCRIBIERON UNA
                // -------------------------------------------------

                if (!string.IsNullOrWhiteSpace(
                    datos.Contrasenia))
                {
                    usuario.Contrasenia =
                        Encoding.UTF8.GetBytes(
                            datos.Contrasenia
                        );
                }


                await _context.SaveChangesAsync();


                return "Realizado";
            }
            catch (Exception ex)
            {
                return
                    ex.InnerException?.Message
                    ?? ex.Message;
            }
        }


        // =====================================================
        // DESACTIVAR USUARIO
        // =====================================================

        public async Task<string> DeleteUsuario(
            Guid id
        )
        {
            try
            {
                var usuario =
                    await _context.Usuarios
                        .FirstOrDefaultAsync(
                            u =>
                                u.IdUsuario ==
                                id
                        );


                if (usuario == null)
                {
                    return
                        "Usuario no encontrado.";
                }


                // No eliminamos físicamente.
                // Lo dejamos inactivo.
                usuario.Estado = false;


                await _context.SaveChangesAsync();


                return "Realizado";
            }
            catch (Exception ex)
            {
                return
                    ex.InnerException?.Message
                    ?? ex.Message;
            }
        }


        // =====================================================
        // INICIAR SESIÓN
        // =====================================================

        public async Task<UsuarioDto?> IniciarSession(
            string usuario,
            byte[] contrasenia
        )
        {
            try
            {
                if (string.IsNullOrWhiteSpace(
                    usuario))
                {
                    return null;
                }


                var nombreUsuario =
                    usuario.Trim();


                var user =
                    await _context.Usuarios
                        .AsNoTracking()
                        .Include(u => u.Persona)
                        .FirstOrDefaultAsync(
                            u =>
                                u.NombreUsuario ==
                                    nombreUsuario
                                &&
                                u.Estado
                        );


                if (user == null)
                {
                    return null;
                }


                // -------------------------------------------------
                // COMPARAR CONTRASEÑA
                // -------------------------------------------------

                var stored =
                    Encoding.UTF8
                        .GetString(
                            user.Contrasenia
                        )
                        .Trim();


                var contraseniaIngresada =
                    Encoding.UTF8
                        .GetString(
                            contrasenia
                        )
                        .Trim();


                if (!string.Equals(
                    stored,
                    contraseniaIngresada,
                    StringComparison.Ordinal
                ))
                {
                    return null;
                }


                // -------------------------------------------------
                // DEVOLVER USUARIO SIN CONTRASEÑA
                // -------------------------------------------------

                return new UsuarioDto
                {
                    IdUsuario =
                        user.IdUsuario,

                    NombreUsuario =
                        user.NombreUsuario,

                    Permisos =
                        user.Permisos,

                    Estado =
                        user.Estado,

                    Id_Persona =
                        user.Id_Persona,

                    FotoPerfil =
                        user.FotoPerfil,

                    Nombres =
                        user.Persona != null
                            ? user.Persona.Nombres
                            : string.Empty,

                    Apellidos =
                        user.Persona != null
                            ? user.Persona.Apellidos
                            : string.Empty,

                    NumeroIdentificacion =
                        user.Persona != null
                            ? user.Persona.NumeroIdentificacion
                            : string.Empty,

                    CorreoElectronico =
                        user.Persona != null
                            ? user.Persona.CorreoElectronico
                            : string.Empty,

                    Telefono =
                        user.Persona != null
                            ? user.Persona.Telefono
                            : string.Empty
                };
            }
            catch (Exception ex)
            {
                throw new Exception(
                    ex.InnerException?.Message
                    ?? ex.Message
                );
            }
        }


        // =====================================================
        // NORMALIZAR ROL
        // =====================================================

        private static string? NormalizarRol(
            string? rol
        )
        {
            if (string.IsNullOrWhiteSpace(
                rol))
            {
                return null;
            }


            var valor =
                rol.Trim()
                    .ToUpperInvariant();


            return valor switch
            {
                "ADMINISTRADOR" =>
                    "ADMINISTRADOR",

                "VETERINARIO" =>
                    "VETERINARIO",

                "RECEPCION" =>
                    "RECEPCION",

                _ =>
                    null
            };
        }


        // =====================================================
        // FOTO BASE64
        // =====================================================

        private static string? LimpiarFoto(
            string? foto
        )
        {
            if (string.IsNullOrWhiteSpace(
                foto))
            {
                return null;
            }


            return foto.Trim();
        }
    }
}