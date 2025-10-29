using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorInventarios(IControladorInventarios inventariosService) : SistecControllerBase
    {
         private readonly IControladorInventarios _inventarioService = inventariosService;

        //CRUD Inventarios
        [HttpGet("FindAllInventarios")]
        public async Task<ActionResult<PaginationDto<Inventarios>>> FindAllInventarios([FromQuery] QueryParams qParams)
        {
            PaginationDto<Inventarios> pagedResult = await _inventarioService.AllInventarios(qParams);
            return Ok(pagedResult);
        }
        [HttpPut("AddInventarios")]
        public async Task<IActionResult> AddInventarios([FromBody] Inventarios Inventarios)
        {
            string response = await _inventarioService.CreateInventario(Inventarios);
            return response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response);
        }
        [HttpPost("UpdateInventarios")]
        public async Task<ActionResult> UpdateInventarios([FromBody] Inventarios Inventarios)
        {
            string response = await _inventarioService.UpdateInventario(Inventarios);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }

        [HttpDelete("DeleteInventarios/{id}")]
        public async Task<ActionResult> DeleteInventarios([FromRoute] Guid id)
        {
            string response = await _inventarioService.DeleteInventario(id);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }
    }
}