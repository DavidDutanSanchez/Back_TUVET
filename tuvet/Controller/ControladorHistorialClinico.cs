using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Interface;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorHistorialClinico(
        IControladorHistorialClinico service
    ) : SistecControllerBase
    {
        private readonly IControladorHistorialClinico
            _service = service;

        [HttpGet("FindHistorialByMascota/{idMascota}")]
        public async Task<IActionResult> ObtenerPorMascota(
            Guid idMascota
        )
        {
            var resultado = await _service
                .ObtenerPorMascota(idMascota);

            return resultado == null
                ? NotFound("La mascota no tiene historial clínico.")
                : Ok(resultado);
        }

        [HttpPut("AddHistorialClinico")]
        public async Task<IActionResult> CrearHistorial(
            [FromBody] HistorialClinicoCreateDto dto
        )
        {
            try
            {
                var id = await _service.CrearHistorial(dto);

                return Ok(id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateHistorialClinico")]
        public async Task<IActionResult> ActualizarHistorial(
            [FromBody] HistorialClinicoUpdateDto dto
        )
        {
            var respuesta = await _service
                .ActualizarHistorial(dto);

            return respuesta == "Realizado"
                ? Ok(respuesta)
                : NotFound(respuesta);
        }

        [HttpPut("AddAtencionClinica")]
        public async Task<IActionResult> CrearAtencion(
            [FromBody] AtencionClinicaCreateDto dto
        )
        {
            try
            {
                var id = await _service.CrearAtencion(dto);

                return Ok(id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("FindAtencionesByMascota/{idMascota}")]
        public async Task<IActionResult> ObtenerAtenciones(
            Guid idMascota
        )
        {
            var resultado = await _service
                .ObtenerAtenciones(idMascota);

            return Ok(resultado);
        }
    }
}