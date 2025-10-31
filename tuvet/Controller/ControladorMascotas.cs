using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorMascotas(IControladorMascotas mascotasService) : SistecControllerBase
    {
         private readonly IControladorMascotas _mascotasService = mascotasService;

        //CRUD Mascotas
        [HttpGet("FindAllMascotas")]
        public async Task<ActionResult<PaginationDto<Mascotas>>> FindAllMascotas([FromQuery] QueryParams qParams)
        {
            PaginationDto<Mascotas> pagedResult = await _mascotasService.AllMascotas(qParams);
            return Ok(pagedResult);
        }
        [HttpPut("AddMascotas")]
        public async Task<IActionResult> AddMascotas([FromBody] Mascotas mascotas)
        {
            string response = await _mascotasService.CreateMascota(mascotas);
            return response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response);
        }
        [HttpPost("UpdateMascotas")]
        public async Task<ActionResult> UpdateMascotas([FromBody] Mascotas mascotas)
        {
            string response = await _mascotasService.UpdateMascota(mascotas);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }

        [HttpDelete("DeleteMascotas/{id}")]
        public async Task<ActionResult> DeleteMascotas([FromRoute] Guid id)
        {
            string response = await _mascotasService.DeleteMascota(id);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }
    }
}