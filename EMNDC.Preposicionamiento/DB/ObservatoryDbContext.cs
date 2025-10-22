using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EMNDC.Preposicionamiento.Models;

namespace EMNDC.Preposicionamiento.DB
{
    public class ObservatoryDbContext : IdentityDbContext<UserModel>
    {
        public ObservatoryDbContext(DbContextOptions<ObservatoryDbContext> options) : base(options) { }
        public DbSet<TokenModel> Token { get; set; }
    }
}
