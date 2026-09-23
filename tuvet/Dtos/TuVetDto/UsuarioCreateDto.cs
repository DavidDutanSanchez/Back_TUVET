namespace tu_vet_back.tuvet.Dtos.TuVetDto
{
    public class UsuarioCreateDto
    {
        public string NombreUsuario { get; set; } = string.Empty;

        public string Contrasenia { get; set; } = string.Empty;

        public string Permisos { get; set; } = string.Empty;

        public bool Estado { get; set; } = true;

        public Guid IdPersona { get; set; }

        public string? FotoPerfil { get; set; }
    }
}