namespace tu_vet_back.tuvet.Model.TuVet
{
    public partial class Inventarios

    {
        public Guid IdInventario { get; set; } = Guid.NewGuid();
        public Guid Id_Producto { get; set; } = Guid.NewGuid();
        public DateTime FechaMovimiento { get; set; } = DateTime.Now;
        public int TipoMovimiento { get; set; } = 0;
        public int Cantidad { get; set; } = 0;
        public DateOnly FechaCaducidad { get; set; } = DateOnly.MinValue;
        public virtual Productos? Producto { get; set; } = null!;
    }
}