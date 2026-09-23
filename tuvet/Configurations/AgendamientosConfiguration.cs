using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Configurations
{
    public class AgendamientosConfiguration
        : IEntityTypeConfiguration<Agendamientos>
    {
        public void Configure(
            EntityTypeBuilder<Agendamientos> entity)
        {
            entity.ToTable("agendamientos");

            entity.HasKey(e => e.IdAgendamiento);

            entity.Property(e => e.IdAgendamiento)
                .HasColumnName("IdAgendamiento");

            entity.Property(e => e.FechaAgendamiento)
                .HasColumnName("FechaAgendamiento");

            entity.Property(e => e.DuracionMinutos)
                .HasColumnName("DuracionMinutos")
                .HasDefaultValue(30);

            entity.Property(e => e.TipoAgendamiento)
                .HasColumnName("TipoAgendamiento");

            entity.Property(e => e.Id_Mascota)
                .HasColumnName("Id_Mascota");

            entity.Property(e => e.Id_Persona)
                .HasColumnName("Id_Persona");

            entity.Property(e => e.Id_Servicio)
                .HasColumnName("Id_Servicio");

            entity.Property(e => e.Id_UsuarioResponsable)
                .HasColumnName("Id_UsuarioResponsable");

            entity.Property(e => e.EstadoAgendamiento)
                .HasColumnName("EstadoAgendamiento");

            entity.Property(e => e.Prioridad)
                .HasColumnName("Prioridad")
                .HasDefaultValue(0);

            entity.Property(e => e.OrigenAgendamiento)
                .HasColumnName("OrigenAgendamiento")
                .HasDefaultValue(0);

            entity.Property(e => e.Motivo)
                .HasColumnName("Motivo")
                .HasMaxLength(500);

            entity.Property(e => e.Observaciones)
                .HasColumnName("Observaciones");

            entity.Property(e => e.RecordatorioEnviado)
                .HasColumnName("RecordatorioEnviado")
                .HasDefaultValue(false);

            entity.Property(e => e.FechaConfirmacion)
                .HasColumnName("FechaConfirmacion");

            entity.Property(e => e.FechaLlegada)
                .HasColumnName("FechaLlegada");

            entity.Property(e => e.FechaInicioAtencion)
                .HasColumnName("FechaInicioAtencion");

            entity.Property(e => e.FechaFinAtencion)
                .HasColumnName("FechaFinAtencion");

            entity.Property(e => e.FechaCreacion)
                .HasColumnName("FechaCreacion")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");


            // =================================================
            // MASCOTA
            // =================================================

            entity.HasOne(e => e.Mascota)
                .WithMany()
                .HasForeignKey(e => e.Id_Mascota)
                .OnDelete(DeleteBehavior.Restrict);


            // =================================================
            // PROPIETARIO
            // =================================================

            entity.HasOne(e => e.Persona)
                .WithMany()
                .HasForeignKey(e => e.Id_Persona)
                .OnDelete(DeleteBehavior.Restrict);


            // =================================================
            // SERVICIO
            // =================================================

            entity.HasOne(e => e.Servicio)
                .WithMany()
                .HasForeignKey(e => e.Id_Servicio)
                .OnDelete(DeleteBehavior.Restrict);


            // =================================================
            // RESPONSABLE
            // =================================================

            entity.HasOne(e => e.UsuarioResponsable)
                .WithMany()
                .HasForeignKey(e => e.Id_UsuarioResponsable)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}