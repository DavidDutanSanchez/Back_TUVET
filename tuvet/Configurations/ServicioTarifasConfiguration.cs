using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Configurations
{
    public class ServicioTarifasConfiguration
        : IEntityTypeConfiguration<ServicioTarifas>
    {
        public void Configure(
            EntityTypeBuilder<ServicioTarifas> entity)
        {
            entity.ToTable(
                "servicio_tarifas"
            );

            entity.HasKey(
                e => e.IdServicioTarifa
            );

            entity.Property(
                    e => e.IdServicioTarifa
                )
                .HasColumnName(
                    "IdServicioTarifa"
                );

            entity.Property(
                    e => e.Id_Servicio
                )
                .HasColumnName(
                    "Id_Servicio"
                );

            entity.Property(
                    e => e.NombreTarifa
                )
                .HasColumnName(
                    "NombreTarifa"
                )
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(
                    e => e.PesoMinimo
                )
                .HasColumnName(
                    "PesoMinimo"
                )
                .HasPrecision(10, 2);

            entity.Property(
                    e => e.PesoMaximo
                )
                .HasColumnName(
                    "PesoMaximo"
                )
                .HasPrecision(10, 2);

            entity.Property(
                    e => e.Tamanio
                )
                .HasColumnName(
                    "Tamanio"
                );

            entity.Property(
                    e => e.Precio
                )
                .HasColumnName(
                    "Precio"
                )
                .HasPrecision(10, 2);

            entity.Property(
                    e => e.DuracionMinutos
                )
                .HasColumnName(
                    "DuracionMinutos"
                );

            entity.Property(
                    e => e.Activo
                )
                .HasColumnName(
                    "Activo"
                );

            entity.HasOne(
                    e => e.Servicio
                )
                .WithMany(
                    e => e.Tarifas
                )
                .HasForeignKey(
                    e => e.Id_Servicio
                )
                .OnDelete(
                    DeleteBehavior.Restrict
                );
        }
    }
}