using EMNDC.Preposicionamiento.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EMNDC.Preposicionamiento.DB
{
    public class PreposicionamientoDbContext : IdentityDbContext<UserModel>
    {
        public PreposicionamientoDbContext(DbContextOptions<PreposicionamientoDbContext> options)
            : base(options) { }

        // DbSets
        public DbSet<TokenModel> Tokens { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Almacen> Almacenes { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Organismo> Organismos { get; set; }
        public DbSet<StockAlmacen> StocksAlmacen { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========== CONFIGURACIÓN DE POSICIONAMIENTO (Value Object) ==========
            // Configuración compartida para todas las entidades que tienen Posicionamiento

            modelBuilder.Entity<Provincia>().OwnsOne(p => p.Posicionamiento);
            modelBuilder.Entity<Almacen>().OwnsOne(a => a.Posicionamiento);
            modelBuilder.Entity<Address>().OwnsOne(a => a.Posicionamiento);
            modelBuilder.Entity<Municipio>().OwnsOne(m => m.Posicionamiento);

            // ========== RELACIONES Y CLAVES FORÁNEAS ==========

            // País -> Provincias
            modelBuilder.Entity<Provincia>()
                .HasOne(p => p.Pais)
                .WithMany(p => p.Provincias)
                .HasForeignKey(p => p.PaisId)
                .OnDelete(DeleteBehavior.Restrict);

            // Provincia -> Municipios
            modelBuilder.Entity<Municipio>()
                .HasOne(m => m.Provincia)
                .WithMany(p => p.Municipios)
                .HasForeignKey(m => m.ProvinciaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Municipio -> Address (opcional: si cada Address tiene un Municipio)
            modelBuilder.Entity<Address>()
                .HasOne(a => a.Municipio)
                .WithMany()  // Un municipio puede tener muchas direcciones
                .HasForeignKey(a => a.MunicipioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Address -> Almacen (uno a muchos)
            modelBuilder.Entity<Almacen>()
                .HasOne(a => a.Address)
                .WithMany(a => a.Almacenes)
                .HasForeignKey(a => a.AddressId)
                .OnDelete(DeleteBehavior.SetNull);

            // Producto -> Organismo (si lo tienes)
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Organismo)
                .WithMany()
                .HasForeignKey(p => p.OrganismoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Tabla intermedia StockAlmacen (muchos a muchos con atributos)
            modelBuilder.Entity<StockAlmacen>(entity =>
            {
                entity.HasKey(s => new { s.AlmacenId, s.ProductoId });

                entity.HasOne(s => s.Almacen)
                    .WithMany(a => a.Stocks)
                    .HasForeignKey(s => s.AlmacenId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.Producto)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(s => s.ProductoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ========== TOKEN ==========
            modelBuilder.Entity<TokenModel>(entity =>
            {
                entity.HasOne(t => t.User)
                    .WithMany()  // Si UserModel no tiene lista de tokens
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(t => t.RefreshToken).IsUnique();  // Importante
            });

            // ========== ÍNDICES Y RESTRICCIONES ==========

            // Índices únicos para códigos
            modelBuilder.Entity<Provincia>().HasIndex(p => p.CodigoProvincia).IsUnique();
            modelBuilder.Entity<Municipio>().HasIndex(m => m.CodigoMunicipio).IsUnique();
            modelBuilder.Entity<Pais>().HasIndex(p => p.Descripcion).IsUnique();  // Evitar duplicados

            // Índices para búsquedas frecuentes
            modelBuilder.Entity<Almacen>().HasIndex(a => a.Descripcion);
            modelBuilder.Entity<Producto>().HasIndex(p => p.Descripcion);
            modelBuilder.Entity<Address>().HasIndex(a => a.MunicipioId);

            // ========== LONGITUDES MÁXIMAS ==========

            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.Property(p => p.Descripcion).HasMaxLength(100).IsRequired();
                entity.Property(p => p.CodigoProvincia).HasMaxLength(10).IsRequired();
            });

            modelBuilder.Entity<Municipio>(entity =>
            {
                entity.Property(m => m.Descripcion).HasMaxLength(100).IsRequired();
                entity.Property(m => m.CodigoMunicipio).HasMaxLength(10).IsRequired();
            });

            modelBuilder.Entity<Pais>(entity =>
            {
                entity.Property(p => p.Descripcion).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Almacen>(entity =>
            {
                entity.Property(a => a.Descripcion).HasMaxLength(200);
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(p => p.Descripcion).HasMaxLength(200).IsRequired();
                entity.Property(p => p.UnidadMedida).HasMaxLength(20);
            });

            modelBuilder.Entity<Organismo>(entity =>
            {
                entity.Property(o => o.Descripcion).HasMaxLength(100).IsRequired();
            });
        }
    }
}
