using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EMNDC.Preposicionamiento.DB;
using EMNDC.Preposicionamiento.IServices;
using EMNDC.Preposicionamiento.Models;
using EMNDC.Preposicionamiento.Models.Responses;

namespace EMNDC.Preposicionamiento.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly ObservatoryDbContext _context;
        private readonly IConfiguration _config;

        public TokenService(UserManager<UserModel> userManager, ObservatoryDbContext context, IConfiguration config)
        {
            _userManager = userManager;
            _context = context;
            _config = config;
        }

        public async Task<TokenResponse> GenerateTokens(UserModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim("lastname", user.LastName),
            new Claim("role", (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "user")
        };

            var accessToken = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            await StoreRefreshToken(user.Id, refreshToken);

            return new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken
            };
        }
        public async Task<bool> ValidateRefreshToken(string refreshToken)
        {
            var tokenEntry = await _context.Token.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            return tokenEntry != null && tokenEntry.ExpiresAt > DateTime.UtcNow && !tokenEntry.IsRevoked;
        }

        public async Task<TokenResponse> RenewAccessToken(string refreshToken)
        {
            var tokenEntry = await _context.Token.FirstOrDefaultAsync(rt => rt.RefreshToken == refreshToken);

            if (tokenEntry == null || tokenEntry.ExpiresAt < DateTime.UtcNow || tokenEntry.IsRevoked)
                return null;

            tokenEntry.IsRevoked = true;
            _context.Update(tokenEntry);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(tokenEntry.UserId);
            return await GenerateTokens(user);
        }

        public async Task RevokeRefreshToken(string userid)
        {
            var tokenEntry = await _context.Token.FirstOrDefaultAsync(u => u.UserId == userid);
            if (tokenEntry != null)
            {
                _context.Token.Remove(tokenEntry);
                await _context.SaveChangesAsync();
            }
        }

        private async Task StoreRefreshToken(string userId, string refreshToken)
        {
            await _context.Token
                .Where(rt => rt.UserId == userId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(rt => rt.IsRevoked, true));

            _context.Token.Add(new TokenModel
            {
                UserId = userId,
                RefreshToken = refreshToken,
                Token = "",
                ExpiresAt = DateTime.UtcNow.AddDays(15)
            });

            await _context.SaveChangesAsync();
        }
    }

}
