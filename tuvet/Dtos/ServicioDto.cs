namespace tu_vet_back.tuvet.Dtos
{
    public class ServicioTarifaDto
    {
        public Guid IdServicioTarifa { get; set; }

        public Guid IdServicio { get; set; }

        public string NombreTarifa { get; set; }
            = string.Empty;

        public decimal? PesoMinimo { get; set; }

        public decimal? PesoMaximo { get; set; }

        public int? Tamanio { get; set; }

        public string TamanioDescripcion { get; set; }
            = string.Empty;

        public decimal Precio { get; set; }

        public int? DuracionMinutos { get; set; }

        public bool Activo { get; set; }
    }


    public class ServicioDto
    {
        public Guid IdServicios { get; set; }

        public string NombreServicio { get; set; }
            = string.Empty;

        public string? DescripcionServicio { get; set; }

        public decimal PreciosServicio { get; set; }

        public bool IncluyeIva { get; set; }

        public int DescuentoServicio { get; set; }

        public int DuracionMinutos { get; set; }

        public int TipoPrecio { get; set; }

        public string TipoPrecioDescripcion { get; set; }
            = string.Empty;

        public bool Activo { get; set; }

        public List<ServicioTarifaDto>
            Tarifas { get; set; } = new();
    }


    public class GuardarServicioTarifaDto
    {
        public Guid? IdServicioTarifa { get; set; }

        public string NombreTarifa { get; set; }
            = string.Empty;

        public decimal? PesoMinimo { get; set; }

        public decimal? PesoMaximo { get; set; }

        public int? Tamanio { get; set; }

        public decimal Precio { get; set; }

        public int? DuracionMinutos { get; set; }

        public bool Activo { get; set; } = true;
    }


    public class GuardarServicioDto
    {
        public Guid? IdServicios { get; set; }

        public string NombreServicio { get; set; }
            = string.Empty;

        public string? DescripcionServicio { get; set; }

        public decimal PreciosServicio { get; set; }

        public bool IncluyeIva { get; set; }

        public int DescuentoServicio { get; set; }

        public int DuracionMinutos { get; set; } = 30;

        public int TipoPrecio { get; set; }

        public bool Activo { get; set; } = true;

        public List<GuardarServicioTarifaDto>
            Tarifas { get; set; } = new();
    }
}