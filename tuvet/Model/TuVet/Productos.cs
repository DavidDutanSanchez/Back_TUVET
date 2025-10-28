namespace tu_vet_back.tuvet.Model.TuVet
{
    public partial class Productos

    {
        public Guid IdProductos { get; set; } = Guid.NewGuid();
        public string NombreProducto { get; set; } = string.Empty;
        public string? CodigProducto { get; set; } = string.Empty;
        public string? DescripcionProducto { get; set; } = string.Empty;
        public decimal PrecioVenta { get; set; } = 0;
        public int StockMinimo { get; set; } = 0;
        public int Unidad { get; set; } = 0;
        public Guid Id_Categoria { get; set; } = Guid.NewGuid();
        public virtual Categorias? Categoria { get; set; } = null!;
        //public virtual List<Agendamientos>? Agendamientos { get; set; } = null!;
    }
}