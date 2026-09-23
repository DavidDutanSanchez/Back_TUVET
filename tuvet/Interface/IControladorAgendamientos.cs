using tu_vet_back.tuvet.Dtos.TuVetDto;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorAgendamientos
    {
        Task<List<AgendamientoListadoDto>>
            ObtenerPorFecha(DateTime fecha);

        Task<Guid> Crear(
            CrearAgendamientoDto datos);

        Task Confirmar(Guid id);

        Task RegistrarLlegada(Guid id);

        Task Cancelar(Guid id);

        Task Reprogramar(
            Guid id,
            DateTime nuevaFecha);

        Task<List<AgendamientoListadoDto>>
    ObtenerPorRango(
        DateTime desde,
        DateTime hasta);
    }
}