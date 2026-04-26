using EMNDC.Preposicionamiento.DB;
using EMNDC.Preposicionamiento.Models.Dto;
using EMNDC.Preposicionamiento.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMNDC.Preposicionamiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class SessionsController : ControllerBase
    {
        private readonly PreposicionamientoDbContext _context;

        public SessionsController(PreposicionamientoDbContext context)
        {
            _context = context;
        }

        // GET: api/Sessions?pageIndex=&pageSize=&search=&userId=&onlyActive=
        [HttpGet]
        public async Task<IActionResult> GetSessions(
            int pageIndex = 0,
            int pageSize = 10,
            string? search = null,
            string? userId = null,
            bool onlyActive = true)
        {
            var query = _context.Token
                .Include(t => t.User)
                .AsQueryable();

            var now = DateTime.UtcNow;

            if (onlyActive)
                query = query.Where(t => !t.IsRevoked && t.ExpiresAt > now);

            if (!string.IsNullOrWhiteSpace(userId))
                query = query.Where(t => t.UserId == userId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(t =>
                    (t.User.Email != null && t.User.Email.ToLower().Contains(s)) ||
                    (t.User.Name != null && t.User.Name.ToLower().Contains(s)) ||
                    (t.User.LastName != null && t.User.LastName.ToLower().Contains(s)));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(t => new SessionDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    UserEmail = t.User.Email,
                    UserName = t.User.Name,
                    UserLastName = t.User.LastName,
                    CreatedAt = t.CreatedAt,
                    ExpiresAt = t.ExpiresAt,
                    IsRevoked = t.IsRevoked,
                    IsExpired = t.ExpiresAt <= now,
                    IsActive = !t.IsRevoked && t.ExpiresAt > now
                })
                .ToListAsync();

            return Ok(new { items, totalCount, pageIndex, pageSize });
        }

        // POST: api/Sessions/{id}/revoke
        [HttpPost("{id:int}/revoke")]
        public async Task<IActionResult> RevokeSession(int id)
        {
            var token = await _context.Token.FirstOrDefaultAsync(t => t.Id == id);
            if (token == null) return NotFound();

            token.IsRevoked = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Sessions/user/{userId}/revoke-all
        [HttpPost("user/{userId}/revoke-all")]
        public async Task<IActionResult> RevokeAllForUser(string userId)
        {
            var tokens = await _context.Token
                .Where(t => t.UserId == userId && !t.IsRevoked)
                .ToListAsync();

            foreach (var t in tokens) t.IsRevoked = true;
            await _context.SaveChangesAsync();
            return Ok(new { revoked = tokens.Count });
        }
    }
}
