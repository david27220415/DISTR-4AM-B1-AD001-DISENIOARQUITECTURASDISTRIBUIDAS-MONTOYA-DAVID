
using Microsoft.EntityFrameworkCore;
using Inventario.Api.Models;

namespace Inventario.Api.Data
{
    public class InventarioContext : DbContext
    {
        public InventarioContext(
            DbContextOptions<InventarioContext> options)
            : base(options)
        {
        }

        public DbSet<Bodega> Bodegas { get; set; }
        public DbSet<Existencia> Existencias { get; set; }
        public DbSet<MovimientoInventario> MovimientosInventario { get; set; }
        public DbSet<ReservaStock> ReservasStock { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Existencia>()
                .HasIndex(e => new { e.IdProducto, e.IdBodega })
                .IsUnique();

            modelBuilder.Entity<Existencia>()
                .HasOne<Bodega>()
                .WithMany()
                .HasForeignKey(e => e.IdBodega)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MovimientoInventario>()
                .HasOne<Bodega>()
                .WithMany()
                .HasForeignKey(m => m.IdBodega)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReservaStock>()
                .HasOne<Bodega>()
                .WithMany()
                .HasForeignKey(r => r.IdBodega)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
