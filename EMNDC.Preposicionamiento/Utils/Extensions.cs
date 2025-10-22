
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EMNDC.Preposicionamiento.Utils
{
    public static class Extensions
    {
        public static string GetUserIdFromToken(this ClaimsPrincipal user)
        {
            var claimsIdentity = user.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? claimsIdentity?.FindFirst("sub")?.Value;

            return userId ?? throw new Exception("User Id not found in the token.");
        }

        public static string GetUserEmailFromToken(this ClaimsPrincipal user)
        {
            var claimsIdentity = user.Identity as ClaimsIdentity;
            var userEmail = claimsIdentity?.FindFirst(JwtRegisteredClaimNames.Email)?.Value
                            ?? claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value;

            return userEmail ?? throw new Exception("The Email not found in the token."); ;
        }

        public static string GetUserRoleFromToken(this ClaimsPrincipal user)
        {
            var claimsIdentity = user.Identity as ClaimsIdentity;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value
                       ?? claimsIdentity?.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            return role ?? throw new Exception("The Role not found in the token.");
        }

        public static string GetUserNameFromToken(this ClaimsPrincipal user)
        {
            var claimsIdentity = user.Identity as ClaimsIdentity;
            var name = claimsIdentity?.FindFirst("name")?.Value;

            return name ?? throw new Exception("The Name not found in the token.");
        }

        public static string GetUserLastNameFromToken(this ClaimsPrincipal user)
        {
            var claimsIdentity = user.Identity as ClaimsIdentity;
            var name = claimsIdentity?.FindFirst("lastname")?.Value;

            return name ?? throw new Exception("The Last Name not found in the token.");
        }
    }

}