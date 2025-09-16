namespace MmaSolution.AppApi.Controllers.v1.Account
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AccountController> _logger;

        private readonly Services.Translator _translator;

        private string Language => Request.Headers.AcceptLanguage.ToString() ?? LanguageCode.Arabic;

        public AccountController(IServiceScopeFactory scopeFactory, ILogger<AccountController> logger, Services.Translator translator)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _translator = translator;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest(new { IsSuccess = false, Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) } });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();

                return BadRequest(new { IsSuccess = false, Messages = errors });
            }

            using var scope = _scopeFactory.CreateScope();
            var accountService = scope.ServiceProvider.GetRequiredService<AccountService>();
            var registrationResult = await accountService.Register(registerDto);

            if (registrationResult.IsSuccess)
            {
                return Ok(registrationResult);
            }

            return BadRequest(registrationResult);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
                });
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();

                return BadRequest(new { IsSuccess = false, Messages = errors });
            }

            using var scope = _scopeFactory.CreateScope();
            using var service = scope.ServiceProvider.GetRequiredService<AccountService>();
            var result = await service.Login(loginDto);
            if (result.IsSuccess)
            {
                HttpContext.Request.Cookies.Keys.ToList().ForEach(x =>
                {
                    Response.Cookies.Delete(x);
                });

                CookieOptions option = new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true
                };

                Response.Cookies.Append("Token", result.Data.Token, option);
                Response.Cookies.Append("Refresh-Token", result.Data.RefreshToken, option);

                return Ok(result);
            }

            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgotPasswordDto forgetPasswordDto, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
                });
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();

                return BadRequest(new { IsSuccess = false, Messages = errors });
            }


            using var scope = _scopeFactory.CreateScope();
            using var service = scope.ServiceProvider.GetRequiredService<AccountService>();
            var result = await service.ForgetPassword(forgetPasswordDto);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("forget-password-validate")]
        public async Task<IActionResult> ForgetPasswordValidate([FromBody] ForgotPasswordDto dto, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
                });
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();

                return BadRequest(new { IsSuccess = false, Messages = errors });
            }


            using var scope = _scopeFactory.CreateScope();
            using var service = scope.ServiceProvider.GetRequiredService<AccountService>();
            var result = await service.PinValidate(new(dto.Email, dto.Pin, TokenTypes.FORGET_PASSWORD_TOKEN));
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }



        [HttpPost("confirm-email")]
        [RequiredPermission("Update")]
        public async Task<IActionResult> ConfirmCmail([FromQuery] Guid userId, [FromQuery] string pin, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
                });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();

                return BadRequest(new { IsSuccess = false, Messages = errors });
            }

            using var scope = _scopeFactory.CreateScope();
            using var service = scope.ServiceProvider.GetRequiredService<AccountService>();
            var result = await service.ConfirmEmail(userId, pin);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

       
        [HttpPost("reset-password")]
        [RequiredPermission("Update")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
                });
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();

                return BadRequest(new { IsSuccess = false, Messages = errors });
            }

            using var scope = _scopeFactory.CreateScope();
            using var service = scope.ServiceProvider.GetRequiredService<AccountService>();
            var result = await service.ResetPassword(resetPasswordDto);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }


        [HttpPost("validate-otp")]
        [RequiredPermission("Read")]
        public async Task<IActionResult> ValidateOTP([FromBody] ValidateOtpDto validateOtpDto, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
                });
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();

                return BadRequest(new { IsSuccess = false, Messages = errors });
            }

            using var scope = _scopeFactory.CreateScope();
            using var service = scope.ServiceProvider.GetRequiredService<AccountService>();
            var result = await service.ValidateOtp(validateOtpDto.Identifier, validateOtpDto.Otp);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);

        }

        [HttpPost("AppRefreshToken")]
        [RequiredPermission("Update")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenRequest, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
                });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();

                return BadRequest(new { IsSuccess = false, Messages = errors });
            }

            using var scope = _scopeFactory.CreateScope();
            using var service = scope.ServiceProvider.GetRequiredService<AccountService>();

            var result = await service.VerifyToken(tokenRequest);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);

        }


        [HttpGet("Logout")]
        [RequiredPermission("Read")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
                });
            }


            using var scope = _scopeFactory.CreateScope();
            using var service = scope.ServiceProvider.GetRequiredService<AccountService>();
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
            var result = await service.Logout(userId);
            if (result.IsSuccess)
            {
                HttpContext.Request.Cookies.Keys.ToList().ForEach(x =>
                {
                    Response.Cookies.Delete(x);
                });

                return Ok(result);
            }

            return BadRequest(result);
        }

    }
}
