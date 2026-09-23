using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Interface;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorAgendamientos
        : ControllerBase
    {
        private readonly IControladorAgendamientos
            _service;

        public ControladorAgendamientos(
            IControladorAgendamientos service)
        {
            _service = service;
        }

        [HttpGet("AgendaPorFecha")]
        public async Task<IActionResult>
            AgendaPorFecha(
                [FromQuery] DateTime fecha)
        {
            var citas =
                await _service.ObtenerPorFecha(fecha);

            return Ok(new
            {
                success = true,
                message = "OK",
                result = citas
            });
        }

        [HttpGet("AgendaHoy")]
        public async Task<IActionResult> AgendaHoy()
        {
            var citas =
                await _service.ObtenerPorFecha(
                    DateTime.Today);

            return Ok(new
            {
                success = true,
                message = "OK",
                result = citas
            });
        }

        [HttpPut("AddAgendamiento")]
        public async Task<IActionResult>
            AddAgendamiento(
                [FromBody]
                CrearAgendamientoDto datos)
        {
            try
            {
                Guid id =
                    await _service.Crear(datos);

                return Ok(new
                {
                    success = true,
                    message = "Cita registrada correctamente.",
                    result = id
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost("Confirmar/{id}")]
        public async Task<IActionResult>
            Confirmar(Guid id)
        {
            try
            {
                await _service.Confirmar(id);
                return Ok(Respuesta(
                    "Cita confirmada."));
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost("MarcarLlegada/{id}")]
        public async Task<IActionResult>
            MarcarLlegada(Guid id)
        {
            try
            {
                await _service.RegistrarLlegada(id);
                return Ok(Respuesta(
                    "Llegada registrada."));
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        [HttpGet("AgendaPorRango")]
public async Task<IActionResult> AgendaPorRango(
    [FromQuery] DateTime desde,
    [FromQuery] DateTime hasta)
{
    if (hasta.Date < desde.Date)
    {
        return BadRequest(new
        {
            success = false,
            message =
                "La fecha final no puede ser anterior a la inicial."
        });
    }

    if ((hasta.Date - desde.Date).TotalDays > 366)
    {
        return BadRequest(new
        {
            success = false,
            message =
                "El rango no puede superar 366 días."
        });
    }

    var citas = await _service.ObtenerPorRango(
        desde,
        hasta);

    return Ok(new
    {
        success = true,
        message = "OK",
        result = citas
    });
}

        [HttpPost("Cancelar/{id}")]
        public async Task<IActionResult>
            Cancelar(Guid id)
        {
            try
            {
                await _service.Cancelar(id);
                return Ok(Respuesta(
                    "Cita cancelada."));
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPost("Reprogramar/{id}")]
        public async Task<IActionResult>
            Reprogramar(
                Guid id,
                [FromBody]
                ReprogramarCitaDto datos)
        {
            try
            {
                await _service.Reprogramar(
                    id,
                    datos.NuevaFecha);

                return Ok(Respuesta(
                    "Cita reprogramada."));
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        private static object Respuesta(
            string mensaje)
        {
            return new
            {
                success = true,
                message = mensaje,
                result = (object?)null
            };
        }

        private IActionResult Error(
            Exception ex)
        {
            return ex switch
            {
                KeyNotFoundException =>
                    NotFound(new
                    {
                        success = false,
                        message = ex.Message
                    }),

                ArgumentException =>
                    BadRequest(new
                    {
                        success = false,
                        message = ex.Message
                    }),

                InvalidOperationException =>
                    Conflict(new
                    {
                        success = false,
                        message = ex.Message
                    }),

                _ => StatusCode(500, new
                {
                    success = false,
                    message =
                        "Error interno al procesar la cita."
                })
            };
        }
    }

    public class ReprogramarCitaDto
    {
        public DateTime NuevaFecha { get; set; }
    }
}