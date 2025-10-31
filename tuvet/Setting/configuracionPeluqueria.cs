using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace tuvet.Setting
{
    public class ClientConfiguration : IEntityTypeConfiguration<Personas>
    {
        public void Configure(EntityTypeBuilder<Personas> builder)
        {
            _ = builder.HasKey(e => e.IdPersona);
            _ = builder.ToTable("personas");
            _ = builder.HasIndex(e => e.CedulaPersona, "cedulaPersona");
            _ = builder.HasIndex(e => e.NombresPersona, "NombresPersona");
            _ = builder.HasIndex(e => e.ApellidosPersona, "apellidosPersona");
            _ = builder.HasIndex(e => e.IdPersona, "idPersona");
            _ = builder.Property(e => e.IdPersona)
                .HasMaxLength(36)
                .HasColumnName("idPersona");
            _ = builder.Property(e => e.CedulaPersona)
                .HasMaxLength(13)
                .HasColumnName("cedulaPersona");
            _ = builder.Property(e => e.NombresPersona)
                .HasMaxLength(255)
                .HasColumnName("nombresPersona");
            _ = builder.Property(e => e.ApellidosPersona)
                .HasMaxLength(255)
                .HasColumnName("apellidosPersona");
            _ = builder.Property(e => e.DireccionPersona)
                .HasColumnName("direccionPersona");
            _ = builder.Property(e => e.FechaNacimientoPersona)
                .HasColumnType("datetime")
                .HasColumnName("fechaNacimientoPersona");
            _ = builder.Property(e => e.CelularPersona)
                .HasMaxLength(20)
                .HasColumnName("celularPersona");
            _ = builder.Property(e => e.CorreoPersona)
                .HasMaxLength(255)
                .HasColumnName("correoPersona");
        }
    }
}