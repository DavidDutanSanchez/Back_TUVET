using Microsoft.EntityFrameworkCore;
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class CasasComercialesService
        : IControladorCasasComerciales
    {
        private readonly TuVetContext _context;

        public CasasComercialesService(
            TuVetContext context)
        {
            _context = context;
        }

        public async Task<List<CasasComerciales>>
            ObtenerTodas()
        {
            return await _context.CasasComerciales
                .AsNoTracking()
                .OrderBy(x => x.NombreCasaComercial)
                .ToListAsync();
        }

        public async Task<CasasComerciales> Crear(
            CasasComerciales casaComercial)
        {
            string nombre =
                casaComercial.NombreCasaComercial?.Trim()
                ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException(
                    "Ingrese el nombre de la casa comercial.");
            }

            if (nombre.Length > 255)
            {
                throw new ArgumentException(
                    "El nombre no puede superar 255 caracteres.");
            }

            bool existe = await _context.CasasComerciales
                .AnyAsync(x =>
                    x.NombreCasaComercial.ToLower()
                    == nombre.ToLower());

            if (existe)
            {
                throw new InvalidOperationException(
                    "La casa comercial ya está registrada.");
            }

            var nueva = new CasasComerciales
            {
                IdCasaComercial = Guid.NewGuid(),
                NombreCasaComercial = nombre
            };

            _context.CasasComerciales.Add(nueva);

            await _context.SaveChangesAsync();

            return nueva;
        }
    }
}