namespace MmaSolution.Services
{
    public class EmailService : IDisposable
    {
        private IFluentEmail _fluentEmail;

        public EmailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        public async Task<SendResponse> Send(string to, string subject, string body)
        {
            return await Policy
                 .Handle<SocketException>()
                 .Or<TimeoutException>()
                 .Or<SmtpException>()
                 .RetryAsync(3)
                 .ExecuteAsync(async () =>
                 {
                     return await SendTask(to, subject, body);
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
}
