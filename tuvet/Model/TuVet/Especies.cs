namespace tu_vet_back.tuvet.Model.TuVet
{
    public partial class Especies
    {
        public Guid IdEspecies { get; set; } = Guid.NewGuid();
        public string NombreEspecie { get; set; } = string.Empty;
        public virtual List<Mascotas>? Mascotas { get; set; } = null!;
    }
}