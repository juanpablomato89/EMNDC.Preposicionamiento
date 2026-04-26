using EMNDC.Preposicionamiento.IServices;
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
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly IAuditService _audit;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<UserModel> userManager, IAuditService audit)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _audit = audit;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var result = new List<RoleDto>();
            foreach (var r in roles)
            {
                var users = await _userManager.GetUsersInRoleAsync(r.Name!);
                result.Add(new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name ?? string.Empty,
                    UsersCount = users.Count,
                    IsSystem = Utils.Roles.All.Contains(r.Name)
                });
            }
            return Ok(result.OrderBy(r => r.Name));
        }

        // GET: api/Roles/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var users = await _userManager.GetUsersInRoleAsync(role.Name!);
            return Ok(new RoleDto
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty,
                UsersCount = users.Count,
                IsSystem = Utils.Roles.All.Contains(role.Name)
            });
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("El nombre del rol es requerido.");

            var name = dto.Name.Trim();
            if (await _roleManager.RoleExistsAsync(name))
                return BadRequest("Ya existe un rol con ese nombre.");

            var result = await _roleManager.CreateAsync(new IdentityRole(name));
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            var created = await _roleManager.FindByNameAsync(name);
            await _audit.LogAsync(AuditActions.RoleCreated,
                entityType: "Role", entityId: created!.Id,
                description: $"Rol creado: {created.Name}");
            return CreatedAtAction(nameof(GetRole), new { id = created!.Id }, new { id = created.Id });
        }

        // PUT: api/Roles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("El nombre del rol es requerido.");

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            if (Utils.Roles.All.Contains(role.Name))
                return BadRequest("No se puede renombrar un rol del sistema.");

            var newName = dto.Name.Trim();
            if (!string.Equals(role.Name, newName, StringComparison.OrdinalIgnoreCase) &&
                await _roleManager.RoleExistsAsync(newName))
                return BadRequest("Ya existe un rol con ese nombre.");

            var oldName = role.Name;
            role.Name = newName;
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            await _audit.LogAsync(AuditActions.RoleUpdated,
                entityType: "Role", entityId: role.Id,
                description: $"Rol renombrado: {oldName} -> {newName}");

            return NoContent();
        }

        // DELETE: api/Roles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            if (Utils.Roles.All.Contains(role.Name))
                return BadRequest("No se puede eliminar un rol del sistema.");

            var users = await _userManager.GetUsersInRoleAsync(role.Name!);
            if (users.Count > 0)
                return BadRequest($"No se puede eliminar el rol porque tiene {users.Count} usuario(s) asignado(s).");

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            await _audit.LogAsync(AuditActions.RoleDeleted,
                entityType: "Role", entityId: id,
                description: $"Rol eliminado: {role.Name}");

            return NoContent();
        }
    }
}
