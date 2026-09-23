using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Interface;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorServicios(
        IControladorServicios serviciosService)
        : SistecControllerBase
    {
        private readonly IControladorServicios
            _serviciosService =
                serviciosService;


        [HttpGet("FindAllServicios")]
        public async Task<ActionResult<List<ServicioDto>>>
            FindAllServicios(
                [FromQuery]
                bool soloActivos = false)
        {
            List<ServicioDto> resultado =
                await _serviciosService
                    .FindAllServicios(
                        soloActivos
                    );

            return Ok(resultado);
        }


        [HttpGet("FindServicioById/{id}")]
        public async Task<IActionResult>
            FindServicioById(
                [FromRoute] Guid id)
        {
            ServicioDto? resultado =
                await _serviciosService
                    .FindServicioById(id);


            if (resultado == null)
            {
                return NotFound(
                    new
                    {
                        success = false,
                        message =
                            "El servicio no existe."
                    }
                );
            }


            return Ok(resultado);
        }


        [HttpPut("AddServicio")]
        public async Task<IActionResult>
            AddServicio(
                [FromBody]
                GuardarServicioDto dto)
        {
            return Resultado(
                await _serviciosService
                    .CreateServicio(dto)
            );
        }


        [HttpPost("UpdateServicio")]
        public async Task<IActionResult>
            UpdateServicio(
                [FromBody]
                GuardarServicioDto dto)
        {
            return Resultado(
                await _serviciosService
                    .UpdateServicio(dto)
            );
        }


        [HttpPost("CambiarEstado/{id}")]
        public async Task<IActionResult>
            CambiarEstado(
                [FromRoute] Guid id,
                [FromQuery] bool activo)
        {
            return Resultado(
                await _serviciosService
                    .CambiarEstadoServicio(
                        id,
                        activo
                    )
            );
        }


        private IActionResult Resultado(
            string response)
        {
            if (
                response ==
                "Realizado"
            )
            {
                return Ok(
                    new
                    {
                        success = true,
                        message = response
                    }
                );
            }


            return BadRequest(
                new
                {
                    success = false,
                    message = response
                }
            );
        }
    }
}