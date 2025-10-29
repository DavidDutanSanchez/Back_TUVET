using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorColores(IControladorColores coloresService) : SistecControllerBase
    {
         private readonly IControladorColores _coloresService = coloresService;

        //CRUD Colores
        [HttpGet("FindaAllColores")]
        public async Task<ActionResult<PaginationDto<Colores>>> FindaAllColores([FromQuery] QueryParams qParams)
        {
            PaginationDto<Colores> pagedResult = await _coloresService.AllColores(qParams);
            return Ok(pagedResult);
        }
        [HttpPut("AddColor")]
        public async Task<IActionResult> AddColor([FromBody] Colores Colores)
        {
            string response = await _coloresService.CreateColor(Colores);
            return response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response);
        }
        [HttpPost("UpdateColor")]
        public async Task<ActionResult> UpdateColor([FromBody] Colores Colores)
        {
            string response = await _coloresService.UpdateColor(Colores);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }

        [HttpDelete("DeleteColor/{id}")]
        public async Task<ActionResult> DeleteColor([FromRoute] Guid id)
        {
            string response = await _coloresService.DeleteColor(id);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }
    }
}