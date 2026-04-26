using EMNDC.Preposicionamiento.DB;
using EMNDC.Preposicionamiento.IServices;
using EMNDC.Preposicionamiento.Models;
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
    public class AlertsController : ControllerBase
    {
        private readonly PreposicionamientoDbContext _context;
        private readonly IAlertEvaluator _evaluator;
        private readonly IAuditService _audit;

        public AlertsController(PreposicionamientoDbContext context, IAlertEvaluator evaluator, IAuditService audit)
        {
            _context = context;
            _evaluator = evaluator;
            _audit = audit;
        }

        // GET: api/Alerts
        [HttpGet]
        public async Task<IActionResult> GetRules()
        {
            var rules = await _context.AlertRules.OrderBy(r => r.Name).ToListAsync();
            return Ok(rules.Select(ToDto));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRule(int id)
        {
            var r = await _context.AlertRules.FindAsync(id);
            if (r == null) return NotFound();
            return Ok(ToDto(r));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AlertRuleCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name requerido.");

            var rule = new AlertRule
            {
                Name = dto.Name.Trim(),
                Description = dto.Description,
                EventType = dto.EventType,
                Threshold = dto.Threshold,
                WindowMinutes = dto.WindowMinutes,
                Enabled = dto.Enabled,
                NotifyEmails = string.Join(",", dto.NotifyEmails ?? new()),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            _context.AlertRules.Add(rule);
            await _context.SaveChangesAsync();

            await _audit.LogAsync(AuditActions.AlertRuleCreated,
                entityType: "AlertRule", entityId: rule.Id.ToString(),
                description: $"Regla creada: {rule.Name}");

            return CreatedAtAction(nameof(GetRule), new { id = rule.Id }, new { id = rule.Id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AlertRuleUpdateDto dto)
        {
            var rule = await _context.AlertRules.FindAsync(id);
            if (rule == null) return NotFound();

            rule.Name = dto.Name.Trim();
            rule.Description = dto.Description;
            rule.EventType = dto.EventType;
            rule.Threshold = dto.Threshold;
            rule.WindowMinutes = dto.WindowMinutes;
            rule.Enabled = dto.Enabled;
            rule.NotifyEmails = string.Join(",", dto.NotifyEmails ?? new());
            rule.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await _audit.LogAsync(AuditActions.AlertRuleUpdated,
                entityType: "AlertRule", entityId: rule.Id.ToString(),
                description: $"Regla actualizada: {rule.Name}");

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rule = await _context.AlertRules.FindAsync(id);
            if (rule == null) return NotFound();

            _context.AlertRules.Remove(rule);
            await _context.SaveChangesAsync();

            await _audit.LogAsync(AuditActions.AlertRuleDeleted,
                entityType: "AlertRule", entityId: id.ToString(),
                description: $"Regla eliminada: {rule.Name}");

            return NoContent();
        }

        [HttpPost("{id:int}/test")]
        public async Task<IActionResult> Test(int id)
        {
            var rule = await _context.AlertRules.FindAsync(id);
            if (rule == null) return NotFound();

            var notification = await _evaluator.SendTestAsync(rule);
            return Ok(new { sent = notification.Sent, error = notification.Error });
        }

        // GET: api/Alerts/notifications
        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications(
            int pageIndex = 0,
            int pageSize = 20,
            int? alertRuleId = null,
            bool? sent = null,
            DateTime? from = null,
            DateTime? to = null)
        {
            var query = _context.AlertNotifications
                .Include(n => n.AlertRule)
                .AsQueryable();

            if (alertRuleId.HasValue) query = query.Where(n => n.AlertRuleId == alertRuleId);
            if (sent.HasValue) query = query.Where(n => n.Sent == sent.Value);
            if (from.HasValue) query = query.Where(n => n.TriggeredAt >= from.Value);
            if (to.HasValue) query = query.Where(n => n.TriggeredAt <= to.Value);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(n => n.TriggeredAt)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(n => new AlertNotificationDto
                {
                    Id = n.Id,
                    AlertRuleId = n.AlertRuleId,
                    AlertRuleName = n.AlertRule != null ? n.AlertRule.Name : "",
                    TriggeredAt = n.TriggeredAt,
                    Payload = n.Payload,
                    Sent = n.Sent,
                    SentAt = n.SentAt,
                    Error = n.Error
                })
                .ToListAsync();

            return Ok(new { items, totalCount, pageIndex, pageSize });
        }

        // GET: api/Alerts/event-types
        [HttpGet("event-types")]
        public IActionResult GetEventTypes()
        {
            return Ok(Enum.GetValues<AlertEventType>().Select(e => new
            {
                value = (int)e,
                name = e.ToString(),
                requiresThreshold = e == AlertEventType.FailedLoginThreshold
            }));
        }

        private static AlertRuleDto ToDto(AlertRule r) => new()
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            EventType = r.EventType,
            EventTypeName = r.EventType.ToString(),
            Threshold = r.Threshold,
            WindowMinutes = r.WindowMinutes,
            Enabled = r.Enabled,
            NotifyEmails = string.IsNullOrEmpty(r.NotifyEmails)
                ? new List<string>()
                : r.NotifyEmails.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(),
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt,
        };
    }
}
