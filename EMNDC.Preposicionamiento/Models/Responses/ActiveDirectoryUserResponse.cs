namespace EMNDC.Preposicionamiento.Models.Responses
{
    public class ActiveDirectoryUserResponse
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Groups { get; set; } = new List<string>();
        public bool IsEnabled { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
