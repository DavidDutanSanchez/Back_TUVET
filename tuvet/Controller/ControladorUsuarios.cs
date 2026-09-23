using Microsoft.AspNetCore.Mvc;
using System.Text;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorUsuarios(
        IControladorUsuario usuarioService
    ) : SistecControllerBase
    {
        private readonly IControladorUsuario
            _usuariosService = usuarioService;


        // =====================================================
        // LISTAR USUARIOS
        // =====================================================

        [HttpGet("FindAllUsuarios")]
        public async Task<
            ActionResult<
                PaginationDto<UsuarioListadoDto>
            >
        > FindAllUsuarios(
            [FromQuery] QueryParams qParams
        )
        {
            var pagedResult =
                await _usuariosService
                    .AllUsuarios(qParams);


            return Ok(pagedResult);
        }


        // =====================================================
        // CREAR USUARIO
        // =====================================================

        [HttpPut("AddUsuario")]
        public async Task<IActionResult>
            AddUsuario(
                [FromBody]
                UsuarioCreateDto usuario
            )
        {
            var response =
                await _usuariosService
                    .CreateUsuario(
                        usuario
                    );


            return response == "Realizado"
                ? Ok(response)
                : BadRequest(response);
        }


        // =====================================================
        // ACTUALIZAR USUARIO
        // =====================================================

        [HttpPost("UpdateUsuarios")]
        public async Task<IActionResult>
            UpdateUsuarios(
                [FromBody]
                UsuarioUpdateDto usuario
            )
        {
            var response =
                await _usuariosService
                    .UpdateUsuario(
                        usuario
                    );


            return response == "Realizado"
                ? Ok(response)
                : BadRequest(response);
        }


        // =====================================================
        // DESACTIVAR USUARIO
        // =====================================================

        [HttpDelete("DeleteUsuarios/{id}")]
        public async Task<IActionResult>
            DeleteUsuarios(
                [FromRoute] Guid id
            )
        {
            var response =
                await _usuariosService
                    .DeleteUsuario(
                        id
                    );


            return response == "Realizado"
                ? Ok(response)
                : BadRequest(response);
        }


        // =====================================================
        // INICIAR SESIÓN
        // =====================================================

        [HttpPost("IniciarSession")]
        public async Task<IActionResult>
            IniciarSession(
                [FromQuery]
                string usuario,

                [FromQuery]
                string clave
            )
        {
            if (
                string.IsNullOrWhiteSpace(
                    usuario
                )
                ||
                string.IsNullOrWhiteSpace(
                    clave
                )
            )
            {
                return BadRequest(
                    "Debe ingresar usuario y contraseña."
                );
            }


            var claveBytes =
                Encoding.UTF8.GetBytes(
                    clave
                );


            var response =
                await _usuariosService
                    .IniciarSession(
                        usuario,
                        claveBytes
                    );


            if (response == null)
            {
                return Unauthorized(
                    "Usuario o contraseña incorrectos."
                );
            }


            return Ok(response);
        }
    }
}