using EMNDC.Preposicionamiento.DB;
using EMNDC.Preposicionamiento.IServices;
using EMNDC.Preposicionamiento.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EMNDC.Preposicionamiento.Services
{
    public class AlertEvaluator : IAlertEvaluator
    {
        private readonly PreposicionamientoDbContext _context;
        private readonly IAlertEmailSender _emailSender;
        private readonly UserManager<UserModel> _userManager;

        public AlertEvaluator(
            PreposicionamientoDbContext context,
            IAlertEmailSender emailSender,
            UserManager<UserModel> userManager)
        {
            _context = context;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public async Task EvaluateAsync(AuditLog log)
        {
            var rules = await _context.AlertRules.Where(r => r.Enabled).ToListAsync();

            foreach (var rule in rules)
            {
                if (await MatchesAsync(rule, log))
                    await TriggerAsync(rule, log);
            }
        }

        private async Task<bool> MatchesAsync(AlertRule rule, AuditLog log)
        {
            switch (rule.EventType)
            {
                case AlertEventType.FailedLoginThreshold:
                    if (log.Action != AuditActions.LoginFail) return false;
                    var threshold = rule.Threshold ?? 5;
                    var window = TimeSpan.FromMinutes(rule.WindowMinutes ?? 15);
                    var since = DateTime.UtcNow - window;
                    var fails = await _context.AuditLogs
                        .Where(a => a.Action == AuditActions.LoginFail &&
                                    a.UserEmail == log.UserEmail &&
                                    a.Timestamp >= since)
                        .CountAsync();
                    return fails >= threshold;

                case AlertEventType.NewAdminCreated:
                    if (log.Action != AuditActions.UserCreated || string.IsNullOrEmpty(log.EntityId)) return false;
                    var user = await _userManager.FindByIdAsync(log.EntityId);
                    if (user == null) return false;
                    return await _userManager.IsInRoleAsync(user, Utils.Roles.Admin);

                case AlertEventType.RoleDeleted:
                    return log.Action == AuditActions.RoleDeleted;

                case AlertEventType.MultipleSessionsRevoked:
                    return log.Action == AuditActions.SessionsRevokedAll;

                case AlertEventType.LdapConfigChanged:
                    return log.Action == AuditActions.LdapConfigUpdated;
            }
            return false;
        }

        private async Task TriggerAsync(AlertRule rule, AuditLog log)
        {
            var payload = JsonSerializer.Serialize(new
            {
                rule = rule.Name,
                eventType = rule.EventType.ToString(),
                log.Action,
                log.UserEmail,
                log.Description,
                log.Timestamp
            });

            var notification = new AlertNotification
            {
                AlertRuleId = rule.Id,
                TriggeredAt = DateTime.UtcNow,
                Payload = payload,
                Sent = false,
            };

            try
            {
                var recipients = (rule.NotifyEmails ?? string.Empty)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (recipients.Length > 0)
                {
                    await _emailSender.SendAsync(recipients,
                        $"[Alerta] {rule.Name}",
                        payload);
                    notification.Sent = true;
                    notification.SentAt = DateTime.UtcNow;
                }
            }
            catch (Exception ex)
            {
                notification.Error = ex.Message;
            }

            _context.AlertNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<AlertNotification> SendTestAsync(AlertRule rule)
        {
            var payload = JsonSerializer.Serialize(new
            {
                rule = rule.Name,
                eventType = rule.EventType.ToString(),
                test = true,
                timestamp = DateTime.UtcNow
            });

            var notification = new AlertNotification
            {
                AlertRuleId = rule.Id,
                TriggeredAt = DateTime.UtcNow,
                Payload = payload,
                Sent = false,
            };

            try
            {
                var recipients = (rule.NotifyEmails ?? string.Empty)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (recipients.Length > 0)
                {
                    await _emailSender.SendAsync(recipients,
                        $"[Alerta TEST] {rule.Name}",
                        payload);
                    notification.Sent = true;
                    notification.SentAt = DateTime.UtcNow;
                }
                else
                {
                    notification.Error = "No hay destinatarios configurados.";
                }
            }
            catch (Exception ex)
            {
                notification.Error = ex.Message;
            }

            _context.AlertNotifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }
    }

    public class StubAlertEmailSender : IAlertEmailSender
    {
        private readonly ILogger<StubAlertEmailSender> _logger;
        public StubAlertEmailSender(ILogger<StubAlertEmailSender> logger) { _logger = logger; }

        public Task SendAsync(IEnumerable<string> recipients, string subject, string body)
        {
            _logger.LogInformation("[ALERT EMAIL STUB] To: {Recipients} | Subject: {Subject} | Body: {Body}",
                string.Join(",", recipients), subject, body);
            return Task.CompletedTask;
        }
    }
}
