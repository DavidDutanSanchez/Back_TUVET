namespace tu_vet_back.tuvet.Model.TuVet
{
    public class Agendamientos
    {
        public Guid IdAgendamiento { get; set; }
            = Guid.NewGuid();

        public DateTime FechaAgendamiento { get; set; }

        public int DuracionMinutos { get; set; } = 30;

        public int TipoAgendamiento { get; set; }

        public Guid Id_Mascota { get; set; }

        public Guid Id_Persona { get; set; }

        public Guid? Id_Servicio { get; set; }

        public Guid? Id_UsuarioResponsable { get; set; }

        public int EstadoAgendamiento { get; set; } = 0;

        public int Prioridad { get; set; } = 0;

        public int OrigenAgendamiento { get; set; } = 0;

        public string? Motivo { get; set; }

        public string? Observaciones { get; set; }

        public bool RecordatorioEnviado { get; set; }

        public DateTime? FechaConfirmacion { get; set; }

        public DateTime? FechaLlegada { get; set; }

        public DateTime? FechaInicioAtencion { get; set; }

        public DateTime? FechaFinAtencion { get; set; }

        public DateTime FechaCreacion { get; set; }
            = DateTime.Now;

        public virtual Mascotas? Mascota { get; set; }

        public virtual Personas? Persona { get; set; }

        public virtual Servicios? Servicio { get; set; }

        public virtual Usuarios? UsuarioResponsable { get; set; }
    }
}