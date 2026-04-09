namespace EMNDC.Preposicionamiento.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string? Iso{ get; set; }
        public required Pais Country { get; set; }
        public Municipio? City { get; set; }
        public Provincia? State { get; set; }
        public long Lat { get; set; }
        public long Lon { get; set; }
        public DateTime Creado { get; set; } = DateTime.UtcNow;
        public DateTime Modificado { get; set; } = DateTime.UtcNow;
        public List<Almacen> Almacens { get; set; } = new List<Almacen>();
    }
}
