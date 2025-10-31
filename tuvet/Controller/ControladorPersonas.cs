using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorPersonas(IControladorPersonas personasService) : SistecControllerBase
    {
         private readonly IControladorPersonas _personasService = personasService;

        //CRUD Personas
        [HttpGet("FindAllPersonas")]
        public async Task<ActionResult<PaginationDto<Personas>>> FindAllPersonas([FromQuery] QueryParams qParams)
        {
            PaginationDto<Personas> pagedResult = await _personasService.AllPersonas(qParams);
            return Ok(pagedResult);
        }
        [HttpPut("AddPersonas")]
        public async Task<IActionResult> AddPersonas([FromBody] Personas personas)
        {
            string response = await _personasService.CreatePersona(personas);
            return response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response);
        }
        [HttpPost("UpdatePersonas")]
        public async Task<ActionResult> UpdatePersonas([FromBody] Personas personas)
        {
            string response = await _personasService.UpdatePersona(personas);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }

        [HttpDelete("DeletePersonas/{id}")]
        public async Task<ActionResult> DeletePersonas([FromRoute] Guid id)
        {
            string response = await _personasService.DeletePersona(id);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }
    }
}