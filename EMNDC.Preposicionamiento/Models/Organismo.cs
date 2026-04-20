namespace EMNDC.Preposicionamiento.Models
{
    public class Organismo
    {
        public int Id { get; set; }
        public required string Descripcion { get; set; }
        public DateTime Creado { get; set; } = DateTime.UtcNow;
        public DateTime Modificado { get; set; } = DateTime.UtcNow;
    }
}
