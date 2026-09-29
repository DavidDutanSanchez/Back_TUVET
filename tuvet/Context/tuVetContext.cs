using Microsoft.EntityFrameworkCore;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Context
{
    public class TuVetContext(
        DbContextOptions<TuVetContext> options)
        : DbContext(options)
    {
        // =====================================================
        // MÓDULOS EXISTENTES
        // =====================================================

        public DbSet<Categorias> Categorias { get; set; }

        public DbSet<Colores> Colores { get; set; }

        public DbSet<Especies> Especies { get; set; }

        public DbSet<Inventarios> Inventarios { get; set; }

        public DbSet<Mascotas> Mascotas { get; set; }

        public DbSet<Personas> Personas { get; set; }

        public DbSet<Productos> Productos { get; set; }

        public DbSet<Razas> Razas { get; set; }

        public DbSet<Servicios> Servicios { get; set; }

        public DbSet<ServicioTarifas> ServicioTarifas { get; set; }

        public DbSet<Usuarios> Usuarios { get; set; }

        public DbSet<Agendamientos> Agendamientos { get; set; }

        public DbSet<CasasComerciales> CasasComerciales { get; set; }

        public DbSet<AgendamientoRecordatorio>
            AgendamientoRecordatorios { get; set; }


        // =====================================================
        // HISTORIAL CLÍNICO
        // =====================================================

        public DbSet<HistorialesClinicos>
            HistorialesClinicos { get; set; }

        public DbSet<AtencionesClinicas>
            AtencionesClinicas { get; set; }


        // =====================================================
        // HOSPITALIZACIÓN
        // =====================================================

        public DbSet<Hospitalizaciones>
            Hospitalizaciones { get; set; }

        public DbSet<HospitalizacionTratamientos>
            HospitalizacionTratamientos { get; set; }

        public DbSet<HospitalizacionAdministraciones>
            HospitalizacionAdministraciones { get; set; }

        public DbSet<HospitalizacionMonitoreos>
            HospitalizacionMonitoreos { get; set; }


        // =====================================================
        // CONFIGURACIÓN DE ENTITY FRAMEWORK
        // =====================================================

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Conservar todas las configuraciones
            // que ya existen en el proyecto.

            modelBuilder
                .ApplyConfigurationsFromAssembly(
                    typeof(TuVetContext).Assembly
                );


            // =================================================
            // HISTORIALES CLÍNICOS
            // =================================================

            modelBuilder.Entity<HistorialesClinicos>(
                entity =>
                {
                    entity.ToTable(
                        "historiales_clinicos"
                    );

                    entity.HasKey(
                        x => x.IdHistorialClinico
                    );

                    entity.HasIndex(
                        x => x.Id_Mascota
                    ).IsUnique();
                }
            );


            // =================================================
            // ATENCIONES CLÍNICAS
            // =================================================

            modelBuilder.Entity<AtencionesClinicas>(
                entity =>
                {
                    entity.ToTable(
                        "atenciones_clinicas"
                    );

                    entity.HasKey(
                        x => x.IdAtencionClinica
                    );
                }
            );


            // =================================================
            // HOSPITALIZACIONES
            // =================================================

            modelBuilder.Entity<Hospitalizaciones>(
                entity =>
                {
                    entity.ToTable(
                        "hospitalizaciones"
                    );

                    entity.HasKey(
                        x => x.IdHospitalizacion
                    );
                }
            );


            // =================================================
            // TRATAMIENTOS
            // =================================================

            modelBuilder.Entity<HospitalizacionTratamientos>(
                entity =>
                {
                    entity.ToTable(
                        "hospitalizacion_tratamientos"
                    );

                    entity.HasKey(
                        x => x.IdTratamiento
                    );
                }
            );


            // =================================================
            // ADMINISTRACIÓN DE MEDICAMENTOS
            // =================================================

            modelBuilder.Entity<HospitalizacionAdministraciones>(
                entity =>
                {
                    entity.ToTable(
                        "hospitalizacion_administraciones"
                    );

                    entity.HasKey(
                        x => x.IdAdministracion
                    );
                }
            );


            // =================================================
            // MONITOREOS
            // =================================================

            modelBuilder.Entity<HospitalizacionMonitoreos>(
                entity =>
                {
                    entity.ToTable(
                        "hospitalizacion_monitoreos"
                    );

                    entity.HasKey(
                        x => x.IdMonitoreo
                    );
                }
            );
        }
    }
}