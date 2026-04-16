namespace EMNDC.Preposicionamiento.Models
{
    public class Municipio
    {
        public int Id { get; set; }
        public required string Descripcion { get; set; }
        public Posicionamiento Posicionamiento { get; set; }
        public DateTime Creado { get; set; } = DateTime.UtcNow;
        public DateTime Modificado { get; set; } = DateTime.UtcNow;
    }
}
