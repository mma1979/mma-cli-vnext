
namespace MmaSolution.Services.SmsService;

public interface ISmsService
{
    Task Send(string to, string message);
}
