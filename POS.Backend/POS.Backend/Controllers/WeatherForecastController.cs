using System.Security.Claims;
using POS.Backend.Helper.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace POS.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly UserAccessor userAccessor;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, UserAccessor userAccessor)
        {
            _logger = logger;
            this.userAccessor = userAccessor;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        //[Authorize(Policy = "AdminOnly")]
        public IEnumerable<WeatherForecast> Get()
        {
            //var userNme = this.userAccessor.UserName;
            var userId = this.userAccessor.UserId;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
