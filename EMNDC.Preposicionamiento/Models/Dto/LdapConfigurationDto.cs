namespace EMNDC.Preposicionamiento.Models.Dto
{
    public class LdapConfigurationDto
    {
        public bool Enabled { get; set; }
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string BaseDn { get; set; } = string.Empty;
        public string BindDn { get; set; } = string.Empty;
        public bool HasPassword { get; set; }
        public string UserSearchFilter { get; set; } = string.Empty;
        public string EmailAttribute { get; set; } = string.Empty;
        public string NameAttribute { get; set; } = string.Empty;
        public string LastNameAttribute { get; set; } = string.Empty;
        public string DefaultRole { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
        public string? UpdatedByUserId { get; set; }
        public string? UpdatedByEmail { get; set; }
    }

    public class LdapConfigurationUpdateDto
    {
        public bool Enabled { get; set; }
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string BaseDn { get; set; } = string.Empty;
        public string BindDn { get; set; } = string.Empty;
        // Si null/vacío, no se modifica el password existente.
        public string? BindPassword { get; set; }
        public string UserSearchFilter { get; set; } = string.Empty;
        public string EmailAttribute { get; set; } = string.Empty;
        public string NameAttribute { get; set; } = string.Empty;
        public string LastNameAttribute { get; set; } = string.Empty;
        public string DefaultRole { get; set; } = string.Empty;
    }

    public class LdapTestRequestDto
    {
        public bool UseStored { get; set; } = true;
        public string? Host { get; set; }
        public int? Port { get; set; }
        public bool? UseSsl { get; set; }
        public string? BindDn { get; set; }
        public string? BindPassword { get; set; }
        public string? SampleUsername { get; set; }
    }

    public class LdapTestResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, string>? SampleUser { get; set; }
    }
}
