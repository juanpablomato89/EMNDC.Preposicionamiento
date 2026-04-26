using EMNDC.Preposicionamiento.DB;
using EMNDC.Preposicionamiento.IServices;
using EMNDC.Preposicionamiento.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace EMNDC.Preposicionamiento.Services
{
    public class LdapConfigService : ILdapConfigService
    {
        private readonly PreposicionamientoDbContext _context;
        private readonly IDataProtector _protector;
        private static LdapConfiguration? _cached;
        private static readonly SemaphoreSlim _lock = new(1, 1);

        public LdapConfigService(PreposicionamientoDbContext context, IDataProtectionProvider provider)
        {
            _context = context;
            _protector = provider.CreateProtector("Ldap.BindPassword.v1");
        }

        public async Task<LdapConfiguration> GetAsync()
        {
            if (_cached != null) return _cached;

            await _lock.WaitAsync();
            try
            {
                if (_cached != null) return _cached;
                var cfg = await _context.LdapConfigurations.AsNoTracking().FirstOrDefaultAsync();
                if (cfg == null)
                {
                    cfg = new LdapConfiguration();
                    _context.LdapConfigurations.Add(cfg);
                    await _context.SaveChangesAsync();
                }
                _cached = cfg;
                return _cached;
            }
            finally { _lock.Release(); }
        }

        public async Task<LdapConfiguration> UpdateAsync(LdapConfiguration config, string? plainPassword, string? updatedByUserId)
        {
            var existing = await _context.LdapConfigurations.FirstOrDefaultAsync();
            if (existing == null)
            {
                existing = new LdapConfiguration();
                _context.LdapConfigurations.Add(existing);
            }

            existing.Enabled = config.Enabled;
            existing.Host = config.Host;
            existing.Port = config.Port;
            existing.UseSsl = config.UseSsl;
            existing.BaseDn = config.BaseDn;
            existing.BindDn = config.BindDn;
            existing.UserSearchFilter = config.UserSearchFilter;
            existing.EmailAttribute = config.EmailAttribute;
            existing.NameAttribute = config.NameAttribute;
            existing.LastNameAttribute = config.LastNameAttribute;
            existing.DefaultRole = config.DefaultRole;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedByUserId = updatedByUserId;

            if (!string.IsNullOrEmpty(plainPassword))
                existing.BindPasswordEncrypted = EncryptPassword(plainPassword);

            await _context.SaveChangesAsync();
            Invalidate();
            return existing;
        }

        public string? EncryptPassword(string? plain)
        {
            if (string.IsNullOrEmpty(plain)) return null;
            return _protector.Protect(plain);
        }

        public string? DecryptPassword(string? encrypted)
        {
            if (string.IsNullOrEmpty(encrypted)) return null;
            try { return _protector.Unprotect(encrypted); }
            catch { return null; }
        }

        public void Invalidate() => _cached = null;
    }
}
