namespace EMNDC.Preposicionamiento.Models
{
    public class Pais
    {
        public int Id { get; set; }
        public required string Descricion { get; set; }
        public DateTime Creado { get; set; } = DateTime.UtcNow;
        public DateTime Modificado { get; set; } = DateTime.UtcNow;
        public List<Provincia> Provincias { get; set; } = new List<Provincia>();
    }
}
