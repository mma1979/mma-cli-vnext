namespace MmaSolution.Tests
{
    public class EmailServiceTests
    {
        private EmailService service;
        StartupBuilder startupBuilder;

        [OneTimeSetUp]
        public void SetupOnce()
        {

            startupBuilder = new StartupBuilder().Init();

        }

        [SetUp]
        public void Setup()
        {

            service = startupBuilder.GetInstance<EmailService>();
        }

        [Test]
        public async Task Send_Email_Return_Success()
        {
            var response = await service.Send("mamado2000@gmail.com", "account confirmation", $"your code is: {Random.Shared.Next(0000, 9999)}");

            Assert.Equals(response.Successful, true);
        }
    }
}
