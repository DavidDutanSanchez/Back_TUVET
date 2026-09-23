using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController(
        IControladorCategorias categoriasService
    ) : SistecControllerBase
    {
        private readonly IControladorCategorias _categoriasService =
            categoriasService;


        // =====================================================
        // OBTENER CATEGORÍAS
        // =====================================================

        [HttpGet("FindAllCategorias")]
        public async Task<ActionResult<PaginationDto<Categorias>>>
            FindAllCategorias(
                [FromQuery] QueryParams qParams
            )
        {
            PaginationDto<Categorias> pagedResult =
                await _categoriasService.AllCategorias(
                    qParams
                );

            return Ok(pagedResult);
        }


        // =====================================================
        // CREAR CATEGORÍA
        // =====================================================

        [HttpPut("AddCategorias")]
        public async Task<IActionResult> AddCategorias(
            [FromBody] Categorias categorias
        )
        {
            string response =
                await _categoriasService.CreateCategorias(
                    categorias
                );

            return response == "Realizado"
                ? Ok(response)
                : InternalServerError(response);
        }


        // =====================================================
        // ACTUALIZAR CATEGORÍA
        // =====================================================

        [HttpPost("UpdateCategorias")]
        public async Task<IActionResult> UpdateCategorias(
            [FromBody] Categorias categorias
        )
        {
            string response =
                await _categoriasService.UpdateCategorias(
                    categorias
                );

            return response == "Realizado"
                ? Ok(response)
                : InternalServerError(response);
        }


        // =====================================================
        // ELIMINAR CATEGORÍA
        // =====================================================

        [HttpDelete("DeleteCategorias/{id}")]
        public async Task<IActionResult> DeleteCategorias(
            [FromRoute] Guid id
        )
        {
            string response =
                await _categoriasService.DeleteCategorias(
                    id
                );

            return response == "Realizado"
                ? Ok(response)
                : InternalServerError(response);
        }
    }
}