namespace MmaSolution.Tests
{
    public class AccountControllerTests
    {

        private AccountController accoutController;
        [SetUp]
        public void Setup()
        {
            StartupBuilder builder = new StartupBuilder().Init();

            accoutController = builder.GetInstance<AccountController>();

        }

        [Test]
        public void Get_AuthenticateController_NotNull()
        {
            Assert.Equals(accoutController, null);
        }

        [TestCase("mamado2000@gmail.com", "Abc@1234", "User")]
        public async Task RegiterUser_Returns_200(string email, string password, string role)
        {
            RegisterDto model = new()
            {
                Email = email,
                Username = email,
                Password = password,
                ConfirmPassword = password,
                Roles = new HashSet<string> { role }
            };

            var res = await accoutController.RegisterUser(model, CancellationToken.None).ConfigureAwait(false);

            Assert.Equals(res is OkResult, true);
        }

        public async Task Login_Returns_Token(string username, string password)
        {
            LoginDto model = new("mamado2000@gmail.com", "Abc@1234");

            var res = await accoutController.Login(model, CancellationToken.None).ConfigureAwait(false);
            Assert.Equals(res is OkResult, true);
        }

    }
}
