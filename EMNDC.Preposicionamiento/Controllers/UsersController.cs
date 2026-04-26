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
    [Authorize(Roles = Roles.Admin)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PreposicionamientoDbContext _context;

        public UsersController(
            UserManager<UserModel> userManager,
            RoleManager<IdentityRole> roleManager,
            PreposicionamientoDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: api/Users?pageIndex=&pageSize=&search=&role=&organismoId=&onlyLocked=
        [HttpGet]
        public async Task<IActionResult> GetUsers(
            int pageIndex = 0,
            int pageSize = 10,
            string? search = null,
            string? role = null,
            int? organismoId = null,
            bool? onlyLocked = null)
        {
            var query = _context.Users
                .Include(u => u.Organismo)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(u =>
                    (u.Email != null && u.Email.ToLower().Contains(s)) ||
                    (u.Name != null && u.Name.ToLower().Contains(s)) ||
                    (u.LastName != null && u.LastName.ToLower().Contains(s)));
            }

            if (organismoId.HasValue)
                query = query.Where(u => u.OrganismoId == organismoId);

            if (onlyLocked == true)
                query = query.Where(u => u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow);

            var totalBeforeRole = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.Email)
                .ToListAsync();

            var result = new List<UserDto>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                var primaryRole = roles.FirstOrDefault();
                if (!string.IsNullOrEmpty(role) && primaryRole != role) continue;

                result.Add(new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Name = u.Name,
                    LastName = u.LastName,
                    OrganismoId = u.OrganismoId,
                    Organismo = u.Organismo?.Descripcion,
                    Role = primaryRole,
                    IsLockedOut = u.LockoutEnd.HasValue && u.LockoutEnd > DateTimeOffset.UtcNow,
                    IsUserDomain = u.IsUserDomain,
                    Creado = u.Creado,
                    Modificado = u.Modificado
                });
            }

            var totalCount = result.Count;
            var paged = result.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return Ok(new { items = paged, totalCount, pageIndex, pageSize });
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _context.Users
                .Include(u => u.Organismo)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                OrganismoId = user.OrganismoId,
                Organismo = user.Organismo?.Descripcion,
                Role = roles.FirstOrDefault(),
                IsLockedOut = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow,
                IsUserDomain = user.IsUserDomain,
                Creado = user.Creado,
                Modificado = user.Modificado
            });
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return BadRequest("Ya existe un usuario con ese email.");

            if (!Roles.All.Contains(dto.Role))
                return BadRequest("Rol no válido.");

            var user = new UserModel
            {
                UserName = dto.Email,
                Email = dto.Email,
                Name = dto.Name,
                LastName = dto.LastName,
                OrganismoId = dto.OrganismoId,
                EmailConfirmed = true,
                Creado = DateTime.UtcNow,
                Modificado = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            await _userManager.AddToRoleAsync(user, dto.Role);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new { user.Id });
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (dto.Name != null) user.Name = dto.Name;
            if (dto.LastName != null) user.LastName = dto.LastName;
            if (dto.OrganismoId.HasValue) user.OrganismoId = dto.OrganismoId;
            user.Modificado = DateTime.UtcNow;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return BadRequest(updateResult.Errors.Select(e => e.Description));

            if (!string.IsNullOrEmpty(dto.Role))
            {
                if (!Roles.All.Contains(dto.Role))
                    return BadRequest("Rol no válido.");

                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, dto.Role);
            }

            return NoContent();
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var currentUserId = User.GetUserIdFromToken();
            if (currentUserId == id)
                return BadRequest("No puede eliminar su propio usuario.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return NoContent();
        }

        // POST: api/Users/{id}/lock
        [HttpPost("{id}/lock")]
        public async Task<IActionResult> LockUser(string id, [FromBody] LockUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var currentUserId = User.GetUserIdFromToken();
            if (currentUserId == id && dto.Lock)
                return BadRequest("No puede bloquear su propio usuario.");

            await _userManager.SetLockoutEnabledAsync(user, true);
            var lockoutEnd = dto.Lock ? DateTimeOffset.UtcNow.AddYears(100) : (DateTimeOffset?)null;
            var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return NoContent();
        }

        // POST: api/Users/{id}/reset-password
        [HttpPost("{id}/reset-password")]
        public async Task<IActionResult> ResetPassword(string id, [FromBody] ResetPasswordAdminDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return NoContent();
        }

        // GET: api/Users/roles
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            return Ok(Roles.All.Select(r => new { name = r }));
        }
    }
}
