using EMNDC.Preposicionamiento.Models;

namespace EMNDC.Preposicionamiento.IServices
{
    public interface ILdapConfigService
    {
        Task<LdapConfiguration> GetAsync();
        Task<LdapConfiguration> UpdateAsync(LdapConfiguration config, string? plainPassword, string? updatedByUserId);
        string? DecryptPassword(string? encrypted);
        string? EncryptPassword(string? plain);
        void Invalidate();
    }
}
