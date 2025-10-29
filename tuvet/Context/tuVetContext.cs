using Microsoft.EntityFrameworkCore;
using tu_vet_back.tuvet.Model.TuVet;

namespace tu_vet_back.tuvet.Context
{
    public class TuVetContext(DbContextOptions<TuVetContext> options) : DbContext(options)
    {
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Colores> Colores { get; set; }
        public DbSet<Especies> Especies { get; set; }
        public DbSet<Inventarios> Inventarios { get; set; }
        public DbSet<Mascotas> Mascotas { get; set; }
        public DbSet<Personas> Personas { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Razas> Razas { get; set; }
        public DbSet<Servicios> Servicios { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TuVetContext).Assembly);
        }
    }
}