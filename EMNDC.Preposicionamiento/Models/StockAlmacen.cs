namespace EMNDC.Preposicionamiento.Models
{
    public class StockAlmacen
    {
        public int AlmacenId { get; set; }
        public Almacen Almacen { get; set; }
        public Guid ProductoId { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime Creado { get; set; } = DateTime.UtcNow;
        public DateTime Modificado { get; set; } = DateTime.UtcNow;

    }
}
