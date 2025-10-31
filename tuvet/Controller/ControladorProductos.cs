using Microsoft.AspNetCore.Mvc;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorProductos(IControladorProductos productosService) : SistecControllerBase
    {
        private readonly IControladorProductos _productosService = productosService;

        //CRUD Prodcutos
        [HttpGet("FindAllProductos")]
        public async Task<ActionResult<PaginationDto<Productos>>> FindAllProductos([FromQuery] QueryParams qParams)
        {
            PaginationDto<Productos> pagedResult = await _productosService.AllProductos(qParams);
            return Ok(pagedResult);
        }
        [HttpPut("AddProducto")]
        public async Task<IActionResult> AddProducto([FromBody] Productos producto)
        {
            string response = await _productosService.CreateProducto(producto);
            return response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response);
        }
        [HttpPost("UpdateProducto")]
        public async Task<ActionResult> UpdateProducto([FromBody] Productos producto)
        {
            string response = await _productosService.UpdateProducto(producto);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }

        [HttpDelete("DeleteProducto/{id}")]
        public async Task<ActionResult> DeleteProducto([FromRoute] Guid id)
        {
            string response = await _productosService.DeleteProductos(id);
            return (ActionResult)(response == "Realizado" ? Ok(response) : (IActionResult)InternalServerError(response));
        }
    }
}