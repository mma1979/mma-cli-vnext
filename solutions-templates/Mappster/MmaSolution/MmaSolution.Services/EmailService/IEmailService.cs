
namespace MmaSolution.Services.EmailService;

public interface IEmailService
{
    Task<bool> Send(string to, string subject, string body, bool isHtml = true);
}
