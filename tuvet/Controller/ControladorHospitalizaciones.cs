using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Interface;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorHospitalizaciones(
        IControladorHospitalizaciones service
    ) : SistecControllerBase
    {
        private readonly IControladorHospitalizaciones
            _service = service;

        [HttpGet("FindAllHospitalizaciones")]
        public async Task<IActionResult> Listar()
        {
            return Ok(
                await _service.ObtenerHospitalizaciones()
            );
        }

        [HttpGet("FindHospitalizacionById/{id}")]
        public async Task<IActionResult> ObtenerPorId(
            Guid id
        )
        {
            var resultado = await _service.ObtenerPorId(id);

            return resultado == null
                ? NotFound("Hospitalización no encontrada.")
                : Ok(resultado);
        }

        [HttpGet("FindHospitalizacionesByMascota/{idMascota}")]
        public async Task<IActionResult> ObtenerPorMascota(
            Guid idMascota
        )
        {
            return Ok(
                await _service.ObtenerPorMascota(idMascota)
            );
        }

        [HttpPut("AddHospitalizacion")]
        public async Task<IActionResult> Crear(
            [FromBody] HospitalizacionCreateDto dto
        )
        {
            try
            {
                var id = await _service
                    .CrearHospitalizacion(dto);

                return Ok(id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateHospitalizacion")]
        public async Task<IActionResult> Actualizar(
            [FromBody] HospitalizacionUpdateDto dto
        )
        {
            try
            {
                var respuesta = await _service
                    .ActualizarHospitalizacion(dto);

                return respuesta == "Realizado"
                    ? Ok(respuesta)
                    : BadRequest(respuesta);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("RegistrarTratamiento")]
        public async Task<IActionResult> RegistrarTratamiento(
            [FromBody] TratamientoCreateDto dto
        )
        {
            try
            {
                var id = await _service
                    .RegistrarTratamiento(dto);

                return Ok(id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("RegistrarAdministracion")]
        public async Task<IActionResult> RegistrarAdministracion(
            [FromBody] AdministracionCreateDto dto
        )
        {
            try
            {
                var id = await _service
                    .RegistrarAdministracion(dto);

                return Ok(id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("RegistrarMonitoreo")]
        public async Task<IActionResult> RegistrarMonitoreo(
            [FromBody] MonitoreoCreateDto dto
        )
        {
            try
            {
                var id = await _service
                    .RegistrarMonitoreo(dto);

                return Ok(id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RegistrarAlta")]
        public async Task<IActionResult> RegistrarAlta(
            [FromBody] AltaHospitalizacionDto dto
        )
        {
            try
            {
                var respuesta = await _service
                    .RegistrarAlta(dto);

                return respuesta == "Realizado"
                    ? Ok(respuesta)
                    : BadRequest(respuesta);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("FindTratamientos/{idHospitalizacion}")]
        public async Task<IActionResult> ObtenerTratamientos(
            Guid idHospitalizacion
        )
        {
            return Ok(
                await _service
                    .ObtenerTratamientos(idHospitalizacion)
            );
        }

        [HttpGet("FindAdministraciones/{idHospitalizacion}")]
        public async Task<IActionResult> ObtenerAdministraciones(
            Guid idHospitalizacion
        )
        {
            return Ok(
                await _service
                    .ObtenerAdministraciones(idHospitalizacion)
            );
        }

        [HttpGet("FindMonitoreos/{idHospitalizacion}")]
        public async Task<IActionResult> ObtenerMonitoreos(
            Guid idHospitalizacion
        )
        {
            return Ok(
                await _service
                    .ObtenerMonitoreos(idHospitalizacion)
            );
        }
    }
}