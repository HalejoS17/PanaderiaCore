namespace PanaderiaCore.Data
{
    using Microsoft.EntityFrameworkCore;
    using PanaderiaCore.Models;

    public class PanaderiaDbContext : DbContext
    {
        public PanaderiaDbContext(DbContextOptions<PanaderiaDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompra> DetalleCompras { get; set; }
        public DbSet<Reabastecimiento> Reabastecimientos { get; set; } // Nuevo




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Producto>()
                .Navigation(p => p.Inventario)
                .AutoInclude(false);

            modelBuilder.Entity<Producto>()
                .Navigation(p => p.Detalles)
                .AutoInclude(false);

            // Relación uno a uno entre Producto e Inventario
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Inventario)
                .WithOne(i => i.Producto)
                .HasForeignKey<Inventario>(i => i.ProductoNombre)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación uno a muchos entre Compra y DetalleCompra
            modelBuilder.Entity<Compra>()
                .HasMany(c => c.Detalles)
                .WithOne(d => d.Compra)
                .HasForeignKey(d => d.IdFactura)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación uno a muchos entre Producto y DetalleCompra
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.Detalles)
                .WithOne(d => d.Producto)
                .HasForeignKey(d => d.ProductoNombre)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Producto>()
                .HasMany(p => p.Reabastecimientos)
                .WithOne(r => r.Producto)
                .HasForeignKey(r => r.ProductoNombre)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

    }
}
