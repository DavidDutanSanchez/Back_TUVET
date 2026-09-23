using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Configurations
{
    public class CasasComercialesConfiguration
        : IEntityTypeConfiguration<CasasComerciales>
    {
        public void Configure(
            EntityTypeBuilder<CasasComerciales> entity)
        {
            entity.ToTable("casas_comerciales");

            entity.HasKey(e => e.IdCasaComercial);

            entity.Property(e => e.IdCasaComercial)
                .HasColumnName("IdCasaComercial")
                .HasMaxLength(36);

            entity.Property(e => e.NombreCasaComercial)
                .HasColumnName("NombreCasaComercial")
                .HasMaxLength(255)
                .IsRequired();

            entity.HasIndex(e => e.NombreCasaComercial)
                .IsUnique();
        }
    }
}