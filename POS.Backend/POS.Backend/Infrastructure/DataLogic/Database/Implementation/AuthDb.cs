using System.Data;
using System.Net;
using System.Text.Json.Serialization;
using POS.Backend.DTO;
using POS.Backend.DTO.Request.Auth;
using POS.Backend.DTO.Response.Auth;
using POS.Backend.Helper.Auth;
using POS.Backend.Helper.Logging;
using POS.Backend.Infrastructure.DataLogic.Database.Interfaces;
using POS.Backend.Models.Configuration;
using POS.Backend.Models.Costants;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;

namespace POS.Backend.Infrastructure.DataLogic.Database.Implementation
{
    public class AuthDb : IAuthDb
    {
        private readonly IOptions<AppSettings> config;
        private readonly IdentityProvider identityProvider;
        //private readonly ILogger<AuthDb> _logger;
        private readonly LogHelper logHelper;


        public AuthDb(IOptions<AppSettings> configuration, IdentityProvider identityProvider, IConfiguration config, LogHelper logHelper)
        {
            this.config = configuration;
            this.identityProvider = identityProvider;
            //_logger = logger;
            this.logHelper = logHelper;
        }

        public async Task<BaseResponse<LoginResponse>> Login(LoginRequest request)
        {
            //_logger.LogInformation("controll is here in db layer of login");
            LoginResponse loginResponse = new LoginResponse();
            BaseResponse<LoginResponse> baseResponse = new BaseResponse<LoginResponse>();

            try
            {
                bool isOtpEnabled = this.config.Value.IsOtpEnabled;


                using var conn = new NpgsqlConnection(config.Value.ConnectionStrings.PosDb);
                using var cmd = new NpgsqlCommand("sp_login_user", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Input parameters
                cmd.Parameters.AddWithValue("in_email", request?.Email);
                cmd.Parameters.AddWithValue("in_password", request.Password);

                // Output parameters
                cmd.Parameters.Add(new NpgsqlParameter("out_user_id", DbType.Int32) { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_full_name", DbType.String) { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_email", DbType.String) { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_is_locked", DbType.Boolean) { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_message", DbType.String) { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_is_success", DbType.Boolean) { Direction = ParameterDirection.Output });

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                var isSuccess = Convert.ToBoolean(cmd.Parameters["out_is_success"].Value);
                var message = cmd.Parameters["out_message"].Value?.ToString() ?? string.Empty;
                var isLocked = (cmd.Parameters["out_is_locked"].Value as bool?) ?? false; // Checks for null, then defaults to false

                if (isSuccess)
                {
                    //if OTP is enabled then we have to verify user through OTP
                    if (isOtpEnabled)
                    {
                        //call an sp where generate OTP random number and add it to table against this user details
                        loginResponse.IsOtpEnable = true;
                    }
                    else //in calse OTP is not enabled
                    {
                        UserInfo userInfo = new UserInfo() //these values will be from response of login sp
                        {
                            FullName = cmd.Parameters["out_full_name"].Value?.ToString() ?? string.Empty,
                            UserId = cmd.Parameters["out_user_id"].Value?.ToString() ?? string.Empty,
                            Email = cmd.Parameters["out_email"].Value?.ToString() ?? string.Empty,
                        };

                        //add this info to db table and get refresh token from there
                        loginResponse.RefreshToken = "23234234"; //this will be from AddOTPUserInfo SP but i have hard coded it
                        loginResponse.AccessToken = this.identityProvider.CreateToken(userInfo);
                        loginResponse.Email = request.Email;
                    }

                    baseResponse.IsSuccess = true;
                    baseResponse.ResponseCode = HttpStatusCode.OK;
                    baseResponse.Message = message;
                    baseResponse.Data = loginResponse;
                }
                else
                {
                    baseResponse.IsSuccess = false;
                    baseResponse.ResponseCode = HttpStatusCode.Unauthorized;
                    baseResponse.Message = message;
                    baseResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.ResponseCode = HttpStatusCode.InternalServerError;
                baseResponse.Message = "Internal error: " + ex.Message;
                baseResponse.Data = null;
                //_logger.LogError(ex, "Login failed for user: {UserName}", request.UserName);
                this.logHelper.LogException(request, ex);
            }
            return baseResponse;
        }

        public async Task<BaseResponse<string>> Register(RegisterRequest request)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();

            try
            {
                using var conn = new NpgsqlConnection(config.Value.ConnectionStrings.PosDb);
                using var cmd = new NpgsqlCommand("sp_add_user", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Input parameters
                cmd.Parameters.AddWithValue("in_full_name", request.FullName);
                cmd.Parameters.AddWithValue("in_email", request.Email);
                cmd.Parameters.AddWithValue("in_password", request.Password);

                // Output parameters
                cmd.Parameters.Add(new NpgsqlParameter("out_message", DbType.String) { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_is_success", DbType.Boolean) { Direction = ParameterDirection.Output });

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                var isSuccess = Convert.ToBoolean(cmd.Parameters["out_is_success"].Value);
                var message = cmd.Parameters["out_message"].Value?.ToString() ?? string.Empty;

                if (isSuccess)
                {
                    baseResponse.IsSuccess = true;
                    baseResponse.Message = message;
                }
                else
                {
                    baseResponse.IsSuccess = false;
                    baseResponse.Message = message;
                }
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.ResponseCode = HttpStatusCode.InternalServerError;
                baseResponse.Message = "Internal error: " + ex.Message;
                baseResponse.Data = null;
                //_logger.LogError(ex, "Login failed for user: {UserName}", request.UserName);
                this.logHelper.LogException(request, ex);
            }

            return baseResponse;
        }

    }
}
