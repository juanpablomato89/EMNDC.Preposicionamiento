namespace EMNDC.Preposicionamiento.Models
{
    public class Provincia
    {
        public int Id { get; set; }
        public required string Descripcion { get; set; }
        public Posicionamiento Posicionamiento { get; set; }
        public DateTime Creado { get; set; } = DateTime.UtcNow;
        public DateTime Modificado { get; set; } = DateTime.UtcNow;
        public int PaisId { get; set; }
        public Pais Pais { get; set; } = null!;
        public List<Municipio> Municipios { get; set; } = new List<Municipio>();
        public required string CodigoProvincia { get; set; }
    }
}
