namespace tu_vet_back.tuvet.Dtos.TuVetDto
{
    public class AgendamientoListadoDto
    {
        public Guid IdAgendamiento { get; set; }

        public DateTime FechaAgendamiento { get; set; }

        public int DuracionMinutos { get; set; }

        public int TipoAgendamiento { get; set; }

        public Guid Id_Mascota { get; set; }

        public string NombreMascota { get; set; }
            = string.Empty;

        public Guid Id_Persona { get; set; }

        public string NombrePropietario { get; set; }
            = string.Empty;

        public Guid? Id_Servicio { get; set; }

        public string? NombreServicio { get; set; }

        public Guid? Id_UsuarioResponsable { get; set; }

        public int EstadoAgendamiento { get; set; }

        public int Prioridad { get; set; }

        public int OrigenAgendamiento { get; set; }

        public string? Motivo { get; set; }

        public string? Observaciones { get; set; }
    }
}