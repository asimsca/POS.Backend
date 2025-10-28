using System;
using System.Data;
using System.Net;
using POS.Backend.DTO;
using POS.Backend.DTO.Request.POS;
using POS.Backend.DTO.Response.Auth;
using POS.Backend.Helper.Auth;
using POS.Backend.Helper.Logging;
using POS.Backend.Infrastructure.DataLogic.Database.Interfaces;
using POS.Backend.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using static System.Net.Mime.MediaTypeNames;

namespace POS.Backend.Infrastructure.DataLogic.Database.Implementation
{
    public class POSDb : IPOSDb
    {
        private readonly LogHelper logHelper;
        private readonly IOptions<AppSettings> config;
        private readonly UserAccessor userAccessor;
        public POSDb(LogHelper logHelper, IOptions<AppSettings> config, UserAccessor userAccessor)
        {
            this.logHelper = logHelper;
            this.config = config;
            this.userAccessor = userAccessor;
        }
        public async Task<BaseResponse<string>> AddPOS(AddPOSRequest request)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();

            try
            {
                using var conn = new NpgsqlConnection(config.Value.ConnectionStrings.POSDb);
                using var cmd = new NpgsqlCommand("sp_add_POS", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Input parameters
                cmd.Parameters.AddWithValue("in_title", request.Title);
                cmd.Parameters.AddWithValue("in_designation", request.Designation);
                cmd.Parameters.AddWithValue("in_summary", request.Summary);
                cmd.Parameters.AddWithValue("in_profile_picture_url", request.ProfilePictureUrl);
                // Serialize list properties to JSON for json params in SP
                cmd.Parameters.Add(new NpgsqlParameter("in_contacts", NpgsqlDbType.Jsonb)
                {
                    Value = JsonConvert.SerializeObject(request.Contacts)
                });
                cmd.Parameters.Add(new NpgsqlParameter("in_education", NpgsqlDbType.Jsonb)
                {
                    Value = JsonConvert.SerializeObject(request.Education)
                });
                cmd.Parameters.Add(new NpgsqlParameter("in_experience", NpgsqlDbType.Jsonb)
                {
                    Value = JsonConvert.SerializeObject(request.Experience)
                });
                cmd.Parameters.AddWithValue("in_skills", request.Skills);
                cmd.Parameters.AddWithValue("in_user_id", Guid.Parse(this.userAccessor.UserId));
                //cmd.Parameters.AddWithValue("in_user_id",  Guid.Parse("42e3bff6-bc56-4d33-b10b-f5a4b537d409"));
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
                    baseResponse.ResponseCode = HttpStatusCode.OK;
                    baseResponse.Message = message;
                    baseResponse.Data = message;
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

        /// <summary>
        /// AddPOSFIle.
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public async Task<BaseResponse<string>> AddPOSFIle(string fileUrl)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();

            try
            {
                using var conn = new NpgsqlConnection(config.Value.ConnectionStrings.POSDb);
                using var cmd = new NpgsqlCommand("sp_add_POS_file", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Input parameters
                cmd.Parameters.AddWithValue("in_user_id", Guid.Parse(this.userAccessor.UserId));
                //cmd.Parameters.AddWithValue("in_user_id", Guid.Parse("42e3bff6-bc56-4d33-b10b-f5a4b537d409"));
                cmd.Parameters.AddWithValue("in_file_url", fileUrl);
                
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
                    baseResponse.ResponseCode = HttpStatusCode.OK;
                    baseResponse.Message = message;
                    baseResponse.Data = message;
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
                this.logHelper.LogException(fileUrl, ex);
            }
            return baseResponse;
        }
    }
}
