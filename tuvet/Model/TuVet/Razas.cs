namespace tu_vet_back.tuvet.Model.TuVet
{
    public partial class Razas
    {
        public Guid IdRaza { get; set; } = Guid.NewGuid();
        public string NombreRaza { get; set; } = string.Empty;
        public virtual List<Mascotas>? Mascotas { get; set; } = null!;
    }
}