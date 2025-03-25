using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace pd311_web_api.BLL.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly string email;
        private readonly string password;
        private readonly string host;
        private readonly int port;
        private readonly SmtpClient _smtpClient;

        public EmailService(IConfiguration configuration)
        {
            email = configuration["SmtpSettings:Email"] ?? "";
            password = configuration["SmtpSettings:Password"] ?? "";
            host = configuration["SmtpSettings:Host"] ?? "";
            port = int.Parse(configuration["SmtpSettings:Port"] ?? "0");

            _smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(email, password)
            };
        }

        public async Task<ServiceResponse<bool>> SendMailAsync(string to, string subject, string body, bool isHtml = false)
        {
            try
            {
                var message = new MailMessage
                {
                    From = new MailAddress(email),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };
                message.To.Add(to);

                return await SendMailAsync(message);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>(ex.Message, false, false);
            }
        }

        public async Task<ServiceResponse<bool>> SendMailAsync(MailMessage message)
        {
            try
            {
                await _smtpClient.SendMailAsync(message);
                return new ServiceResponse<bool>("Email sent successfully", true, true);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>($"Failed to send email: {ex.Message}", false, false);
            }
        }
    }
}
