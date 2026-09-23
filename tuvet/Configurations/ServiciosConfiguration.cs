using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Configurations
{
    public class ServiciosConfiguration
        : IEntityTypeConfiguration<Servicios>
    {
        public void Configure(
            EntityTypeBuilder<Servicios> entity)
        {
            entity.ToTable("servicios");

            entity.HasKey(
                e => e.IdServicios
            );

            entity.Property(
                    e => e.IdServicios
                )
                .HasColumnName("IdServicios");

            entity.Property(
                    e => e.NombreServicio
                )
                .HasColumnName("NombreServicio")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(
                    e => e.DescripcionServicio
                )
                .HasColumnName("DescripcionServicio")
                .HasMaxLength(255);

            entity.Property(
                    e => e.PreciosServicio
                )
                .HasColumnName("PreciosServicio")
                .HasPrecision(10, 2);

            entity.Property(
                    e => e.IncluyeIva
                )
                .HasColumnName("IncluyeIva");

            entity.Property(
                    e => e.DescuentoServicio
                )
                .HasColumnName("DescuentoServicio");

            entity.Property(
                    e => e.DuracionMinutos
                )
                .HasColumnName("DuracionMinutos");

            entity.Property(
                    e => e.TipoPrecio
                )
                .HasColumnName("TipoPrecio");

            entity.Property(
                    e => e.Activo
                )
                .HasColumnName("Activo");
        }
    }
}