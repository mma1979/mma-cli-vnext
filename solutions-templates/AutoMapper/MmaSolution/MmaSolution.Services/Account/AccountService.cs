

namespace MmaSolution.Services.Account
{
    public class AccountService : IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private ILogger<AccountService> _logger;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly PasswordHasher _passwordHasher;

        public AccountService(IServiceScopeFactory scopeFactory, ILogger<AccountService> logger, IOptionsMonitor<JwtConfig> optionsMonitor, TokenValidationParameters tokenValidationParameters, PasswordHasher passwordHasher)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResultViewModel<RegistrationResponse>> Register(RegisterDto model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return ResultViewModel<RegistrationResponse>.Error(messages: new() { ResourcesKeys.PASSWORDS_MISMATCH });

            }

            using var scope = _scopeFactory.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var userExists = await userManager.FindByNameAsync(model.Username).ConfigureAwait(false);
            if (userExists != null)
            {
                return ResultViewModel<RegistrationResponse>.Error(messages: new() { ResourcesKeys.USER_ALREADY_EXISTS });
            }


            await using var context = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();

            AppUser user = new(new()
            {
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                UserName = model.Username,
                NormalizedUserName = model.Username.ToUpper(),
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PasswordHash = _passwordHasher.Hash(model.Password),
            });



