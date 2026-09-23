namespace tu_vet_back.tuvet.Dtos.TuVetDto
{
    public class UsuarioUpdateDto
    {
        public Guid IdUsuario { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string? Contrasenia { get; set; }

        public string Permisos { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public Guid IdPersona { get; set; }

        public string? FotoPerfil { get; set; }
    }
}