namespace EMNDC.Preposicionamiento.Models
{
    public class Provincia
    {
        public int Id { get; set; }
        public required string Descricion { get; set; }
        public DateTime Creado { get; set; } = DateTime.UtcNow;
        public DateTime Modificado { get; set; } = DateTime.UtcNow;
        public List<Municipio> Municipios { get; set; } = new List<Municipio>();

    }
}
