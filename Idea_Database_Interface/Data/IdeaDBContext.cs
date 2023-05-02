using Idea_Database_Interface.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Idea_Database_Interface.Data
{
    public class IdeaDBContext : IdentityDbContext<IdentityUser>
    {
        public IdeaDBContext(DbContextOptions<IdeaDBContext> options) : base(options) { }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Correspondencia> Correspondencia { get; set; }
        public DbSet<Emprendedores> Emprendedores { get; set; }
        public DbSet<Categoría> Categorías { get; set; }
        public DbSet<EmprendedoresCategoría> EmprendedoresCategorías { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("IdeaDatabase");
            modelBuilder.Entity<Empresa>().ToTable("Empresas");
            modelBuilder.Entity<Correspondencia>().ToTable("Correspondencias");
            modelBuilder.Entity<Emprendedores>().ToTable("Emprendedores");
            modelBuilder.Entity<Categoría>().ToTable("Categorías");
            modelBuilder.Entity<EmprendedoresCategoría>().ToTable("EmprendedoresCategorías");
            modelBuilder.Entity<EmprendedoresCategoría>()
                .HasOne(x => x.Emprendedores)
                .WithMany(p => p.Categorías)
                .HasForeignKey(x => x.IdCategoría)
                .IsRequired();
            modelBuilder.Entity<EmprendedoresCategoría>()
                .HasOne(x=>x.Categoría)
                .WithMany(p=>p.Emprendedores)
                .HasForeignKey(x=>x.IdCategoría)
                .IsRequired();
        }
    }
}
