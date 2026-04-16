namespace EMNDC.Preposicionamiento.Models
{
    public class Almacen
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public Posicionamiento Posicionamiento { get; set; }
        public DateTime Creado { get; set; } = DateTime.UtcNow;
        public DateTime Modificado { get; set; } = DateTime.UtcNow;
        public List<Producto> Productos { get; set; } = new List<Producto>();
    }
}
