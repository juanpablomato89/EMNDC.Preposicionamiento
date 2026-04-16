using EMNDC.Preposicionamiento.DB;

namespace EMNDC.Preposicionamiento.Utils
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PreposicionamientoDbContext>();
            var geoService = scope.ServiceProvider.GetRequiredService<CubaGeoService>();

            if (context.Provincias.Any())
                return;

            var provincias = geoService.GetAllProvincias();
            await context.Provincias.AddRangeAsync(provincias);
            await context.SaveChangesAsync();
        }
    }
}
