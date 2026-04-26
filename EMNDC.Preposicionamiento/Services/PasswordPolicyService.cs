using EMNDC.Preposicionamiento.DB;
using EMNDC.Preposicionamiento.IServices;
using EMNDC.Preposicionamiento.Models;
using Microsoft.EntityFrameworkCore;

namespace EMNDC.Preposicionamiento.Services
{
    public class PasswordPolicyService : IPasswordPolicyService
    {
        private readonly PreposicionamientoDbContext _context;
        private static PasswordPolicy? _cached;
        private static readonly SemaphoreSlim _lock = new(1, 1);

        public PasswordPolicyService(PreposicionamientoDbContext context)
        {
            _context = context;
        }

        public async Task<PasswordPolicy> GetAsync()
        {
            if (_cached != null) return _cached;

            await _lock.WaitAsync();
            try
            {
                if (_cached != null) return _cached;
                var policy = await _context.PasswordPolicies.AsNoTracking().FirstOrDefaultAsync();
                if (policy == null)
                {
                    policy = new PasswordPolicy();
                    _context.PasswordPolicies.Add(policy);
                    await _context.SaveChangesAsync();
                }
                _cached = policy;
                return _cached;
            }
            finally { _lock.Release(); }
        }

        public async Task<PasswordPolicy> UpdateAsync(PasswordPolicy policy, string? updatedByUserId)
        {
            var existing = await _context.PasswordPolicies.FirstOrDefaultAsync();
            if (existing == null)
            {
                existing = new PasswordPolicy();
                _context.PasswordPolicies.Add(existing);
            }

            existing.MinLength = policy.MinLength;
            existing.RequireUppercase = policy.RequireUppercase;
            existing.RequireLowercase = policy.RequireLowercase;
            existing.RequireDigit = policy.RequireDigit;
            existing.RequireNonAlphanumeric = policy.RequireNonAlphanumeric;
            existing.MaxFailedAttempts = policy.MaxFailedAttempts;
            existing.LockoutMinutes = policy.LockoutMinutes;
            existing.PasswordExpirationDays = policy.PasswordExpirationDays;
            existing.PreventReuseLastN = policy.PreventReuseLastN;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedByUserId = updatedByUserId;

            await _context.SaveChangesAsync();
            Invalidate();
            return existing;
        }

        public void Invalidate()
        {
            _cached = null;
        }
    }
}
