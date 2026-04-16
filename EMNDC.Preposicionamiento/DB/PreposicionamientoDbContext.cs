using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EMNDC.Preposicionamiento.Models;

namespace EMNDC.Preposicionamiento.DB
{
    public class PreposicionamientoDbContext : IdentityDbContext<UserModel>
    {
        public PreposicionamientoDbContext(DbContextOptions<PreposicionamientoDbContext> options) : base(options) { }
        public DbSet<TokenModel> Token { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Almacen> Almacens { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Pais> Pais { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
    }
}
