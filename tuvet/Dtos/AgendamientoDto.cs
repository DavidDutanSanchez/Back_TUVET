namespace tu_vet_back.tuvet.Dtos
{
    public class AgendamientoDto
    {
        public Guid IdAgendamiento { get; set; }

        public DateTime FechaAgendamiento { get; set; }

        public DateTime FechaFin { get; set; }

        public int DuracionMinutos { get; set; }

        public int TipoAgendamiento { get; set; }


        // =====================================================
        // MASCOTA
        // =====================================================

        public Guid IdMascota { get; set; }

        public string NombreMascota { get; set; } = string.Empty;

        public string? EnfermedadesPreexistentes { get; set; }

        public bool Esterilizado { get; set; }

        public string? CodigoMicrochip { get; set; }

        public DateTime? FechaNacimientoMascota { get; set; }

        public int? EdadAproximadaMascota { get; set; }

        public bool EdadEsAproximada { get; set; }


        // =====================================================
        // PROPIETARIO
        // =====================================================

        public Guid IdPersona { get; set; }

        public string NombrePropietario { get; set; } = string.Empty;

        public string TelefonoPropietario { get; set; } = string.Empty;

        public string CorreoPropietario { get; set; } = string.Empty;


        // =====================================================
        // SERVICIO
        // =====================================================

        public Guid? IdServicio { get; set; }

        public string? NombreServicio { get; set; }

        public decimal? PrecioServicio { get; set; }


        // =====================================================
        // RESPONSABLE
        // =====================================================

        public Guid? IdUsuarioResponsable { get; set; }

        public string? NombreResponsable { get; set; }


        // =====================================================
        // ESTADO
        // =====================================================

        public int EstadoAgendamiento { get; set; }

        public string EstadoDescripcion { get; set; } = string.Empty;


        // =====================================================
        // PRIORIDAD
        // =====================================================

        public int Prioridad { get; set; }

        public string PrioridadDescripcion { get; set; } = string.Empty;


        // =====================================================
        // ORIGEN
        // =====================================================

        public int OrigenAgendamiento { get; set; }

        public string OrigenDescripcion { get; set; } = string.Empty;


        // =====================================================
        // INFORMACIÓN DE LA CITA
        // =====================================================

        public string? Motivo { get; set; }

        public string? Observaciones { get; set; }

        public bool RecordatorioEnviado { get; set; }


        // =====================================================
        // CONTROL DE TIEMPOS
        // =====================================================

        public DateTime? FechaConfirmacion { get; set; }

        public DateTime? FechaLlegada { get; set; }

        public DateTime? FechaInicioAtencion { get; set; }

        public DateTime? FechaFinAtencion { get; set; }

        public DateTime FechaCreacion { get; set; }


        // =====================================================
        // MÉTRICAS
        // =====================================================

        public int? MinutosEspera { get; set; }

        public int? MinutosAtencion { get; set; }
    }
}