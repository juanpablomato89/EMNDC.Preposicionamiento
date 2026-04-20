namespace EMNDC.Preposicionamiento.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int? MunicipioId { get; set; }
        public Municipio? Municipio { get; set; }
        public Posicionamiento Posicionamiento { get; set; }
        public DateTime Creado { get; set; } = DateTime.UtcNow;
        public DateTime Modificado { get; set; } = DateTime.UtcNow;
        public List<Almacen> Almacenes { get; set; } = new List<Almacen>();
    }
}
