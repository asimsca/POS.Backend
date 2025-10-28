using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace POS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestRolePolicyController : ControllerBase
    {
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("admin-only")]
        public IActionResult GetAdminData()
        {
            return Ok("This is an admin-only endpoint.");
        }

        [Authorize(Policy = "UserOrAdmin")]
        [HttpGet("user-or-admin")]
        public IActionResult GetUserOrAdminData()
        {
            return Ok("This is accessible to both users and admins.");
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("user-only")]
        public IActionResult GetUserData()
        {
            return Ok("This is a user-only endpoint.");
        }

    }
}
