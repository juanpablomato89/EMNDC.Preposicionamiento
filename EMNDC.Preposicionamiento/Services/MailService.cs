
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using EMNDC.Preposicionamiento.IServices;
using EMNDC.Preposicionamiento.Models.Requests;

namespace EMNDC.Preposicionamiento.Services
{
    public class MailService :IMailKitService
    {
        private readonly IConfiguration _configuration;
        private bool useNotifications;    

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            useNotifications =bool.Parse(_configuration.GetSection("MailKit")["UseNotifications"]);
        }
        public async Task SendEmailResponseAsync(string subject, string message, string to)
        {
            if (useNotifications == true)
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration.GetSection("MailKit")["from"]));

                if (to.Contains(","))
                {
                    string[] subs = to.Split(",");
                    foreach (var item in subs) email.To.Add(MailboxAddress.Parse(item));
                } else
                {
                    email.To.Add(MailboxAddress.Parse(to));
                }
                
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = message };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.office365.com", 587);
                smtp.Authenticate("", "");
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        }

        public async Task SendEmailResponseAsync(string from, string password, string subject, string message, string to)
        {
            if (useNotifications == true)
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration.GetSection("MailKit")["from"]));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = message };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.office365.com", 587);
                smtp.Authenticate(from, password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        }

        public async Task SendEmailResponseAsync(EmailRequest email)
        {
            try
            {
                if (useNotifications == true)
                {
                    var mailkitEmail = new MimeMessage();

                    string smtpHost = _configuration.GetSection("MailKit")["SmtpHost"];
                    string smtpPort = _configuration.GetSection("MailKit")["SmtpPort"];
                    string password = _configuration.GetSection("MailKit")["SmtpPass"];
                    string from = _configuration.GetSection("MailKit")["from"];

                    mailkitEmail.Subject = email.Subject;
                    mailkitEmail.Body = new TextPart(TextFormat.Html) { Text = email.Body };

                    mailkitEmail.From.Add(MailboxAddress.Parse(from));
                    if (email.To != null)
                    {
                        mailkitEmail.To.Add(MailboxAddress.Parse(email.To));

                    }
                    if (email.Cc != null)
                    {
                        foreach (var Cc in email.Cc)
                        {
                            mailkitEmail.Cc.Add(MailboxAddress.Parse(Cc));
                        }
                    }

                    using var smtp = new SmtpClient();
                    smtp.Connect(smtpHost, Convert.ToInt32(smtpPort));
                    smtp.Authenticate(from, password);
                    await smtp.SendAsync(mailkitEmail);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

       
    }
}
