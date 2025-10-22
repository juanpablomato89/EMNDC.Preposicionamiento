
using Microsoft.AspNetCore.Identity.Data;
using EMNDC.Preposicionamiento.Models;
using EMNDC.Preposicionamiento.Models.Requests;

namespace EMNDC.Preposicionamiento.IServices
{
    public interface IAuthService
    {
        Task<UserModel> SignUpAsync(RegisterUserRequests request);
        Task<UserModel> LoginAsync(LoginRequests loginRequest);
        Task<bool> SendResetCodeAsync(SendEmailRequest email);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
        Task<UserModel> GetUserDetailsAsync(string userId);
    }
}