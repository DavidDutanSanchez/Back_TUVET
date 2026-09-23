namespace tu_vet_back.tuvet.Model.TuVet
{
    public partial class Usuarios
    {
        public Guid IdUsuario { get; set; } = Guid.NewGuid();

        public string NombreUsuario { get; set; } = string.Empty;

        public byte[] Contrasenia { get; set; } = Array.Empty<byte>();

        public string Permisos { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public Guid Id_Persona { get; set; }

        public string? FotoPerfil { get; set; }

        public virtual Personas? Persona { get; set; }
    }
}