using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Interface;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorVentas(
        IControladorVentas service
    ) : SistecControllerBase
    {
        private readonly IControladorVentas
            _service = service;

        [HttpGet("FindAllVentas")]
        public async Task<IActionResult>
            Listar()
        {
            return Ok(
                await _service.ObtenerVentas()
            );
        }

        [HttpGet("FindVentaById/{idVenta}")]
        public async Task<IActionResult>
            Obtener(Guid idVenta)
        {
            var resultado =
                await _service.ObtenerVenta(
                    idVenta
                );

            return resultado == null
                ? NotFound("Venta no encontrada.")
                : Ok(resultado);
        }

        [HttpGet(
            "FindVentasByPersona/{idPersona}"
        )]
        public async Task<IActionResult>
            PorPersona(Guid idPersona)
        {
            return Ok(
                await _service
                    .ObtenerVentasPorPersona(
                        idPersona
                    )
            );
        }

        [HttpGet(
            "FindVentasByMascota/{idMascota}"
        )]
        public async Task<IActionResult>
            PorMascota(Guid idMascota)
        {
            return Ok(
                await _service
                    .ObtenerVentasPorMascota(
                        idMascota
                    )
            );
        }

        [HttpGet(
            "FindStockProducto/{idProducto}"
        )]
        public async Task<IActionResult>
            Stock(Guid idProducto)
        {
            return Ok(
                await _service
                    .ObtenerStockProducto(
                        idProducto
                    )
            );
        }

        [HttpPut("AddVenta")]
        public async Task<IActionResult>
            Crear(
                [FromBody] CrearVentaDto dto
            )
        {
            try
            {
                return Ok(
                    await _service.CrearVenta(dto)
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AnularVenta")]
        public async Task<IActionResult>
            Anular(
                [FromBody] AnularVentaDto dto
            )
        {
            try
            {
                return Ok(
                    await _service.AnularVenta(dto)
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}