namespace tu_vet_back.tuvet.Model.TuVet
{
    public partial class Mascotas
    {
        public Guid IdMascota { get; set; } = Guid.NewGuid();
        public Guid Id_Persona { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty;
        public Guid Id_Color { get; set; } = Guid.NewGuid();
        public bool Sexo { get; set; } = true;
        public Guid Id_Especie { get; set; } = Guid.NewGuid();
        public Guid Id_Raza { get; set; } = Guid.NewGuid();
        public DateOnly FechaDeNacimiento { get; set; } = DateOnly.MinValue;
        public bool Esterilizado { get; set; } = false;
        public string? CodigoMicrochip { get; set; } = string.Empty;
        public virtual Personas? Persona { get; set; } = null!;
        public virtual Especies? Especie { get; set; } = null!;
        public virtual Colores? Color { get; set; } = null!;
        public virtual Razas? Raza { get; set; } = null!;
    }
}