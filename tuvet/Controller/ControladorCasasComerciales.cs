using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorCasasComerciales
        : ControllerBase
    {
        private readonly IControladorCasasComerciales
            _service;

        public ControladorCasasComerciales(
            IControladorCasasComerciales service)
        {
            _service = service;
        }

        [HttpGet("FindAllCasasComerciales")]
        public async Task<IActionResult>
            FindAllCasasComerciales()
        {
            var casas = await _service.ObtenerTodas();

            return Ok(new
            {
                success = true,
                message = "OK",
                result = casas
            });
        }

        [HttpPut("AddCasaComercial")]
        public async Task<IActionResult>
            AddCasaComercial(
                [FromBody] CasasComerciales datos)
        {
            try
            {
                var nueva = await _service.Crear(datos);

                return Ok(new
                {
                    success = true,
                    message = "Casa comercial registrada.",
                    result = nueva
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    result = (object?)null
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    success = false,
                    message = ex.Message,
                    result = (object?)null
                });
            }
        }
    }
}