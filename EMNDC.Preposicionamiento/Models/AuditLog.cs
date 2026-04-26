namespace EMNDC.Preposicionamiento.Models
{
    public class AuditLog
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? UserId { get; set; }
        public string? UserEmail { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? EntityType { get; set; }
        public string? EntityId { get; set; }
        public string? Description { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public bool Success { get; set; } = true;
    }

    public static class AuditActions
    {
        public const string LoginSuccess = "LOGIN_SUCCESS";
        public const string LoginFail = "LOGIN_FAIL";
        public const string Logout = "LOGOUT";
        public const string UserCreated = "USER_CREATED";
        public const string UserUpdated = "USER_UPDATED";
        public const string UserDeleted = "USER_DELETED";
        public const string UserLocked = "USER_LOCKED";
        public const string UserUnlocked = "USER_UNLOCKED";
        public const string UserPasswordReset = "USER_PASSWORD_RESET";
        public const string RoleCreated = "ROLE_CREATED";
        public const string RoleUpdated = "ROLE_UPDATED";
        public const string RoleDeleted = "ROLE_DELETED";
        public const string SessionRevoked = "SESSION_REVOKED";
        public const string SessionsRevokedAll = "SESSIONS_REVOKED_ALL";
        public const string PasswordPolicyUpdated = "PASSWORD_POLICY_UPDATED";
        public const string LdapConfigUpdated = "LDAP_CONFIG_UPDATED";
        public const string LdapConfigTested = "LDAP_CONFIG_TESTED";
        public const string AlertRuleCreated = "ALERT_RULE_CREATED";
        public const string AlertRuleUpdated = "ALERT_RULE_UPDATED";
        public const string AlertRuleDeleted = "ALERT_RULE_DELETED";
    }
}
