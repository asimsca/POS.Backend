using System.Net;
using POS.Backend.DTO;
using POS.Backend.DTO.Request.Auth;
using POS.Backend.DTO.Response.Auth;
using POS.Backend.Helper.Logging;
using POS.Backend.Infrastructure.BusinessLogic.Interfaces;
using POS.Backend.Infrastructure.DataLogic.Database.Interfaces;
using Microsoft.AspNetCore.Identity.Data;

namespace POS.Backend.Infrastructure.BusinessLogic.Implementation
{
    public class AuthLogic : IAuthLogic
    {
        private readonly IAuthDb authDb;
        private readonly LogHelper logHelper;
        public AuthLogic(IAuthDb authDb, LogHelper logHelper)
        {
            this.authDb = authDb;
            this.logHelper = logHelper;
        }
        public async Task<BaseResponse<LoginResponse>> Login(DTO.Request.Auth.LoginRequest loginRequest)
        {
            BaseResponse<LoginResponse> baseResponse = new BaseResponse<LoginResponse>();
            try
            {
                baseResponse = await this.authDb.Login(loginRequest);
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.ResponseCode = HttpStatusCode.InternalServerError;
                baseResponse.Message = ex.Message;
                baseResponse.Data = null;
                this.logHelper.LogException(loginRequest, ex);
            }
            return baseResponse;
        }

        public async Task<BaseResponse<string>> Register(DTO.Request.Auth.RegisterRequest registerRequest)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();
            try
            {
                baseResponse = await this.authDb.Register(registerRequest);
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.ResponseCode = HttpStatusCode.InternalServerError;
                baseResponse.Message = ex.Message;
                baseResponse.Data = null;
                this.logHelper.LogException(registerRequest, ex);
            }
            return baseResponse;
        }
    }
}
