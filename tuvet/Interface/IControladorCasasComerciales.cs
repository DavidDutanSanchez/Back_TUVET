using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Interface
{
    public interface IControladorCasasComerciales
    {
        Task<List<CasasComerciales>> ObtenerTodas();

        Task<CasasComerciales> Crear(
            CasasComerciales casaComercial);
    }
}