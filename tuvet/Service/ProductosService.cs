
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Extensions;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class ProdcutosService(TuVetContext context) : IControladorProductos
    {
        private readonly TuVetContext _context = context;

        public async Task<PaginationDto<Productos>> AllProductos(QueryParams qParams)
        {
            try
            {
                PaginationDto<Productos> prodcutos = await _context.Productos
                .OrderBy(c => c.NombreProducto)
                .ApplySearch(qParams.search, t => t.NombreProducto, t => t.CodigProducto, t => t.DescripcionProducto)
                .OrderBy(qParams.orderBy ?? nameof(Productos.NombreProducto), qParams.isOrderByDescending)
                .GetPagedAsync(qParams);
                return prodcutos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message + " mensaje: " + ex.Message);
            }
        }
        public async Task<string> CreateProducto(Productos Productos)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Productos.Add(Productos);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> DeleteProductos(Guid iD)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Productos.Remove(_context.Productos.Where(x => x.IdProductos == iD).First());
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> UpdateProducto(Productos Productos)
        {
            string response = "Realizado";
            try
            {
                _ = _context.Productos.Update(Productos);
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.InnerException?.Message + "Mensaje : " + ex.Message;
                return response;
            }
            return response;
        }
    }
}