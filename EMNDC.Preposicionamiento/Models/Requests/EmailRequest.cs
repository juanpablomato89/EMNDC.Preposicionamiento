
namespace EMNDC.Preposicionamiento.Models.Requests
{
    public class EmailRequest
    {
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public List<string>? Cc { get; set; }
    }
}
