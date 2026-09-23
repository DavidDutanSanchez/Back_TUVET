namespace tu_vet_back.tuvet.Dtos.TuVetDto
{
    public class UsuarioListadoDto
    {
        public Guid IdUsuario { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string Permisos { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public Guid IdPersona { get; set; }

        public string? FotoPerfil { get; set; }

        public string Nombres { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public string NumeroIdentificacion { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string CorreoElectronico { get; set; } = string.Empty;
    }
}