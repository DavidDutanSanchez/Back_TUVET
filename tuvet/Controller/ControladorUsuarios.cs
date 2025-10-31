using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorUsuarios(IControladorUsuario usuarioService) : SistecControllerBase
    {
        private readonly IControladorUsuario _usuariosService = usuarioService;

        //CRUD Usuarios
        [HttpGet("FindAllUsuarios")]
        public async Task<ActionResult<PaginationDto<Servicios>>> FindAllUsuarios([FromQuery] QueryParams qParams)
        {
            PaginationDto<Usuarios> pagedResult = await _usuariosService.AllUsuarios(qParams);
            return Ok(pagedResult);
        }
        [HttpPut("AddUsuario")]
        public async Task<IActionResult> AddUsuario([FromBody] Usuarios usuarios)
        {
            string response = await _usuariosService.CreateUsuario(usuarios);
            return response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response);
        }
        [HttpPost("UpdateUsuarios")]
        public async Task<ActionResult> UpdateUsuarios([FromBody] Usuarios usuarios)
        {
            string response = await _usuariosService.UpdateUsuario(usuarios);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }

        [HttpDelete("DeleteUsuarios/{id}")]
        public async Task<ActionResult> DeleteUsuarios([FromRoute] Guid id)
        {
            string response = await _usuariosService.DeleteUsuario(id);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }
        [HttpPost("IniciarSession")]
        public async Task<ActionResult> IniciarSession([FromQuery] byte[] clave, [FromQuery] string usuario)
        {
            UsuarioDto? response = await _usuariosService.IniciarSession(usuario, clave);
            return (ActionResult)(response == null ? Ok(response) : (IActionResult)InternalServerError("Error, usuario o contraseña equivocado"));
        }

    }
}