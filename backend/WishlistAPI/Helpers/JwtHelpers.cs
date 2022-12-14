using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WishlistAPI.Extensions;
using WishlistAPI.Models;
using WishlistAPI.Models.DataModels;
using ILogger = Serilog.ILogger;

namespace WishlistAPI.Helpers
{
    public static class JwtHelpers
    {
        private static readonly ILogger logger = Log.ForContext(typeof(JwtHelpers));
        public static IEnumerable<Claim> GetClaims(UserToken userToken)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("UserId", userToken.UserId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, userToken.UserName),
                new Claim(ClaimTypes.Email, userToken.Email),
                // Expires in 1 day
                new Claim(ClaimTypes.Expiration, userToken.Expiration.ToString("MMM ddd dd yyyy HH:mm:ss tt"))
            };

            if (userToken.Role == User.UserRole.Administrator)
            {
                claims.Add(new Claim(ClaimTypes.Role, User.UserRole.Administrator.ToString()));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, User.UserRole.User.ToString()));
            }

            return claims;
        }

        public static async Task<string>GetTokenKey(this UserToken userToken, JwtSettings jwtSettings)
        {
            try
            {
                // Obtain secret key
                SecretClientOptions options = new SecretClientOptions()
                {
                    Retry =
                    {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                    }
                };
                var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultURI"));
                var secretClient = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential(), options);
                KeyVaultSecret issuerSigningKeySecret = await secretClient.GetSecretAsync("WishlistAPIJwtIssuerSigningKey");
                var issuerSigningKeySecretValue = Convert.FromBase64String(issuerSigningKeySecret.Value);

                string issuer = Environment.GetEnvironmentVariable("Issuer") ?? jwtSettings.ValidIssuers[0];

                logger.Information($"JwtHelpers - GetTokenKey: Generating token...");
                // Generate JWT
                var jwtToken = new JwtSecurityToken(
                    issuer: issuer,
                    audience: jwtSettings.ValidAudience,
                    claims: GetClaims(userToken),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(userToken.Expiration).DateTime,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(issuerSigningKeySecretValue),
                        SecurityAlgorithms.HmacSha256
                        )
                    );

                var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                return token;
            }
            catch (Exception ex)
            {
                logger.Error("Error trying to get user: {Message}", ex.Message);
                logger.Error("Inner exception: {InnerException}", ex.InnerException);
                logger.Error("Stack trace: {StackTrace}", ex.StackTrace);
                throw new Exception("Error generating the JWT", ex);
            }
        }
    }
}
