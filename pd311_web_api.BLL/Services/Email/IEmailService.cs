using System.Net.Mail;
using System.Threading.Tasks;

namespace pd311_web_api.BLL.Services.Email
{
    public interface IEmailService
    {
        Task<ServiceResponse<bool>> SendMailAsync(string to, string subject, string body, bool isHtml = false);
        Task<ServiceResponse<bool>> SendMailAsync(MailMessage message);
    }
}
