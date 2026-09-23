namespace tu_vet_back.tuvet.Dtos
{
    public class ReprogramarAgendamientoDto
    {
        public DateTime NuevaFecha { get; set; }

        public int? DuracionMinutos { get; set; }

        public Guid? IdUsuarioResponsable { get; set; }

        public string? Motivo { get; set; }
    }
}