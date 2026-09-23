using Microsoft.EntityFrameworkCore;
using tu_vet_back.tuvet.Context;
using tu_vet_back.tuvet.Dtos.TuVetDto;
using tu_vet_back.tuvet.Interface;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Service
{
    public class AgendamientosService
        : IControladorAgendamientos
    {
        private readonly TuVetContext _context;

        public AgendamientosService(
            TuVetContext context)
        {
            _context = context;
        }

        public async Task<List<AgendamientoListadoDto>>
    ObtenerPorRango(
        DateTime desde,
        DateTime hasta)
{
    DateTime inicio = desde.Date;

    // El límite final es exclusivo.
    DateTime fin = hasta.Date.AddDays(1);

    if (fin <= inicio)
    {
        throw new ArgumentException(
            "El rango de fechas no es válido.");
    }

    return await _context.Agendamientos
        .AsNoTracking()
        .Where(x =>
            x.FechaAgendamiento >= inicio &&
            x.FechaAgendamiento < fin)
        .OrderBy(x => x.FechaAgendamiento)
        .Select(x => new AgendamientoListadoDto
        {
            IdAgendamiento = x.IdAgendamiento,
            FechaAgendamiento =
                x.FechaAgendamiento,
            DuracionMinutos =
                x.DuracionMinutos,
            TipoAgendamiento =
                x.TipoAgendamiento,
            Id_Mascota = x.Id_Mascota,
            NombreMascota =
                x.Mascota != null
                    ? x.Mascota.Nombre
                    : "",
            Id_Persona = x.Id_Persona,
            NombrePropietario =
                x.Persona != null
                    ? x.Persona.Nombres + " " +
                      x.Persona.Apellidos
                    : "",
            Id_Servicio = x.Id_Servicio,
            NombreServicio =
                x.Servicio != null
                    ? x.Servicio.NombreServicio
                    : null,
            Id_UsuarioResponsable =
                x.Id_UsuarioResponsable,
            EstadoAgendamiento =
                x.EstadoAgendamiento,
            Prioridad = x.Prioridad,
            OrigenAgendamiento =
                x.OrigenAgendamiento,
            Motivo = x.Motivo,
            Observaciones =
                x.Observaciones
        })
        .ToListAsync();
}

        public async Task<List<AgendamientoListadoDto>>
            ObtenerPorFecha(DateTime fecha)
        {
            DateTime inicio = fecha.Date;
            DateTime fin = inicio.AddDays(1);

            return await _context.Agendamientos
                .AsNoTracking()
                .Where(x =>
                    x.FechaAgendamiento >= inicio &&
                    x.FechaAgendamiento < fin)
                .OrderBy(x => x.FechaAgendamiento)
                .Select(x => new AgendamientoListadoDto
                {
                    IdAgendamiento = x.IdAgendamiento,
                    FechaAgendamiento =
                        x.FechaAgendamiento,
                    DuracionMinutos =
                        x.DuracionMinutos,
                    TipoAgendamiento =
                        x.TipoAgendamiento,
                    Id_Mascota = x.Id_Mascota,
                    NombreMascota =
                        x.Mascota != null
                            ? x.Mascota.Nombre
                            : "",
                    Id_Persona = x.Id_Persona,
                    NombrePropietario =
                        x.Persona != null
                            ? x.Persona.Nombres + " " +
                              x.Persona.Apellidos
                            : "",
                    Id_Servicio = x.Id_Servicio,
                    NombreServicio =
                        x.Servicio != null
                            ? x.Servicio.NombreServicio
                            : null,
                    Id_UsuarioResponsable =
                        x.Id_UsuarioResponsable,
                    EstadoAgendamiento =
                        x.EstadoAgendamiento,
                    Prioridad = x.Prioridad,
                    OrigenAgendamiento =
                        x.OrigenAgendamiento,
                    Motivo = x.Motivo,
                    Observaciones =
                        x.Observaciones
                })
                .ToListAsync();
        }

        public async Task<Guid> Crear(
            CrearAgendamientoDto datos)
        {
            if (datos.FechaAgendamiento == default)
            {
                throw new ArgumentException(
                    "Seleccione una fecha y hora.");
            }

            if (datos.FechaAgendamiento <= DateTime.Now)
            {
                throw new ArgumentException(
                    "La cita debe tener una fecha futura.");
            }

            var mascota =
                await _context.Mascotas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.IdMascota == datos.Id_Mascota);

            if (mascota == null)
            {
                throw new ArgumentException(
                    "La mascota no existe.");
            }

            int duracion = datos.DuracionMinutos;

            if (datos.Id_Servicio.HasValue)
            {
                var servicio =
                    await _context.Servicios
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x =>
                            x.IdServicios ==
                            datos.Id_Servicio.Value);

                if (servicio == null)
                {
                    throw new ArgumentException(
                        "El servicio no existe.");
                }

                if (servicio.DuracionMinutos > 0)
                {
                    duracion =
                        servicio.DuracionMinutos;
                }
            }

            if (duracion <= 0)
            {
                throw new ArgumentException(
                    "La duración debe ser mayor a cero.");
            }

            DateTime inicio =
                datos.FechaAgendamiento;

            DateTime fin =
                inicio.AddMinutes(duracion);

            // Consultamos las citas potencialmente
            // coincidentes para evaluar sus intervalos.
            DateTime limiteAnterior =
                inicio.AddDays(-1);

            var citas =
                await _context.Agendamientos
                    .AsNoTracking()
                    .Where(x =>
                        x.FechaAgendamiento >=
                            limiteAnterior &&
                        x.FechaAgendamiento < fin &&
                        x.EstadoAgendamiento != 6 &&
                        x.EstadoAgendamiento != 7 &&
                        x.EstadoAgendamiento != 8)
                    .Select(x => new
                    {
                        x.FechaAgendamiento,
                        x.DuracionMinutos,
                        x.Id_Mascota,
                        x.Id_UsuarioResponsable
                    })
                    .ToListAsync();

            bool existeCruce = citas.Any(x =>
            {
                DateTime finExistente =
                    x.FechaAgendamiento
                        .AddMinutes(x.DuracionMinutos);

                bool seCruzan =
                    inicio < finExistente &&
                    x.FechaAgendamiento < fin;

                bool mismaMascota =
                    x.Id_Mascota ==
                    datos.Id_Mascota;

                bool mismoResponsable =
                    datos.Id_UsuarioResponsable
                        .HasValue &&
                    x.Id_UsuarioResponsable ==
                    datos.Id_UsuarioResponsable;

                return seCruzan &&
                    (mismaMascota ||
                     mismoResponsable);
            });

            if (existeCruce)
            {
                throw new InvalidOperationException(
                    "Existe otra cita en ese horario para la mascota o el responsable.");
            }

            var cita = new Agendamientos
            {
                IdAgendamiento =
                    Guid.NewGuid(),

                FechaAgendamiento =
                    datos.FechaAgendamiento,

                DuracionMinutos =
                    duracion,

                TipoAgendamiento =
                    datos.TipoAgendamiento,

                Id_Mascota =
                    mascota.IdMascota,

                Id_Persona =
                    mascota.Id_Persona,

                Id_Servicio =
                    datos.Id_Servicio,

                Id_UsuarioResponsable =
                    datos.Id_UsuarioResponsable,

                EstadoAgendamiento = 0,

                Prioridad =
                    datos.Prioridad,

                OrigenAgendamiento =
                    datos.OrigenAgendamiento,

                Motivo =
                    datos.Motivo?.Trim(),

                Observaciones =
                    datos.Observaciones?.Trim(),

                FechaCreacion =
                    DateTime.Now
            };

            _context.Agendamientos.Add(cita);

            await _context.SaveChangesAsync();

            return cita.IdAgendamiento;
        }

        public async Task Confirmar(Guid id)
        {
            var cita = await Buscar(id);

            if (cita.EstadoAgendamiento != 0)
            {
                throw new InvalidOperationException(
                    "Solo se pueden confirmar citas pendientes.");
            }

            cita.EstadoAgendamiento = 1;
            cita.FechaConfirmacion = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task RegistrarLlegada(Guid id)
        {
            var cita = await Buscar(id);

            if (
                cita.EstadoAgendamiento != 0 &&
                cita.EstadoAgendamiento != 1
            )
            {
                throw new InvalidOperationException(
                    "La cita no admite registrar llegada en su estado actual.");
            }

            cita.EstadoAgendamiento = 2;
            cita.FechaLlegada = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task Cancelar(Guid id)
        {
            var cita = await Buscar(id);

            if (
                cita.EstadoAgendamiento >= 4 &&
                cita.EstadoAgendamiento <= 5
            )
            {
                throw new InvalidOperationException(
                    "No se puede cancelar una atención iniciada o finalizada.");
            }

            cita.EstadoAgendamiento = 6;

            await _context.SaveChangesAsync();
        }

        public async Task Reprogramar(
            Guid id,
            DateTime nuevaFecha)
        {
            var cita = await Buscar(id);

            if (
                cita.EstadoAgendamiento == 4 ||
                cita.EstadoAgendamiento == 5 ||
                cita.EstadoAgendamiento == 6 ||
                cita.EstadoAgendamiento == 8
            )
            {
                throw new InvalidOperationException(
                    "Esta cita no se puede reprogramar.");
            }

            if (nuevaFecha <= DateTime.Now)
            {
                throw new ArgumentException(
                    "La nueva fecha debe ser futura.");
            }

            DateTime fin =
                nuevaFecha.AddMinutes(
                    cita.DuracionMinutos);

            DateTime limiteAnterior =
                nuevaFecha.AddDays(-1);

            var otrasCitas =
                await _context.Agendamientos
                    .AsNoTracking()
                    .Where(x =>
                        x.IdAgendamiento != id &&
                        x.FechaAgendamiento >=
                            limiteAnterior &&
                        x.FechaAgendamiento < fin &&
                        x.EstadoAgendamiento != 6 &&
                        x.EstadoAgendamiento != 7 &&
                        x.EstadoAgendamiento != 8)
                    .Select(x => new
                    {
                        x.FechaAgendamiento,
                        x.DuracionMinutos,
                        x.Id_Mascota,
                        x.Id_UsuarioResponsable
                    })
                    .ToListAsync();

            bool cruce = otrasCitas.Any(x =>
            {
                DateTime finExistente =
                    x.FechaAgendamiento
                        .AddMinutes(x.DuracionMinutos);

                return nuevaFecha < finExistente &&
                    x.FechaAgendamiento < fin &&
                    (
                        x.Id_Mascota ==
                            cita.Id_Mascota ||
                        (
                            cita.Id_UsuarioResponsable
                                .HasValue &&
                            x.Id_UsuarioResponsable ==
                                cita.Id_UsuarioResponsable
                        )
                    );
            });

            if (cruce)
            {
                throw new InvalidOperationException(
                    "El nuevo horario tiene un cruce de citas.");
            }

            cita.FechaAgendamiento = nuevaFecha;
            cita.EstadoAgendamiento = 0;
            cita.FechaConfirmacion = null;
            cita.FechaLlegada = null;

            await _context.SaveChangesAsync();
        }

        private async Task<Agendamientos>
            Buscar(Guid id)
        {
            return await _context.Agendamientos
                .FirstOrDefaultAsync(x =>
                    x.IdAgendamiento == id)
                ?? throw new KeyNotFoundException(
                    "La cita no existe.");
        }
    }
}