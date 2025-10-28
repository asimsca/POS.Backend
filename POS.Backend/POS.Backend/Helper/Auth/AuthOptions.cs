using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace POS.Backend.Helper.Auth
{
    public static class AuthOptions
    {
        public const string Issuer = "POSIssuer";
        public const string Audience = "POSUsers";
        private const string SigningKey = "THIS_IS_A_SUPER_SECRET_KEY_1234567890!";  // at least 32+ characters for HMAC
        public static TimeSpan TokenExpiryDuration = TimeSpan.FromMinutes(30);

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            //return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            return  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
        }
    }
}