            var result = await userManager.CreateAsync(user).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                return ResultViewModel<RegistrationResponse>.Error(messages: result.Errors.Select(e => e.Description).ToList());
            }

            using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            foreach (string role in model.Roles)
            {
                if (!await roleManager.RoleExistsAsync(role).ConfigureAwait(false))
                    await roleManager.CreateAsync(new AppRole(role)).ConfigureAwait(false);

                await userManager.AddToRoleAsync(user, role).ConfigureAwait(false);
            }

            var jwtToken = await GenerateJwtToken(user);

            // send confirmation email 
            string confirmationToken = await userManager.
                 GenerateEmailConfirmationTokenAsync(user);


            string pin = confirmationToken.ExtractPin();
            user.AddUserToken(new AppUserTokenModifyModel
            {
                UserId = user.Id,
                Value = pin,
                Token = confirmationToken,
                LoginProvider = TokenTypes.EMAIL_CONFIRMATION,
                Name = TokenTypes.EMAIL_CONFIRMATION
            });

            _ = await context.SaveChangesAsync();

            //send email
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            _ = emailService.Send(user.Email, TokenTypes.EMAIL_CONFIRMATION, $"Your pin code is: {pin}");

            return ResultViewModel<RegistrationResponse>.Success(new()
            {
                RefreshToken = jwtToken.RefreshToken,
                Token = jwtToken.Token
            }, messages: new() { ResourcesKeys.USER_CREATED_SUCCESS });

        }


        public async Task<ResultViewModel<TokenDto>> Login(LoginDto model)
        {
            using var scope = _scopeFactory.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<AppUser>>();

            var user = await userManager.FindByNameAsync(model.Username).ConfigureAwait(false);
            if (user == null)
            {
                return ResultViewModel<TokenDto>.Error(messages: new() { ResourcesKeys.USER_NOT_EXIST });
            }

            var result = _passwordHasher.Verify(model.Password, user.PasswordHash);
            if (!result)
            {
                return ResultViewModel<TokenDto>.Error(messages: new() { ResourcesKeys.PASSWORD_INVALID });
            }

            var jwtToken = await GenerateJwtToken(user);
            return ResultViewModel<TokenDto>.Success(new() { RefreshToken = jwtToken.RefreshToken, Token = jwtToken.Token }, messages: new()
            { "User logged in successfully" });
        }

        public async Task<AcknowledgeViewModel> Logout(Guid userId)
        {
            using var scope = _scopeFactory.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<AppUser>>();

            var user = await userManager.FindByIdAsync(userId.ToString()).ConfigureAwait(false);
            if (user == null)
            {
                return AcknowledgeViewModel.Error(messages: [ResourcesKeys.USER_NOT_EXIST]);
            }

            using var context = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
            var userTokens = context.UserTokens.Where(t => t.UserId == userId && t.Name == TokenTypes.LOGIN_TOKEN).ToList();
            context.UserTokens.RemoveRange(userTokens);
            _ = await context.SaveChangesAsync();

            return AcknowledgeViewModel.Success(messages: new() { "User loged out" });

        }

        public async Task<ResultViewModel<TokenDto>> ResetPassword(ResetPasswordDto model)
        {
            if (model.NewPassword != model.ConfirmPassword)
            {
                return ResultViewModel<TokenDto>.Error(
                messages: new() { ResourcesKeys.PASSWORDS_MISMATCH });
            }

            using var scope = _scopeFactory.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await userManager.FindByNameAsync(model.Username).ConfigureAwait(false);
            if (user == null)
            {
                return ResultViewModel<TokenDto>.Error(messages: new() { ResourcesKeys.USER_NOT_EXIST });
            }



            var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return ResultViewModel<TokenDto>.Error(messages: result.Errors.Select(e => e.Description).ToList());
            }

            user.ResetPassword(_passwordHasher.Hash(model.NewPassword));
            var updates = await userManager.UpdateAsync(user).ConfigureAwait(false);

            if (!updates.Succeeded)
            {
                return ResultViewModel<TokenDto>.Error(messages: updates.Errors.Select(e => e.Description).ToList());
            }

            var jwtToken = await GenerateJwtToken(user);
            return ResultViewModel<TokenDto>.Success(new() { RefreshToken = jwtToken.RefreshToken, Token = jwtToken.Token },
                messages: new() { ResourcesKeys.PASSWORD_RESET_SUCCESS });
        }

        public async Task<AcknowledgeViewModel> EnableTwoFactorAuthentication(EnableTwoFactorAuthenticationDto model)
        {
            using var scope = _scopeFactory.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
            if (user == null)
            {
                return AcknowledgeViewModel.Error(messages: new() { ResourcesKeys.USER_NOT_EXIST });
            }

            var result = await userManager.SetTwoFactorEnabledAsync(user, model.Enabled).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return AcknowledgeViewModel.Error(messages: result.Errors.Select(e => e.Description).ToList());
            }

            return AcknowledgeViewModel.Success(messages: new() { ResourcesKeys.TWO_FACTOR_ENABLED_SUCCESS });
        }

        public async Task<ResultViewModel<string>> GenerateOtp(string userName)
        {
            using var scope = _scopeFactory.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await userManager.FindByNameAsync(userName).ConfigureAwait(false);
            if (user == null)
            {
                return new ResultViewModel<string>
                {
                    IsSuccess = false,
                    Messages = { ResourcesKeys.USER_NOT_EXIST }
                };
            }

            var otp = await userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber).ConfigureAwait(false);
            return new ResultViewModel<string>
            {
                IsSuccess = true,
                Data = otp
            };
        }
        public async Task<AcknowledgeViewModel> ValidateOtp(string userName, string otp)
        {
            using var scope = _scopeFactory.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await userManager.FindByNameAsync(userName).ConfigureAwait(false);
            if (user == null)
            {
                return new AcknowledgeViewModel
                {
                    IsSuccess = false,
                    Messages = { ResourcesKeys.USER_NOT_EXIST }
                };
            }

            var result = await userManager.VerifyTwoFactorTokenAsync(user, "Phone", otp.ToString()).ConfigureAwait(false);
            if (!result)
            {
                return new AcknowledgeViewModel
                {
                    IsSuccess = false,
                    Messages = { ResourcesKeys.OTP_INVALID }
                };
            }

            return new AcknowledgeViewModel
            {
                IsSuccess = true,
                Messages = { ResourcesKeys.OTP_VALIDATION_SUCCESS }
            };
        }

        public async Task<ResultViewModel<TokenDto>> VerifyToken(TokenDto tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);

                // Now we need to check if the token has a valid security algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return new()
                        {
                            IsSuccess = false,
                            Messages = { ResourcesKeys.TOKEN_INVALID }
                        };
                    }
                }

                // Now we need to check if the token has a valid date
                var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expDate > DateTime.UtcNow)
                {
                    return new()
                    {
                        Messages = { ResourcesKeys.TOKEN_NOT_EXPIRED_CANNOT_REFRESH },
                        IsSuccess = false
                    };
                }

                // Check the token we got if its saved in the db
                using var scope = _scopeFactory.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
                var storedRefreshToken = await context.AppRefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if (storedRefreshToken == null)
                {
                    return new()
                    {
                        Messages = { ResourcesKeys.REFRESH_TOKEN_NOT_EXIST },
                        IsSuccess = false
                    };
                }

                // Check the date of the saved token if it has expired
                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    return new()
                    {
                        Messages = { ResourcesKeys.TOKEN_EXPIRED },
                        IsSuccess = false
                    };
                }

                // check if the refresh token has been used
                if (storedRefreshToken.IsUsed)
                {
                    return new()
                    {
                        Messages = { ResourcesKeys.TOKEN_USED },
                        IsSuccess = false
                    };
                }

                // Check if the token is revoked
                if (storedRefreshToken.IsRevoked)
                {
                    return new()
                    {
                        Messages = { ResourcesKeys.TOKEN_REVOKED },
                        IsSuccess = false
                    };
                }

                // we are getting here the jwt token id
                var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                // check the id that the recieved token has against the id saved in the db
                if (storedRefreshToken.JwtId != jti)
                {
                    return new()
                    {
                        Messages = { "the token doenst mateched the saved token" },
                        IsSuccess = false
                    };
                }

                storedRefreshToken.MarkAsUsed();
                context.AppRefreshTokens.Update(storedRefreshToken);
                await context.SaveChangesAsync();
                using var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var dbUser = await _userManager.FindByIdAsync(storedRefreshToken.UserId.ToString());
                var genratedToken = await GenerateJwtToken(dbUser);

                return new()
                {
                    IsSuccess = true,
                    Data = new()
                    {
                        Token = genratedToken.Token,
                        RefreshToken = genratedToken.RefreshToken,
                    }
                };

            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(VerifyToken)}", tokenRequest, ex);
                return new()
                {
                    Messages = { ResourcesKeys.TOKEN_VALIDATION_FAILED },
                    IsSuccess = false
                };
            }
        }

        private async Task<TokenDto> GenerateJwtToken(AppUser user)
        {
            using var scope = _scopeFactory.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            var userRoles = await userManager.GetRolesAsync(user).ConfigureAwait(false);

            

            var token = JweTokenGenerator.GenerateJweToken(_tokenValidationParameters, user, userRoles);

            var refreshToken = new AppRefreshToken(new()
            {

                JwtId = token.ToUniqueNumericValue().ToString(),
                IsUsed = false,
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                IsRevoked = false,
                Token = RandomString(25) + Guid.NewGuid()

            });

            using var context = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
            await context.AppRefreshTokens.AddAsync(refreshToken);

            await RemoveLoginTokens(user.Id, context.Database.GetDbConnection());

            await context.UserTokens.AddAsync(new(new()
            {
                UserId = user.Id,
                LoginProvider = TokenTypes.LOGIN_TOKEN,
                Token = token.ToUniqueNumericValue().ToString(),
                Value = token,
                Name = TokenTypes.LOGIN_TOKEN
            }));

            _ = await context.SaveChangesAsync();

            return new()
            {
                Token = token,
                RefreshToken = refreshToken.Token
            };
        }

        private static async Task RemoveLoginTokens(Guid userId, DbConnection connection)
        {
          
            if(connection.State!= System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            _ = await connection.ExecuteAsync("Delete from AppUserTokens where UserId=@UserId and Name=@Provider", new { UserId = userId, Provider = TokenTypes.LOGIN_TOKEN });
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }
        public string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<AcknowledgeViewModel> ConfirmEmail(Guid userId, string pin)
        {
            using var scope = _scopeFactory.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            using var context = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();

            var user = await userManager.FindByIdAsync(userId.ToString());
            var userToken = await context.UserTokens.FirstOrDefaultAsync(e => e.LoginProvider == TokenTypes.EMAIL_CONFIRMATION && e.Value == pin);
            if (userToken == null)
            {
                _logger.LogInformation($"{DateTime.UtcNow} - Info on {nameof(ConfirmEmail)}: Token does not exist", userId, pin);
                return new()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.TOKEN_NOT_EXIST }
                };
            }

            user.UserTokens.Remove(userToken);
            IdentityResult result = await userManager.
                ConfirmEmailAsync(user, userToken.Token);

            if (!result.Succeeded)
            {
                _logger.LogInformation($"{DateTime.UtcNow} - Info on {nameof(ConfirmEmail)}: Error while confirming your email", userId, pin, userToken);
                return new()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.EMAIL_CONFIRM_ERROR }
                };
            }

            _ = await context.SaveChangesAsync();

            return new()
            {
                IsSuccess = true,
                StatusCode = 200,
                Messages = { ResourcesKeys.EMAIL_CONFIRM_SUCCESS }
            };
        }

        public async Task<AcknowledgeViewModel> ForgetPassword(ForgotPasswordDto model)
        {

            using var scope = _scopeFactory.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await userManager.FindByNameAsync(model.Email).ConfigureAwait(false);

            if (user == null)
            {
                return new()
                {
                    IsSuccess = false,
                    Messages = { ResourcesKeys.USER_NOT_EXIST },
                    StatusCode = 500
                };
            }
            using var context = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();

            var token = await userManager
                .GeneratePasswordResetTokenAsync(user);

            string pin = token.ExtractPin();
            user.AddUserToken(new AppUserTokenModifyModel
            {
                Value = pin,
                Token = token,
                LoginProvider = TokenTypes.FORGET_PASSWORD_TOKEN,
                Name = TokenTypes.FORGET_PASSWORD_TOKEN
            });

            _ = await context.SaveChangesAsync();

            //send email
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            _ = emailService.Send(user.Email, TokenTypes.FORGET_PASSWORD_TOKEN, $"Your pin code is: {pin}");


            return new()
            {
                IsSuccess = true,
                Messages = { ResourcesKeys.PASSWORD_RESET_EMAIL_SENT },
                StatusCode = 200
            };

        }

        public async Task<AcknowledgeViewModel> PinValidate(ValidatePinDto model)
        {

            using var scope = _scopeFactory.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            using var context = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();

            var user = await userManager.FindByEmailAsync(model.Email);
            var userToken = await context.UserTokens.FirstOrDefaultAsync(e => e.LoginProvider == model.Provider && e.Value == model.Pin);
            if (userToken == null)
            {
                _logger.LogInformation($"{DateTime.UtcNow} - Info on {nameof(ConfirmEmail)}: Token does not exist", model);
                return new()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.TOKEN_NOT_EXIST }
                };
            }

            user.UserTokens.Remove(userToken);
            IdentityResult result = await userManager.
                ConfirmEmailAsync(user, userToken.Token);

            if (!result.Succeeded)
            {
                _logger.LogInformation($"{DateTime.UtcNow} - Info on {nameof(ConfirmEmail)}: Error while confirming your email", model);
                return new()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.EMAIL_CONFIRM_ERROR }
                };
            }

            _ = await context.SaveChangesAsync();

            return new()
            {
                IsSuccess = true,
                StatusCode = 200,
                Messages = { "Email successfuly confirmed" }
            };

        }

        #region IDisposable
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                Dispose();
                GC.Collect();
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        #endregion
    }
}
