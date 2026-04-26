namespace EMNDC.Preposicionamiento.Models
{
    public class LdapConfiguration
    {
        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 389;
        public bool UseSsl { get; set; }
        public string BaseDn { get; set; } = string.Empty;
        public string BindDn { get; set; } = string.Empty;
        public string? BindPasswordEncrypted { get; set; }
        public string UserSearchFilter { get; set; } = "(sAMAccountName={0})";
        public string EmailAttribute { get; set; } = "mail";
        public string NameAttribute { get; set; } = "givenName";
        public string LastNameAttribute { get; set; } = "sn";
        public string DefaultRole { get; set; } = "User";
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? UpdatedByUserId { get; set; }
    }
}
