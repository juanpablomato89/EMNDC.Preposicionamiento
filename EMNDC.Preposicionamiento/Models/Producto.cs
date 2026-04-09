namespace EMNDC.Preposicionamiento.Models
{
    public class Producto
    {
        public Guid Id { get; set; }
        public required string Descripcion { get; set; }
        public string? UnidadMedidas { get; set; }
        public string? Organismo { get; set; }
        public DateTime Almacenado { get; set; }
        public DateTime Creado { get; set; } = DateTime.UtcNow;
        public DateTime Modificado { get; set; } = DateTime.UtcNow;
        public List<Almacen> Almacens { get; set; } = new List<Almacen>();
    }
}
