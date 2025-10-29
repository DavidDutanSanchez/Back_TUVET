using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController(IControladorCategorias categoriasService) : SistecControllerBase
    {
         private readonly IControladorCategorias _categoriasService = categoriasService;

        //CRUD Categorias
        [HttpGet("FindaAllCategorias")]
        public async Task<ActionResult<PaginationDto<Categorias>>> FindaAllCategorias([FromQuery] QueryParams qParams)
        {
            PaginationDto<Categorias> pagedResult = await _categoriasService.AllCategorias(qParams);
            return Ok(pagedResult);
        }
        [HttpPut("AddCategorias")]
        public async Task<IActionResult> AddCategorias([FromBody] Categorias categorias)
        {
            string response = await _categoriasService.CreateCategorias(categorias);
            return response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response);
        }
        [HttpPost("UpdateCategorias")]
        public async Task<ActionResult> UpdateCategorias([FromBody] Categorias categorias)
        {
            string response = await _categoriasService.UpdateCategorias(categorias);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }

        [HttpDelete("DeleteCategorias/{id}")]
        public async Task<ActionResult> DeleteCategorias([FromRoute] Guid id)
        {
            string response = await _categoriasService.DeleteCategorias(id);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }
    }
}