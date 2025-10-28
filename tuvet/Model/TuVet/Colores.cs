namespace tu_vet_back.tuvet.Model.TuVet
{
    public partial class Colores
    {
        public Guid IdColor { get; set; } = Guid.NewGuid();
        public string NombreColor { get; set; } = string.Empty;
        public virtual List<Mascotas>? Mascotas { get; set; } = null!;
    }
}