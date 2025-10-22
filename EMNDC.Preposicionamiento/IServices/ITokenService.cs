using Microsoft.AspNetCore.Identity;
using EMNDC.Preposicionamiento.Models;
using EMNDC.Preposicionamiento.Models.Responses;

namespace EMNDC.Preposicionamiento.IServices
{
    public interface ITokenService
    {
        Task<TokenResponse> GenerateTokens(UserModel user);
        Task<TokenResponse> RenewAccessToken(string refreshToken);
        Task RevokeRefreshToken(string userId);
        Task<bool> ValidateRefreshToken(string refreshToken);
    }
}