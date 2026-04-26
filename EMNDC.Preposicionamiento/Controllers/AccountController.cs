using EMNDC.Preposicionamiento.DB;
using EMNDC.Preposicionamiento.Models;
using EMNDC.Preposicionamiento.Models.Dto;
using EMNDC.Preposicionamiento.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMNDC.Preposicionamiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly PreposicionamientoDbContext _context;

        public AccountController(UserManager<UserModel> userManager, PreposicionamientoDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: api/Account/me
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userId = User.GetUserIdFromToken();
            var user = await _context.Users
                .Include(u => u.Organismo)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new AccountDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                OrganismoId = user.OrganismoId,
                Organismo = user.Organismo?.Descripcion,
                Role = roles.FirstOrDefault(),
                IsUserDomain = user.IsUserDomain,
                Creado = user.Creado
            });
        }

        // PUT: api/Account/profile
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = User.GetUserIdFromToken();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (user.IsUserDomain)
                return BadRequest("Los datos de usuarios de dominio se gestionan en Active Directory.");

            if (dto.Name != null) user.Name = dto.Name;
            if (dto.LastName != null) user.LastName = dto.LastName;
            user.Modificado = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return NoContent();
        }

        // POST: api/Account/change-password
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = User.GetUserIdFromToken();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (user.IsUserDomain)
                return BadRequest("La contraseña de usuarios de dominio se gestiona en Active Directory.");

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return NoContent();
        }

        // POST: api/Account/change-email
        [HttpPost("change-email")]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDto dto)
        {
            var userId = User.GetUserIdFromToken();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (user.IsUserDomain)
                return BadRequest("El email de usuarios de dominio se gestiona en Active Directory.");

            var passwordOk = await _userManager.CheckPasswordAsync(user, dto.CurrentPassword);
            if (!passwordOk) return BadRequest("Contraseña actual incorrecta.");

            if (await _userManager.FindByEmailAsync(dto.NewEmail) != null)
                return BadRequest("Ya existe un usuario con ese email.");

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, dto.NewEmail);
            var result = await _userManager.ChangeEmailAsync(user, dto.NewEmail, token);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            await _userManager.SetUserNameAsync(user, dto.NewEmail);
            user.Modificado = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        // POST: api/Account/revoke-all-sessions
        [HttpPost("revoke-all-sessions")]
        public async Task<IActionResult> RevokeAllSessions()
        {
            var userId = User.GetUserIdFromToken();
            var tokens = await _context.Token.Where(t => t.UserId == userId).ToListAsync();
            foreach (var t in tokens) t.IsRevoked = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
