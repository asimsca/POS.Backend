using POS.Backend.DTO;
using POS.Backend.DTO.Request.Auth;
using POS.Backend.DTO.Response.Auth;
using POS.Backend.Helper.Logging;
using POS.Backend.Infrastructure.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace POS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthLogic authLogic;
        private readonly LogHelper logHelper;


        public AuthController(IAuthLogic authLogic, LogHelper logHelper)
        {
            this.authLogic = authLogic;
            this.logHelper = logHelper;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<BaseResponse<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            var resp = await this.authLogic.Login(loginRequest);
            //this.logHelper.LogRequestResponse(loginRequest, resp);
            return resp;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<BaseResponse<string>> Register([FromBody] RegisterRequest registerRequest)
        {
            var resp = await this.authLogic.Register(registerRequest);
            return resp;
        }
    }
}
