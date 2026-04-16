using System.Text.Json;
using EMNDC.Preposicionamiento.Models;
using GeoJSON.Text.Feature;
using GeoJSON.Text.Geometry;

public class CubaGeoService
{
    private readonly IWebHostEnvironment _env;
    private List<Provincia> _provincias;

    public CubaGeoService(IWebHostEnvironment env)
    {
        _env = env;
        CargarDatos();
    }

    private void CargarDatos()
    {
        var filePath = Path.Combine(_env.WebRootPath, "data", "cuba.geojson");
        if (!File.Exists(filePath))
            throw new FileNotFoundException("No se encuentra el archivo GeoJSON");

        var json = File.ReadAllText(filePath);
        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json);

        var provinciasDict = new Dictionary<string, Provincia>();
        int idProv = 1, idMun = 1;

        foreach (var feature in featureCollection.Features)
        {
            var props = feature.Properties;
            string provinciaNombre = props["province"]?.ToString();
            string provinciaId = props["province_id"]?.ToString();
            string municipioNombre = props["municipality"]?.ToString();

            // Calcular centroide de la geometría
            var centroide = CalcularCentroide(feature.Geometry);

            // Obtener o crear la provincia
            if (!provinciasDict.TryGetValue(provinciaId, out var provincia))
            {
                provincia = new Provincia
                {
                    Id = idProv++,
                    Descripcion = provinciaNombre,
                    Posicionamiento = new Posicionamiento{ Lat = centroide.Lat, Lon= centroide.Lon },
                    Municipios = new List<Municipio>()
                };
                provinciasDict[provinciaId] = provincia;
            }

            // Crear el municipio
            var municipio = new Municipio
            {
                Id = idMun++,
                Descripcion = municipioNombre,
                Posicionamiento = new Posicionamiento { Lat = centroide.Lat, Lon = centroide.Lon },
            };
            provincia.Municipios.Add(municipio);
        }

        _provincias = provinciasDict.Values.ToList();
    }

    private (double Lat, double Lon) CalcularCentroide(IGeometryObject geometry)
    {
        var allPoints = new List<IPosition>();

        if (geometry is Polygon polygon)
        {
            allPoints.AddRange(polygon.Coordinates.First().Coordinates);
        }
        else if (geometry is MultiPolygon multiPolygon)
        {
            foreach (var poly in multiPolygon.Coordinates)
                allPoints.AddRange(poly.Coordinates.First().Coordinates);
        }

        if (allPoints.Count == 0) return (0, 0);

        var lat = allPoints.Average(p => p.Latitude);
        var lon = allPoints.Average(p => p.Longitude);
        return (lat, lon);
    }

    public List<Provincia> GetAllProvincias() => _provincias;
}