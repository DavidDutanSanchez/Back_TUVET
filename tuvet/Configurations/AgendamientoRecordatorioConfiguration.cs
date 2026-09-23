using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Configurations
{
    public class AgendamientoRecordatorioConfiguration
        : IEntityTypeConfiguration<AgendamientoRecordatorio>
    {
        public void Configure(
            EntityTypeBuilder<AgendamientoRecordatorio> entity)
        {
            entity.ToTable(
                "agendamiento_recordatorios"
            );

            entity.HasKey(
                e => e.IdRecordatorio
            );

            entity.Property(
                    e => e.IdRecordatorio
                )
                .HasColumnName(
                    "IdRecordatorio"
                );

            entity.Property(
                    e => e.Id_Agendamiento
                )
                .HasColumnName(
                    "Id_Agendamiento"
                );

            entity.Property(
                    e => e.TipoRecordatorio
                )
                .HasColumnName(
                    "TipoRecordatorio"
                )
                .HasDefaultValue(1);

            entity.Property(
                    e => e.FechaProgramada
                )
                .HasColumnName(
                    "FechaProgramada"
                );

            entity.Property(
                    e => e.FechaEnvio
                )
                .HasColumnName(
                    "FechaEnvio"
                );

            entity.Property(
                    e => e.TelefonoDestino
                )
                .HasColumnName(
                    "TelefonoDestino"
                )
                .HasMaxLength(30);

            entity.Property(
                    e => e.EstadoEnvio
                )
                .HasColumnName(
                    "EstadoEnvio"
                )
                .HasDefaultValue(0);

            entity.Property(
                    e => e.IdMensajeProveedor
                )
                .HasColumnName(
                    "IdMensajeProveedor"
                )
                .HasMaxLength(255);

            entity.Property(
                    e => e.MensajeError
                )
                .HasColumnName(
                    "MensajeError"
                );

            entity.Property(
                    e => e.Intentos
                )
                .HasColumnName(
                    "Intentos"
                )
                .HasDefaultValue(0);

            entity.Property(
                    e => e.FechaCreacion
                )
                .HasColumnName(
                    "FechaCreacion"
                )
                .HasDefaultValueSql(
                    "CURRENT_TIMESTAMP"
                );

            entity.HasOne(
                    e => e.Agendamiento
                )
                .WithMany()
                .HasForeignKey(
                    e => e.Id_Agendamiento
                )
                .OnDelete(
                    DeleteBehavior.Restrict
                );
        }
    }
}