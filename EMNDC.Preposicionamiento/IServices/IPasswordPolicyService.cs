using EMNDC.Preposicionamiento.Models;

namespace EMNDC.Preposicionamiento.IServices
{
    public interface IPasswordPolicyService
    {
        Task<PasswordPolicy> GetAsync();
        Task<PasswordPolicy> UpdateAsync(PasswordPolicy policy, string? updatedByUserId);
        void Invalidate();
    }
}
