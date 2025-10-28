using POS.Backend.DTO.Response.Auth;
using POS.Backend.DTO;
using POS.Backend.DTO.Request.Auth;
using Microsoft.AspNetCore.Mvc;

namespace POS.Backend.Infrastructure.BusinessLogic.Interfaces
{
    public interface IAuthLogic
    {
        Task<BaseResponse<LoginResponse>> Login(LoginRequest loginRequest);

        Task<BaseResponse<string>> Register(RegisterRequest registerRequest);
    }
}
