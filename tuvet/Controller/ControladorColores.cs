using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorColores(
        IControladorColores coloresService
    ) : SistecControllerBase
    {
        private readonly IControladorColores
            _coloresService = coloresService;


        // =========================================================
        // LISTAR COLORES
        // =========================================================

        [HttpGet("FindAllColores")]
        public async Task<
            ActionResult<PaginationDto<Colores>>
        > FindAllColores(
            [FromQuery] QueryParams qParams
        )
        {
            PaginationDto<Colores> pagedResult =
                await _coloresService
                    .AllColores(qParams);

            return Ok(pagedResult);
        }


        // =========================================================
        // CREAR COLOR
        // =========================================================

        [HttpPut("AddColor")]
        public async Task<IActionResult> AddColor(
            [FromBody] Colores color
        )
        {
            string response =
                await _coloresService
                    .CreateColor(color);

            return response == "Realizado"
                ? Ok(response)
                : InternalServerError(response);
        }


        // =========================================================
        // ACTUALIZAR COLOR
        // =========================================================

        [HttpPost("UpdateColor")]
        public async Task<IActionResult> UpdateColor(
            [FromBody] Colores color
        )
        {
            string response =
                await _coloresService
                    .UpdateColor(color);

            return response == "Realizado"
                ? Ok(response)
                : InternalServerError(response);
        }


        // =========================================================
        // ELIMINAR COLOR
        // =========================================================

        [HttpDelete("DeleteColor/{id}")]
        public async Task<IActionResult> DeleteColor(
            [FromRoute] Guid id
        )
        {
            string response =
                await _coloresService
                    .DeleteColor(id);

            return response == "Realizado"
                ? Ok(response)
                : InternalServerError(response);
        }
    }
}