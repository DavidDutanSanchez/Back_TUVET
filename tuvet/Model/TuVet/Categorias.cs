namespace tu_vet_back.tuvet.Model.TuVet
{
    public partial class Categorias

    {
        public Guid IdCategoria { get; set; } = Guid.NewGuid();
        public string NombreCategoria { get; set; } = string.Empty;
        public virtual List<Productos>? Productos { get; set; } = null!;

    }
}