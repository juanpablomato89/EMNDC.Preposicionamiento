
using EMNDC.Preposicionamiento.Models.Requests;

namespace EMNDC.Preposicionamiento.IServices
{
    public interface IMailKitService
    {
        Task SendEmailResponseAsync(string subject, string message, string to);
        Task SendEmailResponseAsync(string userEmail, string password, string subject, string message, string to);
        Task SendEmailResponseAsync(EmailRequest email);       

    }
}
