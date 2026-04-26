using EMNDC.Preposicionamiento.IServices;
using EMNDC.Preposicionamiento.Models;
using Microsoft.AspNetCore.Identity;

namespace EMNDC.Preposicionamiento.Services
{
    public class DbPasswordValidator : IPasswordValidator<UserModel>
    {
        private readonly IServiceProvider _serviceProvider;

        public DbPasswordValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<UserModel> manager, UserModel user, string? password)
        {
            if (string.IsNullOrEmpty(password))
                return IdentityResult.Failed(new IdentityError { Code = "PasswordEmpty", Description = "La contraseña es requerida." });

            using var scope = _serviceProvider.CreateScope();
            var policyService = scope.ServiceProvider.GetRequiredService<IPasswordPolicyService>();
            var policy = await policyService.GetAsync();

            var errors = new List<IdentityError>();

            if (password.Length < policy.MinLength)
                errors.Add(new IdentityError { Code = "PasswordTooShort", Description = $"La contraseña debe tener al menos {policy.MinLength} caracteres." });

            if (policy.RequireUppercase && !password.Any(char.IsUpper))
                errors.Add(new IdentityError { Code = "PasswordRequiresUpper", Description = "La contraseña requiere al menos una mayúscula." });

            if (policy.RequireLowercase && !password.Any(char.IsLower))
                errors.Add(new IdentityError { Code = "PasswordRequiresLower", Description = "La contraseña requiere al menos una minúscula." });

            if (policy.RequireDigit && !password.Any(char.IsDigit))
                errors.Add(new IdentityError { Code = "PasswordRequiresDigit", Description = "La contraseña requiere al menos un dígito." });

            if (policy.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
                errors.Add(new IdentityError { Code = "PasswordRequiresNonAlphanumeric", Description = "La contraseña requiere al menos un carácter no alfanumérico." });

            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }
    }
}
