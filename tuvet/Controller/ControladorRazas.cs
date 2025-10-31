using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorRazas(IControladorRazas razasService) : SistecControllerBase
    {
        private readonly IControladorRazas _razasService = razasService;

        //CRUD Razas
        [HttpGet("FindAllRazas")]
        public async Task<ActionResult<PaginationDto<Razas>>> FindAllRazas([FromQuery] QueryParams qParams)
        {
            PaginationDto<Razas> pagedResult = await _razasService.AllRazas(qParams);
            return Ok(pagedResult);
        }
        [HttpPut("AddRaza")]
        public async Task<IActionResult> AddRaza([FromBody] Razas raza)
        {
            string response = await _razasService.CreateRaza(raza);
            return response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response);
        }
        [HttpPost("UpdateRaza")]
        public async Task<ActionResult> UpdateRaza([FromBody] Razas raza)
        {
            string response = await _razasService.UpdateRaza(raza);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }

        [HttpDelete("DeleteRaza/{id}")]
        public async Task<ActionResult> DeleteRaza([FromRoute] Guid id)
        {
            string response = await _razasService.DeleteRaza(id);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }
    }
}