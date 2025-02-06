using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace API.Helpers
{
    public class JWTHelper
    {
        private const string SecretKey = "RuyekAvdaras417107701741SaradvaKeyur"; // The secret key used for token validation.

        /// <summary>
        /// Validates the JWT token and returns the ClaimsPrincipal if valid.
        /// </summary>
        /// <param name="token">The JWT token to be validated.</param>
        /// <returns>A ClaimsPrincipal if the token is valid, otherwise null.</returns>
        public static ClaimsPrincipal ValidateJwtToken(string token)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            try
            {
                TokenValidationParameters parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = "Issuer",
                    ValidAudience = "Audience",
                    IssuerSigningKey = key
                };

                SecurityToken validatedToken;
                ClaimsPrincipal principal = handler.ValidateToken(token, parameters, out validatedToken);

                return principal;
            }
            catch
            {
                return null; // Invalid token
            }
        }

        /// <summary>
        /// Generates a JWT token for the specified username, with role claim and expiration.
        /// </summary>
        /// <param name="username">The username for which the token is being generated.</param>
        /// <param name="role">The role to assign to the user in the token.</param>
        /// <returns>A string representing the generated JWT token.</returns>
        public static string GenerateJwtToken(string username, int userID, string role)
        {
            Claim[] claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role), // Dynamically setting the role
            new Claim(ClaimTypes.NameIdentifier, userID.ToString())
        };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "Issuer",
                audience: "Audience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static int GetUserIdFromToken(string token)
        {
            JwtSecurityTokenHandler objJwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jsonToken = objJwtSecurityTokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                throw new UnauthorizedAccessException("Invalid JWT token.");
            }

            // Get the userId claim from the token
            Claim userIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                return int.Parse(userIdClaim.Value);  // Return the UserId as an integer
            }

            throw new UnauthorizedAccessException("User ID not found in the token.");
        }
    }
}