
using Resend;


namespace MmaSolution.Services.EmailService;

public class ResendService: IEmailService
{
    private readonly IConfiguration _configuration;


    public ResendService(IConfiguration configuration)
    {
        _configuration = configuration;

    }



    public async Task<bool> Send(string to, string subject, string body, bool isHtml = true)
    {
        var key = _configuration["Resend:ApiToken"];
        var from = _configuration["Resend:Sender"];
        IResend resend = ResendClient.Create(key);

        var mail1 = new EmailMessage()
        {
            From = from,
            To = to,
            Subject = subject,
            HtmlBody = body,
        };



        var resp = await resend.EmailSendAsync(mail1);

        return resp.Success;
    }


}