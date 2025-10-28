using POS.Backend.DTO.Response.Auth;
using POS.Backend.DTO;
using POS.Backend.DTO.Request.Auth;

namespace POS.Backend.Infrastructure.DataLogic.Database.Interfaces
{
    public interface IAuthDb
    {
        Task<BaseResponse<LoginResponse>> Login(LoginRequest loginRequest);

        Task<BaseResponse<string>> Register(RegisterRequest registerRequest);
    }
}
