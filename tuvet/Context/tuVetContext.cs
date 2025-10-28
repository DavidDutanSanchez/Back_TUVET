using Microsoft.EntityFrameworkCore;

namespace tu_vet_back.tuvet.Context
{
    public class TuVetContext(DbContextOptions<TuVetContext> options) : DbContext(options)
    {



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TuVetContext).Assembly);
        }
    }
}