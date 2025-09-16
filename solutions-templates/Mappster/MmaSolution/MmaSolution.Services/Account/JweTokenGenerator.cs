
using Microsoft.IdentityModel.JsonWebTokens;

using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace MmaSolution.Services.Account;

internal static class JweTokenGenerator
{
    public static string GenerateJweToken(TokenValidationParameters validationParameters, AppUser user, IList<string> userRoles)
    {
        var encryptionKey = validationParameters.TokenDecryptionKey;
        var signingKey = validationParameters.IssuerSigningKey;

        // Use JsonWebTokenHandler for proper JWE support
        var tokenHandler = new JsonWebTokenHandler();

        // Create claims
        var claims = new Dictionary<string, object>
        {
            {"Id", user.Id.ToString() },
            { JwtRegisteredClaimNames.Sub, user.UserName },
            { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() },
            { JwtRegisteredClaimNames.Name, user.UserName },
            {JwtRegisteredClaimNames.Email, user.Email },
            { JwtRegisteredClaimNames.Iss, validationParameters.ValidIssuer },
            { JwtRegisteredClaimNames.Aud, validationParameters.ValidAudience },
            { JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
            { JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
            { JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds() },
            { ClaimTypes.Role, userRoles}
        };

        

        // Create token descriptor with both signing and encryption
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = claims,
            // First sign the token
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            // Then encrypt it (this creates a JWE with a signed JWT inside)
            EncryptingCredentials = new EncryptingCredentials(
                encryptionKey,
                SecurityAlgorithms.Aes256KW,
                SecurityAlgorithms.Aes256CbcHmacSha512
            )
        };

        return tokenHandler.CreateToken(tokenDescriptor);

    }

    public static string GenerateSignedJwtToken(TokenValidationParameters validationParameters, AppUser user, IList<string> userRoles)
    {
        
        // Create claims
        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString()),
            new( JwtRegisteredClaimNames.Sub, user.UserName),
            new( JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new( JwtRegisteredClaimNames.Name, user.UserName),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new( JwtRegisteredClaimNames.Iss, validationParameters.ValidIssuer),
            new( JwtRegisteredClaimNames.Aud, validationParameters.ValidAudience),
            new( JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
           
        };


        foreach (var userRole in userRoles)
        {
            claims.Add(new(ClaimTypes.Role, userRole));
        }

        // Create token descriptor with both signing and encryption
        var encryptionKey = validationParameters.TokenDecryptionKey;
        var signingKey = validationParameters.IssuerSigningKey;

        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);


        var jwtSecurityToken = new JwtSecurityToken(
           issuer: validationParameters.ValidIssuer,
           audience: validationParameters.ValidAudience,
           claims: claims,
           notBefore: DateTime.UtcNow,
           expires: DateTime.UtcNow.AddMinutes(30),
           signingCredentials: signingCredentials
       );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(jwtSecurityToken);

    }
}
