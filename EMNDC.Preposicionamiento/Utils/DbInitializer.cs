using EMNDC.Preposicionamiento.DB;
using EMNDC.Preposicionamiento.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMNDC.Preposicionamiento.Utils
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PreposicionamientoDbContext>();
            var geoService = scope.ServiceProvider.GetRequiredService<CubaGeoService>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();

            foreach (var roleName in Roles.All)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            if (!await context.Organismos.AnyAsync())
            {
                await context.Organismos.AddRangeAsync(
                    new Organismo { Descripcion = "MINCIN" },
                    new Organismo { Descripcion = "MINSAP" },
                    new Organismo { Descripcion = "MINFAR" },
                    new Organismo { Descripcion = "MININT" }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Provincias.AnyAsync())
            {
                var provincias = geoService.GetAllProvincias();
                await context.Provincias.AddRangeAsync(provincias);
                await context.SaveChangesAsync();
            }

            const string adminEmail = "admin@preposicionamiento.local";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new UserModel
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Administrador",
                    LastName = "Sistema",
                    EmailConfirmed = true,
                    Creado = DateTime.UtcNow,
                    Modificado = DateTime.UtcNow
                };
                var result = await userManager.CreateAsync(admin, "Admin1234*");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, Roles.Admin);
            }
            else if (!await userManager.IsInRoleAsync(admin, Roles.Admin))
            {
                await userManager.AddToRoleAsync(admin, Roles.Admin);
            }
        }
    }
}
