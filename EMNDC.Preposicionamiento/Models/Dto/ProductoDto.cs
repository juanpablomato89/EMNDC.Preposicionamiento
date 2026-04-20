namespace EMNDC.Preposicionamiento.Models.Dto
{
    public class ProductoDto
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; }
        public string? UnidadMedida { get; set; }
        public int? OrganismoId { get; set; }
        public string? Organismo { get; set; }
        public DateTime Creado { get; set; }
        public DateTime Modificado { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
