using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using POS.Backend.DTO;
using Microsoft.IdentityModel.Tokens;

namespace POS.Backend.Helper.Auth
{
    public class IdentityProvider
    {
        public const string UserIdClaim = "userId";
        public const string FullNameClaim = "uFullName";
        public const string UserEmailClaim = "uEmail";

        private readonly SigningCredentials? signingCredentials;
        private readonly JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        public IdentityProvider()
        {
            this.signingCredentials = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
        }

        public string CreateToken(UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException(nameof(userInfo));
            }

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserId),
                new Claim(UserIdClaim, userInfo.UserId.ToString(CultureInfo.InvariantCulture)),
                new Claim(FullNameClaim, userInfo.FullName.ToString(CultureInfo.InvariantCulture)),
                new Claim(UserEmailClaim, userInfo.Email.ToString(CultureInfo.InvariantCulture)),
            };

            var issueTime = DateTime.UtcNow;

            var token = new JwtSecurityToken(
                AuthOptions.Issuer,
                AuthOptions.Audience,
                claims,
                issueTime,
                issueTime + AuthOptions.TokenExpiryDuration,
                this.signingCredentials
                );

            return this.tokenHandler.WriteToken(token);
        }
    
        public string GenerateOTP(string mobileNumber)
        {
            string otp = Utility.RandomNumberGenerator(6); //6 is the length of random number

            //logic here to send this otp to mobile number like using notification service for sending sms
            return otp;
        }
    }
}
