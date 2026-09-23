namespace tu_vet_back.tuvet.Model.TuVet
{
    public class Mascotas
    {
        public Guid IdMascota { get; set; }

        public Guid Id_Persona { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public Guid Id_Color { get; set; }

        public bool Sexo { get; set; }

        public Guid Id_Especie { get; set; }

        public Guid Id_Raza { get; set; }

        public DateTime? FechaDeNacimiento { get; set; }

        public int? EdadAproximada { get; set; }

        public bool EdadEsAproximada { get; set; }

        public bool Esterilizado { get; set; }

        public string? CodigoMicrochip { get; set; }

        public string? EnfermedadesPreexistentes { get; set; }

        // 1 = Pequeño
        // 2 = Mediano
        // 3 = Grande
        // 4 = Gigante
        public int? Tamanio { get; set; }

        // Peso ingresado manualmente en kilogramos.
        public decimal? PesoKg { get; set; }

        // null = Sin registrar
        // false = No
        // true = Sí
        public bool? VacunasAlDia { get; set; }

        // 1 = Casera
        // 2 = Mixta
        // 3 = Balanceado
        public int? TipoAlimentacion { get; set; }

        public virtual Personas? Persona { get; set; }

        public virtual Colores? Color { get; set; }

        public virtual Especies? Especie { get; set; }

        public virtual Razas? Raza { get; set; }
    }
}