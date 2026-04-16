namespace EMNDC.Preposicionamiento.Models
{
    public class Municipio
    {
        public int Id { get; set; }
        public required string Descripcion { get; set; }
        public Posicionamiento Posicionamiento { get; set; }
        public required string CodigoMunicipio { get; set; }
        public int ProvinciaId { get; set; }
        public Provincia Provincia { get; set; } = null!;
        public DateTime Creado { get; set; } = DateTime.UtcNow;
        public DateTime Modificado { get; set; } = DateTime.UtcNow;
    }
}
