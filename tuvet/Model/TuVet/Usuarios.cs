namespace tu_vet_back.tuvet.Model.TuVet
{
    public partial class Usuarios
    {
        public Guid IdUsuario { get; set; } = Guid.NewGuid();
        public string NombreUsuario { get; set; } = string.Empty;
        public byte[] Contrasenia { get; set; } = null!;
        public string Permisos { get; set; } = string.Empty;
        public Guid Id_Persona { get; set; } = Guid.NewGuid();
        public virtual Personas? Persona { get; set; } = null!;
    }
}