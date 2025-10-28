using System.Security.Claims;

namespace POS.Backend.Helper.Auth
{
    public class UserAccessor
    {
        private readonly IHttpContextAccessor _accessor;
        public UserAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        }

        public ClaimsPrincipal User => _accessor.HttpContext?.User ?? throw new InvalidOperationException("User Context is not available");

        //public string UserName => GetClaimValue(IdentityProvider.UserNameClaim);
        public string UserId => GetClaimValue(IdentityProvider.UserIdClaim);
        public string GetClaimValue(string claimName)
        {
            var value = User.FindFirst(claimName)?.Value;
            
            return !string.IsNullOrEmpty(value) ? value : throw new ArgumentException($"Claim '{claimName}' not found");
        }
    }
}
