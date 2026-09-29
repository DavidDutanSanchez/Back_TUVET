using Microsoft.EntityFrameworkCore;
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class HospitalizacionesService(
        TuVetContext context
    ) : IControladorHospitalizaciones
    {
        private readonly TuVetContext _context = context;

        // =====================================================
        // LISTAR HOSPITALIZACIONES
        // =====================================================

        public async Task<List<HospitalizacionListadoDto>>
            ObtenerHospitalizaciones()
        {
            var registros = await (
                from h in _context.Hospitalizaciones

                join m in _context.Mascotas
                    on h.Id_Mascota equals m.IdMascota

                join p in _context.Personas
                    on m.Id_Persona equals p.IdPersona

                join u in _context.Usuarios
                    on h.Id_UsuarioResponsable
                    equals u.IdUsuario

                select new HospitalizacionListadoDto
                {
                    IdHospitalizacion =
                        h.IdHospitalizacion,

                    Id_Mascota = h.Id_Mascota,

                    NombreMascota = m.Nombre,

                    NombrePropietario =
                        p.Nombres + " " + p.Apellidos,

                    Id_UsuarioResponsable =
                        h.Id_UsuarioResponsable,

                    VeterinarioResponsable =
                        u.NombreUsuario,

                    FechaIngreso = h.FechaIngreso,

                    PesoIngresoKg = h.PesoIngresoKg,

                    DiagnosticoIngreso =
                        h.DiagnosticoIngreso,

                    EstadoHospitalizacion =
                        h.EstadoHospitalizacion,

                    FechaAlta = h.FechaAlta
                }
            )
            .AsNoTracking()
            .OrderByDescending(
                x => x.FechaIngreso
            )
            .ToListAsync();

            return registros;
        }

        // =====================================================
        // OBTENER POR ID
        // =====================================================

        public async Task<Hospitalizaciones?>
            ObtenerPorId(Guid id)
        {
            return await _context.Hospitalizaciones
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.IdHospitalizacion == id
                );
        }

        // =====================================================
        // OBTENER POR MASCOTA
        // =====================================================

        public async Task<List<Hospitalizaciones>>
            ObtenerPorMascota(Guid idMascota)
        {
            return await _context.Hospitalizaciones
                .AsNoTracking()
                .Where(
                    x => x.Id_Mascota == idMascota
                )
                .OrderByDescending(
                    x => x.FechaIngreso
                )
                .ToListAsync();
        }

        // =====================================================
        // CREAR HOSPITALIZACIÓN
        // =====================================================

        public async Task<Guid> CrearHospitalizacion(
            HospitalizacionCreateDto dto
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

            await ValidarUsuario(
                dto.Id_UsuarioResponsable
            );

            if (dto.PesoIngresoKg < 0)
            {
                throw new InvalidOperationException(
                    "El peso no puede ser negativo."
                );
            }

            var ingresoActivo = await _context
                .Hospitalizaciones
                .AnyAsync(
                    x =>
                        x.Id_Mascota == dto.Id_Mascota
                        && x.EstadoHospitalizacion == 0
                );

            if (ingresoActivo)
            {
                throw new InvalidOperationException(
                    "La mascota ya tiene una hospitalización activa."
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

            if (dto.Id_AtencionClinica.HasValue)
            {
                var atencionValida = await _context
                    .AtencionesClinicas
                    .AnyAsync(
                        x =>
                            x.IdAtencionClinica ==
                            dto.Id_AtencionClinica.Value
                            && x.Id_Mascota == dto.Id_Mascota
                    );

                if (!atencionValida)
                {
                    throw new InvalidOperationException(
                        "La atención clínica no corresponde a la mascota."
                    );
                }
            }

            var hospitalizacion = new Hospitalizaciones
            {
                IdHospitalizacion = Guid.NewGuid(),

                Id_HistorialClinico =
                    historial.IdHistorialClinico,

                Id_Mascota = dto.Id_Mascota,

                Id_UsuarioResponsable =
                    dto.Id_UsuarioResponsable,

                Id_AtencionClinica =
                    dto.Id_AtencionClinica,

                FechaIngreso =
                    dto.FechaIngreso ?? DateTime.Now,

                PesoIngresoKg =
                    dto.PesoIngresoKg,

                EdadAlIngreso =
                    dto.EdadAlIngreso,

                MotivoIngreso =
                    dto.MotivoIngreso,

                DiagnosticoIngreso =
                    dto.DiagnosticoIngreso,

                Procedimiento =
                    dto.Procedimiento,

                PlanTerapeutico =
                    dto.PlanTerapeutico,

                ObservacionesIngreso =
                    dto.ObservacionesIngreso,

                EstadoHospitalizacion = 0,

                FechaCreacion = DateTime.Now
            };

            _context.Hospitalizaciones.Add(
                hospitalizacion
            );

            await _context.SaveChangesAsync();

            return hospitalizacion.IdHospitalizacion;
        }

        // =====================================================
        // ACTUALIZAR HOSPITALIZACIÓN
        // =====================================================

        public async Task<string> ActualizarHospitalizacion(
            HospitalizacionUpdateDto dto
        )
        {
            var hospitalizacion = await _context
                .Hospitalizaciones
                .FindAsync(dto.IdHospitalizacion);

            if (hospitalizacion == null)
            {
                return "Hospitalización no encontrada.";
            }

            if (hospitalizacion.EstadoHospitalizacion != 0)
            {
                return "No se puede modificar una hospitalización cerrada.";
            }

            await ValidarUsuario(
                dto.Id_UsuarioResponsable
            );

            if (dto.PesoIngresoKg < 0)
            {
                return "El peso no puede ser negativo.";
            }

            hospitalizacion.Id_UsuarioResponsable =
                dto.Id_UsuarioResponsable;

            hospitalizacion.PesoIngresoKg =
                dto.PesoIngresoKg;

            hospitalizacion.EdadAlIngreso =
                dto.EdadAlIngreso;

            hospitalizacion.MotivoIngreso =
                dto.MotivoIngreso;

            hospitalizacion.DiagnosticoIngreso =
                dto.DiagnosticoIngreso;

            hospitalizacion.Procedimiento =
                dto.Procedimiento;

            hospitalizacion.PlanTerapeutico =
                dto.PlanTerapeutico;

            hospitalizacion.ObservacionesIngreso =
                dto.ObservacionesIngreso;

            hospitalizacion.FechaModificacion =
                DateTime.Now;

            await _context.SaveChangesAsync();

            return "Realizado";
        }

        // =====================================================
        // REGISTRAR TRATAMIENTO
        // =====================================================

        public async Task<Guid> RegistrarTratamiento(
            TratamientoCreateDto dto
        )
        {
            await ValidarHospitalizacionActiva(
                dto.Id_Hospitalizacion
            );

            await ValidarUsuario(
                dto.Id_UsuarioPrescriptor
            );

            if (string.IsNullOrWhiteSpace(
                dto.NombreMedicamento
            ))
            {
                throw new InvalidOperationException(
                    "Debe ingresar el nombre del medicamento."
                );
            }

            if (dto.Dosis < 0 ||
                dto.FrecuenciaHoras <= 0)
            {
                throw new InvalidOperationException(
                    "La dosis o frecuencia no es válida."
                );
            }

            var tratamiento =
                new HospitalizacionTratamientos
                {
                    IdTratamiento = Guid.NewGuid(),

                    Id_Hospitalizacion =
                        dto.Id_Hospitalizacion,

                    Id_UsuarioPrescriptor =
                        dto.Id_UsuarioPrescriptor,

                    Id_Producto = dto.Id_Producto,

                    NombreMedicamento =
                        dto.NombreMedicamento.Trim(),

                    Dosis = dto.Dosis,

                    UnidadDosis = dto.UnidadDosis,

                    ViaAdministracion =
                        dto.ViaAdministracion,

                    FrecuenciaHoras =
                        dto.FrecuenciaHoras,

                    IndicacionFrecuencia =
                        dto.IndicacionFrecuencia,

                    FechaInicio =
                        dto.FechaInicio ?? DateTime.Now,

                    FechaFin = dto.FechaFin,

                    Indicaciones = dto.Indicaciones,

                    Observaciones = dto.Observaciones,

                    EstadoTratamiento = 0,

                    FechaCreacion = DateTime.Now
                };

            _context.HospitalizacionTratamientos.Add(
                tratamiento
            );

            await _context.SaveChangesAsync();

            return tratamiento.IdTratamiento;
        }

        // =====================================================
        // REGISTRAR ADMINISTRACIÓN DE MEDICAMENTO
        // =====================================================

        public async Task<Guid> RegistrarAdministracion(
            AdministracionCreateDto dto
        )
        {
            await ValidarHospitalizacionActiva(
                dto.Id_Hospitalizacion
            );

            await ValidarUsuario(
                dto.Id_UsuarioResponsable
            );

            var tratamiento = await _context
                .HospitalizacionTratamientos
                .FirstOrDefaultAsync(
                    x =>
                        x.IdTratamiento == dto.Id_Tratamiento
                        && x.Id_Hospitalizacion ==
                        dto.Id_Hospitalizacion
                );

            if (tratamiento == null)
            {
                throw new InvalidOperationException(
                    "El tratamiento no pertenece a esta hospitalización."
                );
            }

            if (tratamiento.EstadoTratamiento != 0)
            {
                throw new InvalidOperationException(
                    "El tratamiento no está activo."
                );
            }

            if (dto.EstadoAdministracion < 0 ||
                dto.EstadoAdministracion > 3)
            {
                throw new InvalidOperationException(
                    "Estado de administración no válido."
                );
            }

            if (dto.DosisAdministrada < 0)
            {
                throw new InvalidOperationException(
                    "La dosis administrada no puede ser negativa."
                );
            }

            var administracion =
                new HospitalizacionAdministraciones
                {
                    IdAdministracion = Guid.NewGuid(),

                    Id_Tratamiento =
                        dto.Id_Tratamiento,

                    Id_Hospitalizacion =
                        dto.Id_Hospitalizacion,

                    Id_UsuarioResponsable =
                        dto.Id_UsuarioResponsable,

                    FechaProgramada =
                        dto.FechaProgramada,

                    FechaAdministracion =
                        dto.EstadoAdministracion == 1
                            ? dto.FechaAdministracion
                                ?? DateTime.Now
                            : null,

                    DosisAdministrada =
                        dto.DosisAdministrada,

                    UnidadDosis =
                        dto.UnidadDosis,

                    ViaAdministracion =
                        dto.ViaAdministracion,

                    EstadoAdministracion =
                        dto.EstadoAdministracion,

                    MotivoOmision =
                        dto.MotivoOmision,

                    Observaciones =
                        dto.Observaciones,

                    FechaCreacion =
                        DateTime.Now
                };

            _context.HospitalizacionAdministraciones.Add(
                administracion
            );

            await _context.SaveChangesAsync();

            return administracion.IdAdministracion;
        }

        // =====================================================
        // REGISTRAR MONITOREO
        // =====================================================

        public async Task<Guid> RegistrarMonitoreo(
            MonitoreoCreateDto dto
        )
        {
            await ValidarHospitalizacionActiva(
                dto.Id_Hospitalizacion
            );

            await ValidarUsuario(
                dto.Id_UsuarioResponsable
            );

            if (dto.PesoKg < 0 ||
                dto.FrecuenciaCardiaca < 0 ||
                dto.FrecuenciaRespiratoria < 0 ||
                dto.SaturacionOxigeno < 0 ||
                dto.SaturacionOxigeno > 100)
            {
                throw new InvalidOperationException(
                    "Existen valores de monitoreo no válidos."
                );
            }

            var monitoreo =
                new HospitalizacionMonitoreos
                {
                    IdMonitoreo = Guid.NewGuid(),

                    Id_Hospitalizacion =
                        dto.Id_Hospitalizacion,

                    Id_UsuarioResponsable =
                        dto.Id_UsuarioResponsable,

                    FechaMonitoreo =
                        dto.FechaMonitoreo ?? DateTime.Now,

                    Temperatura = dto.Temperatura,

                    FrecuenciaCardiaca =
                        dto.FrecuenciaCardiaca,

                    FrecuenciaRespiratoria =
                        dto.FrecuenciaRespiratoria,

                    PesoKg = dto.PesoKg,

                    PresionArterialSistolica =
                        dto.PresionArterialSistolica,

                    PresionArterialDiastolica =
                        dto.PresionArterialDiastolica,

                    SaturacionOxigeno =
                        dto.SaturacionOxigeno,

                    EstadoHidratacion =
                        dto.EstadoHidratacion,

                    ColorMucosas =
                        dto.ColorMucosas,

                    TiempoLlenadoCapilar =
                        dto.TiempoLlenadoCapilar,

                    NivelDolor =
                        dto.NivelDolor,

                    Apetito = dto.Apetito,

                    ConsumoAgua = dto.ConsumoAgua,

                    Vomitos = dto.Vomitos,

                    Miccion = dto.Miccion,

                    Defecacion = dto.Defecacion,

                    Comportamiento =
                        dto.Comportamiento,

                    EvolucionClinica =
                        dto.EvolucionClinica,

                    Observaciones =
                        dto.Observaciones,

                    FechaCreacion =
                        DateTime.Now
                };

            _context.HospitalizacionMonitoreos.Add(
                monitoreo
            );

            await _context.SaveChangesAsync();

            return monitoreo.IdMonitoreo;
        }

        // =====================================================
        // REGISTRAR ALTA MÉDICA
        // =====================================================

        public async Task<string> RegistrarAlta(
            AltaHospitalizacionDto dto
        )
        {
            var hospitalizacion = await _context
                .Hospitalizaciones
                .FindAsync(dto.IdHospitalizacion);

            if (hospitalizacion == null)
            {
                return "Hospitalización no encontrada.";
            }

            if (hospitalizacion.EstadoHospitalizacion != 0)
            {
                return "La hospitalización ya está cerrada.";
            }

            await ValidarUsuario(
                dto.Id_UsuarioAlta
            );

            var fechaAlta =
                dto.FechaAlta ?? DateTime.Now;

            if (fechaAlta < hospitalizacion.FechaIngreso)
            {
                return "El alta no puede ser anterior al ingreso.";
            }

            hospitalizacion.FechaAlta =
                fechaAlta;

            hospitalizacion.Id_UsuarioAlta =
                dto.Id_UsuarioAlta;

            hospitalizacion.DiagnosticoAlta =
                dto.DiagnosticoAlta;

            hospitalizacion.ResumenEvolucion =
                dto.ResumenEvolucion;

            hospitalizacion.CondicionAlta =
                dto.CondicionAlta;

            hospitalizacion.TratamientoDomiciliario =
                dto.TratamientoDomiciliario;

            hospitalizacion.CuidadosDomiciliarios =
                dto.CuidadosDomiciliarios;

            hospitalizacion.RecomendacionesAlta =
                dto.RecomendacionesAlta;

            hospitalizacion.FechaProximoControl =
                dto.FechaProximoControl;

            hospitalizacion.ObservacionesAlta =
                dto.ObservacionesAlta;

            hospitalizacion.EstadoHospitalizacion = 1;

            hospitalizacion.FechaModificacion =
                DateTime.Now;

            await _context.SaveChangesAsync();

            return "Realizado";
        }

        // =====================================================
        // CONSULTAR TRATAMIENTOS
        // =====================================================

        public async Task<List<HospitalizacionTratamientos>>
            ObtenerTratamientos(Guid idHospitalizacion)
        {
            return await _context
                .HospitalizacionTratamientos
                .AsNoTracking()
                .Where(
                    x =>
                        x.Id_Hospitalizacion ==
                        idHospitalizacion
                )
                .OrderByDescending(
                    x => x.FechaInicio
                )
                .ToListAsync();
        }

        // =====================================================
        // CONSULTAR ADMINISTRACIONES
        // =====================================================

        public async Task<List<HospitalizacionAdministraciones>>
            ObtenerAdministraciones(Guid idHospitalizacion)
        {
            return await _context
                .HospitalizacionAdministraciones
                .AsNoTracking()
                .Where(
                    x =>
                        x.Id_Hospitalizacion ==
                        idHospitalizacion
                )
                .OrderByDescending(
                    x => x.FechaCreacion
                )
                .ToListAsync();
        }

        // =====================================================
        // CONSULTAR MONITOREOS
        // =====================================================

        public async Task<List<HospitalizacionMonitoreos>>
            ObtenerMonitoreos(Guid idHospitalizacion)
        {
            return await _context
                .HospitalizacionMonitoreos
                .AsNoTracking()
                .Where(
                    x =>
                        x.Id_Hospitalizacion ==
                        idHospitalizacion
                )
                .OrderByDescending(
                    x => x.FechaMonitoreo
                )
                .ToListAsync();
        }

        // =====================================================
        // VALIDACIONES INTERNAS
        // =====================================================

        private async Task ValidarUsuario(Guid idUsuario)
        {
            var existe = await _context.Usuarios
                .AnyAsync(
                    x =>
                        x.IdUsuario == idUsuario
                        && x.Estado
                );

            if (!existe)
            {
                throw new InvalidOperationException(
                    "El usuario responsable no existe o está inactivo."
                );
            }
        }

        private async Task ValidarHospitalizacionActiva(
            Guid idHospitalizacion
        )
        {
            var existe = await _context
                .Hospitalizaciones
                .AnyAsync(
                    x =>
                        x.IdHospitalizacion ==
                        idHospitalizacion
                        && x.EstadoHospitalizacion == 0
                );

            if (!existe)
            {
                throw new InvalidOperationException(
                    "La hospitalización no existe o ya está cerrada."
                );
            }
        }
    }
}