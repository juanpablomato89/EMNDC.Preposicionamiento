namespace EMNDC.Preposicionamiento.Models.Dto
{
    public class ProductoCreateUpdateDto
    {
        public string Descripcion { get; set; }
        public string? UnidadMedida { get; set; }
        public int? OrganismoId { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
