using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos;
using tu_vet_back.tuvet.Extensions;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.Parameters;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class PersonasService(TuVetContext context) : IControladorPersonas
    {
        private readonly TuVetContext _context = context;


        // =====================================================
        // OBTENER TODAS LAS PERSONAS
        // =====================================================
        public async Task<PaginationDto<Personas>> AllPersonas(
            QueryParams qParams
        )
        {
            try
            {
                var campoOrdenamiento =
                    ResolverCampoOrdenamiento(qParams.orderBy);

                var personas = await _context.Personas

                    .ApplySearch(
                        qParams.search,
                        p => p.Nombres,
                        p => p.Apellidos,
                        p => p.NumeroIdentificacion
                    )

                    .OrderBy(
                        campoOrdenamiento,
                        qParams.isOrderByDescending
                    )

                    .GetPagedAsync(qParams);

                return personas;
            }
            catch
            {
                throw;
            }
        }


        // =====================================================
        // CREAR PERSONA
        // =====================================================
        public async Task<string> CreatePersona(
            Personas personas
        )
        {
            try
            {
                _context.Personas.Add(personas);

                await _context.SaveChangesAsync();

                return "Realizado";
            }
            catch (Exception ex)
            {
                return ex.InnerException?.Message
                    ?? ex.Message;
            }
        }


        // =====================================================
        // ACTUALIZAR PERSONA
        // =====================================================
        public async Task<string> UpdatePersona(
            Personas personas
        )
        {
            try
            {
                _context.Personas.Update(personas);

                await _context.SaveChangesAsync();

                return "Realizado";
            }
            catch (Exception ex)
            {
                return ex.InnerException?.Message
                    ?? ex.Message;
            }
        }


        // =====================================================
        // ELIMINAR PERSONA
        // =====================================================
        public async Task<string> DeletePersona(
            Guid id
        )
        {
            try
            {
                var persona =
                    await _context.Personas.FindAsync(id);

                if (persona == null)
                {
                    return "La persona no existe.";
                }

                _context.Personas.Remove(persona);

                await _context.SaveChangesAsync();

                return "Realizado";
            }
            catch (Exception ex)
            {
                return ex.InnerException?.Message
                    ?? ex.Message;
            }
        }


        // =====================================================
        // RESOLVER CAMPO DE ORDENAMIENTO
        // =====================================================
        private static string ResolverCampoOrdenamiento(
            string? orderBy
        )
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return nameof(Personas.Apellidos);
            }

            return orderBy.Trim().ToLowerInvariant() switch
            {
                "idpersona" =>
                    nameof(Personas.IdPersona),

                "tipoidentificacion" =>
                    nameof(Personas.TipoIdentificacion),

                "numeroidentificacion" =>
                    nameof(Personas.NumeroIdentificacion),

                "nombres" =>
                    nameof(Personas.Nombres),

                "apellidos" =>
                    nameof(Personas.Apellidos),

                "tipotelefono" =>
                    nameof(Personas.TipoTelefono),

                "telefono" =>
                    nameof(Personas.Telefono),

                "correoelectronico" =>
                    nameof(Personas.CorreoElectronico),

                "direccion" =>
                    nameof(Personas.Direccion),

                _ =>
                    nameof(Personas.Apellidos)
            };
        }
    }
}