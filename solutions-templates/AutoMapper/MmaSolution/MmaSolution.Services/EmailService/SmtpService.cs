namespace MmaSolution.Services.EmailService;

public class SmtpService : IEmailService, IDisposable
{
    private IFluentEmail _fluentEmail;

    public SmtpService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    public async Task<bool> Send(string to, string subject, string body, bool isHtml = true)
    {
        return await Policy
             .Handle<SocketException>()
             .Or<TimeoutException>()
             .Or<SmtpException>()
             .RetryAsync(3)
             .ExecuteAsync(async () =>
             {
                 var response = await SendTask(to, subject, body);
                 return response.Successful;
             });
       
    }

    private async Task<SendResponse> SendTask(string to, string subject, string body)
    {
        var response = await _fluentEmail
          .To(to)
          .Subject(subject)
          .Body(body)
          .SendAsync();

        return response;
    }

    

    #region IDisposable Support
    public void Dispose(bool dispose)
    {
        if (dispose)
        {
            Dispose();

        }
    }

    public void Dispose()
    {

        GC.SuppressFinalize(this);
        GC.Collect();
    }





    #endregion
}
