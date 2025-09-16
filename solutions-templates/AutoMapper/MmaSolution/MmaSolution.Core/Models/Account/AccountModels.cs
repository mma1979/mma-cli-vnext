namespace MmaSolution.Core.Models.Account;


public partial class RegisterDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string FullName { get; set; }
    public IEnumerable<string> Roles { get; set; }

    public RegisterDto()
    {
    }
}

public record LoginDto(string Username, string Password);
public record ForgotPasswordDto(string Email, string Pin);
public record ValidatePinDto(string Email, string Pin, string Provider);
public record ResetPasswordDto(string Username, string NewPassword, string ConfirmPassword, string Token);
public record RequestOtpDto(string Identifier, string OtpType);
public record ValidateOtpDto(string Identifier, string OtpType, string Otp);
public record ChangePasswordDto(string OldPassword, string NewPassword, string Confirmpassword);

public partial class ChangeEmailDto
{
    public string Email { get; set; }
    public string Confirmemail { get; set; }
}

public partial class ChangeMobileDto
{
    public string Mobile { get; set; }
    public string Confirmmobile { get; set; }
}

public partial class ChangeUsernameDto
{
    public string Username { get; set; }
    public string Confirmusername { get; set; }
}

public partial class ChangeCountryCodeDto
{
    public string CountryCode { get; set; }
}

public partial class ChangeProfilePictureDto
{
    public string ProfilePicture { get; set; }
}

public partial class EnableTwoFactorAuthenticationDto
{
    public string UserName { get; set; }
    public bool Enabled { get; set; }
    public TwoFactorMethods TwoFactorMethod { get; set; }
}

public partial class TokenDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}

public partial class RegistrationResponse : TokenDto
{

}

public class JwtConfig
{
    public string ValidAudience { get; set; }
    public string ValidIssuer { get; set; }
    public string TokenDecryptionKey { get; set; }
    public string IssuerSigningKey { get; set; }
    public int Expire { get; set; }
}

