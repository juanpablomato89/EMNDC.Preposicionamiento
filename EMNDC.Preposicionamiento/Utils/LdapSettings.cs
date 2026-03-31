namespace EMNDC.Preposicionamiento.Utils
{
    public class LdapSettings
    {
        public required string Domain { get; set; }
        public required string LdapServer { get; set; }
        public required int LdapPort { get; set; }
        public required bool UseSSL { get; set; }
        public required string BaseDN { get; set; }
        public required string UserSearchBase { get; set; }
        public required string AdminUser { get; set; }
        public required string AdminPassword { get; set; }


    }
}
