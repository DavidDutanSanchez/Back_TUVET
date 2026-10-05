using tu_vet_back.tuvet.Dtos.TuVetDto;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorVentas
    {
        Task<List<VentaListadoDto>>
            ObtenerVentas();

        Task<VentaCompletaDto?>
            ObtenerVenta(Guid idVenta);

        Task<List<VentaListadoDto>>
            ObtenerVentasPorPersona(Guid idPersona);

        Task<List<VentaListadoDto>>
            ObtenerVentasPorMascota(Guid idMascota);

        Task<VentaResultadoDto>
            CrearVenta(CrearVentaDto dto);

        Task<string>
            AnularVenta(AnularVentaDto dto);

        Task<decimal>
            ObtenerStockProducto(Guid idProducto);
    }
}