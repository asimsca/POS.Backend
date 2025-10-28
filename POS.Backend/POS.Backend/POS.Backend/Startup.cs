using System.Text;
using POS.Backend.Helper.ActionFilter;
using POS.Backend.Helper.Auth;
using POS.Backend.Helper.Logging;
using POS.Backend.Infrastructure.DependencyInjection;
using POS.Backend.Middlewares;
using POS.Backend.Models.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IO;


namespace POS.Backend
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthServices();
            services.AddHttpClient(); // for chatgpt or thirdparty api call Add this line

            // Define a named CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontendApp", builder =>
                {
                    builder.WithOrigins(
                        "http://localhost:4200",          // local dev
                        "https://asimsca.github.io"               // deployed frontend
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            // BIND AppSettings to our custom class
            services.Configure<AppSettings>(_configuration);
            //services.AddControllers();
            services.AddControllers(options =>
            {
                // Add the ValidateModelAttribute filter globally to all controllers action
                options.Filters.Add<ValidateModelAttributeFilter>();
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSingleton<LogHelper>(); //loggig helper
            services.AddSingleton<ValidateModelAttributeFilter>(); //ValidateModelAttributeFilter
            // Disable automatic 400 Bad Request response when model validation fails.
            // This lets us handle model validation manually, for example in a custom action filter "ValidateModelAttributeFilter".
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddApplicationServices(); // Your custom DI setup DependencyInjectionExtensions
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Add Global Exception Middleware EARLY in the pipeline
            //app.UseMiddleware<GlobalExceptionMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Ensure uploads folder exists inside wwwroot
            //var uploadsPath = Path.Combine(env.WebRootPath, "uploads");
            //if (!Directory.Exists(uploadsPath))
            //{
            //    Directory.CreateDirectory(uploadsPath);
            //}

            var webRootPath = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsPath = Path.Combine(webRootPath, "uploads");

            // Make sure the folder exists
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            // Serve static files from wwwroot/uploads
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(uploadsPath),
                RequestPath = "/uploads",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                }
            });

            app.UseRouting();

            // Enable the CORS policy
            app.UseCors("AllowFrontendApp");

            app.UseAuthentication(); // Don't forget this line!
            app.UseAuthorization();

            // custom request/response logging middleware
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
