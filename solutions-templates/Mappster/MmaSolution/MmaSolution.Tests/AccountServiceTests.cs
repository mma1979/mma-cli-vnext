namespace MmaSolution.Tests
{
    public class AccountServiceTests
    {
        private AccountService accountService;
        [SetUp]
        public void Setup()
        {
            StartupBuilder builder = new StartupBuilder().Init();

            accountService = builder.GetInstance<AccountService>();

        }

        [Test]
        public void Test1()
        {
            Assert.Equals(accountService, null);
        }
    }
}