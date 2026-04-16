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
        var municipiosTemp = new List<(string CodigoMunicipio, string Descripcion, double Lat, double Lon, string CodigoProvincia)>();

        foreach (var feature in featureCollection.Features)
        {
            var props = feature.Properties;
            string provinciaNombre = props["province"]?.ToString();
            string provinciaId = props["province_id"]?.ToString();
            string municipioNombre = props["municipality"]?.ToString();
            string codigoMunicipio = props["DPA_municipality_code"]?.ToString();

            var centroide = CalcularCentroide(feature.Geometry);

            if (!municipiosTemp.Any(a => a.CodigoMunicipio == codigoMunicipio))
            {
                municipiosTemp.Add((codigoMunicipio, municipioNombre, centroide.Lat, centroide.Lon, provinciaId));
            }

            if (!provinciasDict.ContainsKey(provinciaId))
            {
                provinciasDict[provinciaId] = new Provincia
                {
                    Descripcion = provinciaNombre,
                    CodigoProvincia = provinciaId,
                    Posicionamiento = new Posicionamiento { Lat = 0, Lon = 0 },
                    Municipios = new List<Municipio>(),
                    Creado = DateTime.UtcNow,
                    Modificado = DateTime.UtcNow
                };
            }
        }

        foreach (var provincia in provinciasDict.Values)
        {
            var municipiosDeProvincia = municipiosTemp.Where(m => m.CodigoProvincia == provincia.CodigoProvincia).ToList();
            if (municipiosDeProvincia.Any())
            {
                var latPromedio = municipiosDeProvincia.Average(m => m.Lat);
                var lonPromedio = municipiosDeProvincia.Average(m => m.Lon);
                provincia.Posicionamiento = new Posicionamiento { Lat = latPromedio, Lon = lonPromedio };
            }
        }

        foreach (var temp in municipiosTemp)
        {
            var provincia = provinciasDict[temp.CodigoProvincia];
            var municipio = new Municipio
            {
                Descripcion = temp.Descripcion,
                CodigoMunicipio = temp.CodigoMunicipio,
                Posicionamiento = new Posicionamiento { Lat = temp.Lat, Lon = temp.Lon },
                Provincia = provincia,
                Creado = DateTime.UtcNow,
                Modificado = DateTime.UtcNow
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