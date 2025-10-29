using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorEspecies(IControladorEspecies especieService) : SistecControllerBase
    {
         private readonly IControladorEspecies _epecieService = especieService;

        //CRUD Especies
        [HttpGet("FindAllEspecies")]
        public async Task<ActionResult<PaginationDto<Especies>>> FindAllEspecies([FromQuery] QueryParams qParams)
        {
            PaginationDto<Especies> pagedResult = await _epecieService.AllEspecies(qParams);
            return Ok(pagedResult);
        }
        [HttpPut("AddEspecie")]
        public async Task<IActionResult> AddEspecie([FromBody] Especies Especies)
        {
            string response = await _epecieService.CreateEspecie(Especies);
            return response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response);
        }
        [HttpPost("UpdateEspecie")]
        public async Task<ActionResult> UpdateEspecie([FromBody] Especies Especies)
        {
            string response = await _epecieService.UpdateEspecie(Especies);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }

        [HttpDelete("DeleteEspecie/{id}")]
        public async Task<ActionResult> DeleteEspecie([FromRoute] Guid id)
        {
            string response = await _epecieService.DeleteEspecie(id);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }
    }
}