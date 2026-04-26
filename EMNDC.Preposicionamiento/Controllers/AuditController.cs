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
    public class AuditController : ControllerBase
    {
        private readonly PreposicionamientoDbContext _context;

        public AuditController(PreposicionamientoDbContext context)
        {
            _context = context;
        }

        // GET: api/Audit
        [HttpGet]
        public async Task<IActionResult> GetLogs(
            int pageIndex = 0,
            int pageSize = 20,
            string? search = null,
            string? action = null,
            string? userId = null,
            DateTime? from = null,
            DateTime? to = null,
            bool? success = null)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(a =>
                    (a.UserEmail != null && a.UserEmail.ToLower().Contains(s)) ||
                    (a.Description != null && a.Description.ToLower().Contains(s)) ||
                    (a.EntityType != null && a.EntityType.ToLower().Contains(s)) ||
                    a.Action.ToLower().Contains(s));
            }
            if (!string.IsNullOrWhiteSpace(action))
                query = query.Where(a => a.Action == action);
            if (!string.IsNullOrWhiteSpace(userId))
                query = query.Where(a => a.UserId == userId);
            if (from.HasValue)
                query = query.Where(a => a.Timestamp >= from.Value);
            if (to.HasValue)
                query = query.Where(a => a.Timestamp <= to.Value);
            if (success.HasValue)
                query = query.Where(a => a.Success == success.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(a => a.Timestamp)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(a => new AuditLogDto
                {
                    Id = a.Id,
                    Timestamp = a.Timestamp,
                    UserId = a.UserId,
                    UserEmail = a.UserEmail,
                    Action = a.Action,
                    EntityType = a.EntityType,
                    EntityId = a.EntityId,
                    Description = a.Description,
                    IpAddress = a.IpAddress,
                    UserAgent = a.UserAgent,
                    Success = a.Success
                })
                .ToListAsync();

            return Ok(new { items, totalCount, pageIndex, pageSize });
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            var a = await _context.AuditLogs.FindAsync(id);
            if (a == null) return NotFound();
            return Ok(new AuditLogDto
            {
                Id = a.Id,
                Timestamp = a.Timestamp,
                UserId = a.UserId,
                UserEmail = a.UserEmail,
                Action = a.Action,
                EntityType = a.EntityType,
                EntityId = a.EntityId,
                Description = a.Description,
                IpAddress = a.IpAddress,
                UserAgent = a.UserAgent,
                Success = a.Success
            });
        }

        // GET: api/Audit/actions
        [HttpGet("actions")]
        public async Task<IActionResult> GetActions()
        {
            var actions = await _context.AuditLogs
                .Select(a => a.Action)
                .Distinct()
                .OrderBy(a => a)
                .ToListAsync();
            return Ok(actions);
        }
    }
}
