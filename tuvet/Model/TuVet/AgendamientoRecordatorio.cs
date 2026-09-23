namespace tu_vet_back.tuvet.Model.TuVet
{
    public class AgendamientoRecordatorio
    {
        public Guid IdRecordatorio { get; set; }
            = Guid.NewGuid();

        public Guid Id_Agendamiento { get; set; }

        // 1 = Recordatorio dos días antes
        public int TipoRecordatorio { get; set; } = 1;

        public DateTime FechaProgramada { get; set; }

        public DateTime? FechaEnvio { get; set; }

        public string? TelefonoDestino { get; set; }

        // 0 = Pendiente
        // 1 = Enviado
        // 2 = Error
        // 3 = Cancelado
        public int EstadoEnvio { get; set; } = 0;

        public string? IdMensajeProveedor { get; set; }

        public string? MensajeError { get; set; }

        public int Intentos { get; set; } = 0;

        public DateTime FechaCreacion { get; set; }
            = DateTime.Now;

        public virtual Agendamientos? Agendamiento
        {
            get;
            set;
        }
    }
}