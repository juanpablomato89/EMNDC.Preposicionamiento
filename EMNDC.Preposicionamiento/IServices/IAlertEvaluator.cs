using EMNDC.Preposicionamiento.Models;

namespace EMNDC.Preposicionamiento.IServices
{
    public interface IAlertEvaluator
    {
        Task EvaluateAsync(AuditLog log);
        Task<AlertNotification> SendTestAsync(AlertRule rule);
    }

    public interface IAlertEmailSender
    {
        Task SendAsync(IEnumerable<string> recipients, string subject, string body);
    }
}
