
using Microsoft.EntityFrameworkCore;
using Pedidos.Api.Models;

namespace Pedidos.Api.Data
{
    public class PedidosContext : DbContext
    {
        public PedidosContext(
            DbContextOptions<PedidosContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Cedula)
                .IsUnique();

            modelBuilder.Entity<Pedido>()
                .HasOne<Cliente>()
                .WithMany()
                .HasForeignKey(p => p.IdCliente)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DetallePedido>()
                .HasOne<Pedido>()
                .WithMany()
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
