using System.Data;
using System.Net;
using Npgsql;
using NpgsqlTypes;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using POS.Backend.DTO;
using POS.Backend.DTO.Request.Sale;
using POS.Backend.Helper.Logging;
using POS.Backend.Models.Configuration;
using POS.Backend.Infrastructure.DataLogic.Database.Interfaces;
using POS.Backend.Helper.Auth;
using POS.Models;

namespace POS.Backend.Infrastructure.DataLogic.Database.Implementation
{
    public class SaleDb : ISaleDb
    {
        private readonly LogHelper logHelper;
        private readonly IOptions<AppSettings> config;
        private readonly UserAccessor userAccessor;

        public SaleDb(LogHelper logHelper, IOptions<AppSettings> config, UserAccessor userAccessor)
        {
            this.logHelper = logHelper;
            this.config = config;
            this.userAccessor = userAccessor;
        }

        public async Task<BaseResponse<string>> AddSale(AddSaleRequest request)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();

            try
            {
                using var conn = new NpgsqlConnection(config.Value.ConnectionStrings.PosDb);
                using var cmd = new NpgsqlCommand("sp_add_sale", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("in_customer_id", request.CustomerId);
                cmd.Parameters.AddWithValue("in_payment_type", request.PaymentTypeId);
                cmd.Parameters.AddWithValue("in_total_amount", request.TotalAmount);
                cmd.Parameters.Add(new NpgsqlParameter("in_items", NpgsqlDbType.Jsonb)
                {
                    Value = JsonConvert.SerializeObject(request.Items)
                });
                cmd.Parameters.AddWithValue("in_user_id", Guid.Parse(this.userAccessor.UserId));

                cmd.Parameters.Add(new NpgsqlParameter("out_message", DbType.String) { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_is_success", DbType.Boolean) { Direction = ParameterDirection.Output });

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                var isSuccess = Convert.ToBoolean(cmd.Parameters["out_is_success"].Value);
                var message = cmd.Parameters["out_message"].Value?.ToString() ?? "";

                baseResponse.IsSuccess = isSuccess;
                baseResponse.Message = message;
                baseResponse.ResponseCode = isSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                baseResponse.Data = message;
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.ResponseCode = HttpStatusCode.InternalServerError;
                baseResponse.Message = ex.Message;
                this.logHelper.LogException(request, ex);
            }

            return baseResponse;
        }
    }
}
