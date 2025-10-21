using Microsoft.EntityFrameworkCore;

namespace TodoApi
{
    public class TodoDbContext : DbContext
    {
        public DbSet<Todo> Todos => Set<Todo>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var file = Path.Combine(folder, "CaixaApi.db");
                       
            optionsBuilder.UseSqlite($"Data Source={file}");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
            .Property(t => t.Prioridade)
            .HasConversion<string>()
            .HasMaxLength(20);

            base.OnModelCreating(modelBuilder);
        }
    }
}
