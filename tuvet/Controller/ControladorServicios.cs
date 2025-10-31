using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorServicios(IControladorServicios serviciosService) : SistecControllerBase
    {
        private readonly IControladorServicios _serviciosService = serviciosService;

        //CRUD Servicios
        [HttpGet("FindAllServicios")]
        public async Task<ActionResult<PaginationDto<Servicios>>> FindAllServicios([FromQuery] QueryParams qParams)
        {
            PaginationDto<Servicios> pagedResult = await _serviciosService.AllServicios(qParams);
            return Ok(pagedResult);
        }
        [HttpPut("AddServicio")]
        public async Task<IActionResult> AddServicio([FromBody] Servicios servicios)
        {
            string response = await _serviciosService.CreateServicio(servicios);
            return response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response);
        }
        [HttpPost("UpdateServicios")]
        public async Task<ActionResult> UpdateServicios([FromBody] Servicios servicios)
        {
            string response = await _serviciosService.UpdateServicio(servicios);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }

        [HttpDelete("DeleteServicios/{id}")]
        public async Task<ActionResult> DeleteServicios([FromRoute] Guid id)
        {
            string response = await _serviciosService.DeleteServicio(id);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }
    }
}