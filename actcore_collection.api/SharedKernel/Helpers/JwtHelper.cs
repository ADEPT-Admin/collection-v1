using System.IdentityModel.Tokens.Jwt;

namespace SharedKernel.Helpers
{
    public static class JwtHelper
    {
        public static string? GetValueFromToken(string token, string key)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
                throw new ArgumentException("Invalid token format");

            var jwtToken = handler.ReadJwtToken(token);

            // Payload is just a dictionary of key/value
            if (jwtToken.Payload.TryGetValue(key, out var value))
            {
                return value?.ToString();
            }

            return null;
        }
    }
}
