namespace EMNDC.Preposicionamiento.Models
{
    public class Producto
    {
        public Guid Id { get; set; }
        public required string Descripcion { get; set; }
        public string? UnidadMedida { get; set; }
        public int? OrganismoId { get; set; }
        public Organismo? Organismo { get; set; }
        public DateTime Creado { get; set; } = DateTime.UtcNow;
        public DateTime Modificado { get; set; } = DateTime.UtcNow;
        public List<StockAlmacen> Stocks { get; set; } = new List<StockAlmacen>();

    }
}
