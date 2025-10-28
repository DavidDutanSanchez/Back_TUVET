namespace tu_vet_back.tuvet.Model.TuVet
{
    public partial class Personas
    {
        public Guid IdPersona { get; set; } = Guid.NewGuid();
        public int TipoIdentificacion { get; set; } = 0;
        public string NumeroIdentificacion { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public int TipoTelefono { get; set; } = 0;
        public string Telefono { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public virtual List<Mascotas>? Mascotas { get; set; } = null!;
        public virtual List<Usuarios>? Usuarios { get; set; } = null!;
    }
}