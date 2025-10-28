using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace POS.Backend.Helper.Auth
{
    public static class AuthHelper
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = AuthOptions.Issuer, // Ideally: _configuration["Jwt:Issuer"]
                        ValidAudience = AuthOptions.Audience, // Ideally: _configuration["Jwt:Audience"]
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()   // Or _configuration["Jwt:SecretKey"]
                    };
                });

            services.AddAuthorization(ConfigureAuthorization);
            services.AddTransient<IdentityProvider>();
            services.AddHttpContextAccessor();
            services.AddTransient<UserAccessor>();


            return services;

        }

        public static void ConfigureAuthorization(AuthorizationOptions options)
        {
            // Set the default and fallback policy to require authenticated users using JWT Bearer scheme

            // DefaultPolicy  applies when you use [Authorize] on a controller or action
            // but don't specify any custom policy. It makes sure the user is authenticated.

            // FallbackPolicy used when there's no [Authorize] at all on a controller or action.
            // It's a good way to secure everything by default unless you explicitly allow anonymous access.

            // JWT Scheme tells the app to expect and validate a JWT token for authentication.

            options.DefaultPolicy = options.FallbackPolicy = new AuthorizationPolicyBuilder()
                                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                    .RequireAuthenticatedUser()
                                    .Build();

            // Add role-based authorization policies using role names
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));  // "Admin" role name

            options.AddPolicy("UserOrAdmin", policy =>
                policy.RequireRole("Admin", "User"));  // "Admin" or "User" roles

            options.AddPolicy("UserOnly", policy =>
                policy.RequireRole("User"));  // "User" role name
        }
    }
}
