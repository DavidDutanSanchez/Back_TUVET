using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorProductos
    {
        //CRUD Productos
        public Task<string> CreateProducto(Productos Productos);
        public Task<PaginationDto<Productos>> AllProductos(QueryParams qParams);
        public Task<string> UpdateProducto(Productos Productos);
        public Task<string> DeleteProductos(Guid iD);
    }
}