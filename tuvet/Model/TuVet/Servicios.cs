namespace tu_vet_back.tuvet.Model.TuVet
{
    public partial class Servicios

    {
        public Guid IdServicios { get; set; } = Guid.NewGuid();
        public string NombreServicio { get; set; } = string.Empty;
        public string? DescripcionServicio { get; set; } = string.Empty;
        public decimal? PreciosServicio { get; set; } = 0;
        public bool IncluyeIva { get; set; } = false;
        public int DescuentoServicio { get; set; } = 0;
    }
}