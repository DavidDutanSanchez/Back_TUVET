using Microsoft.EntityFrameworkCore;
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class HistorialClinicoService(
        TuVetContext context
    ) : IControladorHistorialClinico
    {
        private readonly TuVetContext _context = context;

        public async Task<HistorialClinicoDto?>
            ObtenerPorMascota(Guid idMascota)
        {
            var historial = await _context
                .HistorialesClinicos
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id_Mascota == idMascota
                );

            if (historial == null)
            {
                return null;
            }

            var mascota = await _context.Mascotas
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.IdMascota == idMascota
                );

            return new HistorialClinicoDto
            {
                IdHistorialClinico =
                    historial.IdHistorialClinico,

                Id_Mascota =
                    historial.Id_Mascota,

                NombreMascota =
                    mascota?.Nombre ?? string.Empty,

                FechaApertura =
                    historial.FechaApertura,

                AntecedentesClinicos =
                    historial.AntecedentesClinicos,

                AlergiasConocidas =
                    historial.AlergiasConocidas,

                EnfermedadesCronicas =
                    historial.EnfermedadesCronicas,

                ObservacionesGenerales =
                    historial.ObservacionesGenerales,

                Activo =
                    historial.Activo
            };
        }

        public async Task<Guid> CrearHistorial(
            HistorialClinicoCreateDto dto
        )
        {
            var mascotaExiste = await _context.Mascotas
                .AnyAsync(
                    x => x.IdMascota == dto.Id_Mascota
                );

            if (!mascotaExiste)
            {
                throw new InvalidOperationException(
                    "La mascota no existe."
                );
            }

            var historialExistente = await _context
                .HistorialesClinicos
                .FirstOrDefaultAsync(
                    x => x.Id_Mascota == dto.Id_Mascota
                );

            if (historialExistente != null)
            {
                return historialExistente.IdHistorialClinico;
            }

            var historial = new HistorialesClinicos
            {
                IdHistorialClinico = Guid.NewGuid(),

                Id_Mascota = dto.Id_Mascota,

                FechaApertura = DateTime.Now,

                AntecedentesClinicos =
                    dto.AntecedentesClinicos,

                AlergiasConocidas =
                    dto.AlergiasConocidas,

                EnfermedadesCronicas =
                    dto.EnfermedadesCronicas,

                ObservacionesGenerales =
                    dto.ObservacionesGenerales,

                Activo = true,

                FechaCreacion = DateTime.Now
            };

            _context.HistorialesClinicos.Add(
                historial
            );

            await _context.SaveChangesAsync();

            return historial.IdHistorialClinico;
        }

        public async Task<string> ActualizarHistorial(
            HistorialClinicoUpdateDto dto
        )
        {
            var historial = await _context
                .HistorialesClinicos
                .FindAsync(dto.IdHistorialClinico);

            if (historial == null)
            {
                return "Historial clínico no encontrado.";
            }

            historial.AntecedentesClinicos =
                dto.AntecedentesClinicos;

            historial.AlergiasConocidas =
                dto.AlergiasConocidas;

            historial.EnfermedadesCronicas =
                dto.EnfermedadesCronicas;

            historial.ObservacionesGenerales =
                dto.ObservacionesGenerales;

            historial.FechaModificacion =
                DateTime.Now;

            await _context.SaveChangesAsync();

            return "Realizado";
        }

        public async Task<Guid> CrearAtencion(
            AtencionClinicaCreateDto dto
        )
        {
            var mascotaExiste = await _context.Mascotas
                .AnyAsync(
                    x => x.IdMascota == dto.Id_Mascota
                );

            if (!mascotaExiste)
            {
                throw new InvalidOperationException(
                    "La mascota no existe."
                );
            }

            var usuarioExiste = await _context.Usuarios
                .AnyAsync(
                    x =>
                        x.IdUsuario ==
                        dto.Id_UsuarioResponsable
                        && x.Estado
                );

            if (!usuarioExiste)
            {
                throw new InvalidOperationException(
                    "El usuario responsable no existe o está inactivo."
                );
            }

            var historial = await _context
                .HistorialesClinicos
                .FirstOrDefaultAsync(
                    x => x.Id_Mascota == dto.Id_Mascota
                );

            if (historial == null)
            {
                historial = new HistorialesClinicos
                {
                    IdHistorialClinico = Guid.NewGuid(),

                    Id_Mascota = dto.Id_Mascota,

                    FechaApertura = DateTime.Now,

                    FechaCreacion = DateTime.Now,

                    Activo = true
                };

                _context.HistorialesClinicos.Add(
                    historial
                );
            }

            var atencion = new AtencionesClinicas
            {
                IdAtencionClinica = Guid.NewGuid(),

                Id_HistorialClinico =
                    historial.IdHistorialClinico,

                Id_Mascota = dto.Id_Mascota,

                Id_UsuarioResponsable =
                    dto.Id_UsuarioResponsable,

                FechaAtencion =
                    dto.FechaAtencion ?? DateTime.Now,

                TipoAtencion = dto.TipoAtencion,

                MotivoConsulta = dto.MotivoConsulta,

                Anamnesis = dto.Anamnesis,

                ExamenFisico = dto.ExamenFisico,

                PesoKg = dto.PesoKg,

                Temperatura = dto.Temperatura,

                FrecuenciaCardiaca =
                    dto.FrecuenciaCardiaca,

                FrecuenciaRespiratoria =
                    dto.FrecuenciaRespiratoria,

                Diagnostico = dto.Diagnostico,

                Procedimiento = dto.Procedimiento,

                Tratamiento = dto.Tratamiento,

                Recomendaciones = dto.Recomendaciones,

                ProximoControl = dto.ProximoControl,

                Observaciones = dto.Observaciones,

                Estado = 0,

                FechaCreacion = DateTime.Now
            };

            _context.AtencionesClinicas.Add(
                atencion
            );

            await _context.SaveChangesAsync();

            return atencion.IdAtencionClinica;
        }

        public async Task<List<AtencionesClinicas>>
            ObtenerAtenciones(Guid idMascota)
        {
            return await _context.AtencionesClinicas
                .AsNoTracking()
                .Where(
                    x => x.Id_Mascota == idMascota
                )
                .OrderByDescending(
                    x => x.FechaAtencion
                )
                .ToListAsync();
        }
    }
}