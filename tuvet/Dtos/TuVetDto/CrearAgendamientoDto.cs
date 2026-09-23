namespace tu_vet_back.tuvet.Dtos.TuVetDto
{
    public class CrearAgendamientoDto
    {
        public DateTime FechaAgendamiento { get; set; }

        public int DuracionMinutos { get; set; } = 30;

        public int TipoAgendamiento { get; set; } = 0;

        public Guid Id_Mascota { get; set; }

        public Guid? Id_Servicio { get; set; }

        public Guid? Id_UsuarioResponsable { get; set; }

        public int Prioridad { get; set; } = 0;

        public int OrigenAgendamiento { get; set; } = 0;

        public string? Motivo { get; set; }

        public string? Observaciones { get; set; }
    }
}