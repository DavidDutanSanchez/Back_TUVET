namespace tu_vet_back.tuvet.Model.TuVet
{
    public partial class Productos
    {
        public Guid IdProductos { get; set; }
            = Guid.NewGuid();

        public string NombreProducto { get; set; }
            = string.Empty;

        public string? CodigProducto { get; set; }

        public string? DescripcionProducto { get; set; }

        public decimal PrecioVenta { get; set; }

        public int StockMinimo { get; set; }

        public int Unidad { get; set; }

        public Guid Id_Categoria { get; set; }

        public Guid? Id_CasaComercial { get; set; }

        public virtual Categorias? Categoria { get; set; }

        public virtual CasasComerciales? CasaComercial
        {
            get;
            set;
        }
    }
}