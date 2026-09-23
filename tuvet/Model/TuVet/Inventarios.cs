namespace tu_vet_back.tuvet.Model.TuVet
{
    public partial class Inventarios
    {
        public Guid IdInventario { get; set; } = Guid.NewGuid();

        public Guid Id_Producto { get; set; }

        public DateTime FechaMovimiento { get; set; }

        public int TipoMovimiento { get; set; }

        public int Cantidad { get; set; }

        public DateOnly? FechaCaducidad { get; set; }

        public virtual Productos? Producto { get; set; }
    }
}