using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Configurations
{
    public class ProductosConfiguration
        : IEntityTypeConfiguration<Productos>
    {
        public void Configure(
            EntityTypeBuilder<Productos> entity)
        {
            entity.ToTable("productos");

            entity.HasKey(e => e.IdProductos);

            entity.Property(e => e.IdProductos)
                .HasColumnName("IdProductos");

            entity.Property(e => e.NombreProducto)
                .HasColumnName("NombreProducto")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.CodigProducto)
                .HasColumnName("CodigProducto")
                .HasMaxLength(255);

            entity.Property(e => e.DescripcionProducto)
                .HasColumnName("DescripcionProducto");

            entity.Property(e => e.PrecioVenta)
                .HasColumnName("PrecioVenta")
                .HasPrecision(10, 2);

            entity.Property(e => e.StockMinimo)
                .HasColumnName("StockMinimo");

            entity.Property(e => e.Unidad)
                .HasColumnName("Unidad");

            entity.Property(e => e.Id_Categoria)
                .HasColumnName("Id_Categoria");

            entity.Property(e => e.Id_CasaComercial)
                .HasColumnName("Id_CasaComercial");

            entity.HasOne(e => e.Categoria)
                .WithMany()
                .HasForeignKey(e => e.Id_Categoria)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CasaComercial)
                .WithMany(e => e.Productos)
                .HasForeignKey(e => e.Id_CasaComercial)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}