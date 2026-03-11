using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EMNDC.Preposicionamiento.Models;

namespace EMNDC.Preposicionamiento.DB
{
    public class PreposicionamientoDbContext : IdentityDbContext<UserModel>
    {
        public PreposicionamientoDbContext(DbContextOptions<PreposicionamientoDbContext> options) : base(options) { }
        public DbSet<TokenModel> Token { get; set; }
    }
}
