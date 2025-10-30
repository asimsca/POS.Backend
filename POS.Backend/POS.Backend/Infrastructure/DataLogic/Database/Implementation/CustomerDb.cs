using Microsoft.Extensions.Options;
using Npgsql;
using POS.Backend.DTO;
using POS.Backend.DTO.Request.Customer;
using POS.Backend.DTO.Response.Customer;
using POS.Backend.Helper.Auth;
using POS.Backend.Helper.Logging;
using POS.Backend.Infrastructure.DataLogic.Database.Interfaces;
using POS.Backend.Models.Configuration;
using System.Data;
using System.Net;

namespace POS.Backend.Infrastructure.DataLogic.Database.Implementation
{
    public class CustomerDb : ICustomerDb
    {
        private readonly LogHelper logHelper;
        private readonly IOptions<AppSettings> config;
        private readonly UserAccessor userAccessor;

        public CustomerDb(LogHelper logHelper, IOptions<AppSettings> config, UserAccessor userAccessor)
        {
            this.logHelper = logHelper;
            this.config = config;
            this.userAccessor = userAccessor;
        }

        public async Task<BaseResponse<List<GetCustomerResponse>>> GetAllCustomers()
        {
            var baseResponse = new BaseResponse<List<GetCustomerResponse>>
            {
                Data = new List<GetCustomerResponse>()
            };

            try
            {
                await using var conn = new NpgsqlConnection(config.Value.ConnectionStrings.PosDb);
                await conn.OpenAsync();

                await using var transaction = await conn.BeginTransactionAsync();

                await using var cmd = new NpgsqlCommand("sp_get_customers", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new NpgsqlParameter("out_cursor", NpgsqlTypes.NpgsqlDbType.Refcursor)
                { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_is_success", NpgsqlTypes.NpgsqlDbType.Boolean)
                { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_message", NpgsqlTypes.NpgsqlDbType.Text)
                { Direction = ParameterDirection.Output });

                await cmd.ExecuteNonQueryAsync();

                var isSuccess = Convert.ToBoolean(cmd.Parameters["out_is_success"].Value);
                var message = cmd.Parameters["out_message"].Value?.ToString() ?? "";

                string cursorName = cmd.Parameters["out_cursor"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(cursorName))
                {
                    using var fetchCmd = new NpgsqlCommand($"FETCH ALL IN \"{cursorName}\";", conn);
                    using var reader = await fetchCmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        baseResponse.Data.Add(new GetCustomerResponse
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Email = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Address = reader.IsDBNull(4) ? null : reader.GetString(4),
                            CreatedDate = reader.GetDateTime(5),
                            CreatedBy = reader.GetGuid(6)
                        });
                    }
                }

                await transaction.CommitAsync();

                baseResponse.IsSuccess = isSuccess;
                baseResponse.Message = message;
                baseResponse.ResponseCode = isSuccess ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "Error retrieving customers: " + ex.Message;
                this.logHelper.LogException(null, ex);
            }

            return baseResponse;
        }

        public async Task<BaseResponse<string>> AddCustomer(AddCustomerRequest request)
        {
            var baseResponse = new BaseResponse<string>();

            try
            {
                await using var conn = new NpgsqlConnection(config.Value.ConnectionStrings.PosDb);
                await using var cmd = new NpgsqlCommand("sp_add_customer", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("in_name", request.Name);
                cmd.Parameters.AddWithValue("in_email", (object?)request.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("in_phone", (object?)request.Phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("in_address", (object?)request.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("in_user_id", Guid.Parse(this.userAccessor.UserId));

                cmd.Parameters.Add(new NpgsqlParameter("out_message", NpgsqlTypes.NpgsqlDbType.Text)
                { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_is_success", NpgsqlTypes.NpgsqlDbType.Boolean)
                { Direction = ParameterDirection.Output });

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                var isSuccess = Convert.ToBoolean(cmd.Parameters["out_is_success"].Value);
                var message = cmd.Parameters["out_message"].Value?.ToString() ?? "";

                baseResponse.IsSuccess = isSuccess;
                baseResponse.Message = message;
                baseResponse.ResponseCode = isSuccess ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                baseResponse.Data = message;
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "Error adding customer: " + ex.Message;
                this.logHelper.LogException(request, ex);
            }

            return baseResponse;
        }

        public async Task<BaseResponse<string>> UpdateCustomer(UpdateCustomerRequest request)
        {
            var baseResponse = new BaseResponse<string>();

            try
            {
                await using var conn = new NpgsqlConnection(config.Value.ConnectionStrings.PosDb);
                await using var cmd = new NpgsqlCommand("sp_update_customer", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("in_id", request.CustomerId);
                cmd.Parameters.AddWithValue("in_name", request.Name);
                cmd.Parameters.AddWithValue("in_email", (object?)request.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("in_phone", (object?)request.Phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("in_address", (object?)request.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("in_user_id", Guid.Parse(this.userAccessor.UserId));

                cmd.Parameters.Add(new NpgsqlParameter("out_message", NpgsqlTypes.NpgsqlDbType.Text)
                { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_is_success", NpgsqlTypes.NpgsqlDbType.Boolean)
                { Direction = ParameterDirection.Output });

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                var isSuccess = Convert.ToBoolean(cmd.Parameters["out_is_success"].Value);
                var message = cmd.Parameters["out_message"].Value?.ToString() ?? "";

                baseResponse.IsSuccess = isSuccess;
                baseResponse.Message = message;
                baseResponse.ResponseCode = isSuccess ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                baseResponse.Data = message;
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "Error updating customer: " + ex.Message;
                this.logHelper.LogException(request, ex);
            }

            return baseResponse;
        }

        public async Task<BaseResponse<string>> DeleteCustomer(DeleteCustomerRequest request)
        {
            var baseResponse = new BaseResponse<string>();

            try
            {
                await using var conn = new NpgsqlConnection(config.Value.ConnectionStrings.PosDb);
                await using var cmd = new NpgsqlCommand("sp_delete_customer", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("in_id", request.CustomerId);
                cmd.Parameters.AddWithValue("in_user_id", Guid.Parse(this.userAccessor.UserId));

                cmd.Parameters.Add(new NpgsqlParameter("out_message", NpgsqlTypes.NpgsqlDbType.Text)
                { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_is_success", NpgsqlTypes.NpgsqlDbType.Boolean)
                { Direction = ParameterDirection.Output });

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                var isSuccess = Convert.ToBoolean(cmd.Parameters["out_is_success"].Value);
                var message = cmd.Parameters["out_message"].Value?.ToString() ?? "";

                baseResponse.IsSuccess = isSuccess;
                baseResponse.Message = message;
                baseResponse.ResponseCode = isSuccess ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
                baseResponse.Data = message;
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "Error deleting customer: " + ex.Message;
                this.logHelper.LogException(request, ex);
            }

            return baseResponse;
        }
    }
}
