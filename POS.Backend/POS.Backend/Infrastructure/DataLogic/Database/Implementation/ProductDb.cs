using Microsoft.Extensions.Options;
using Npgsql;
using POS.Backend.DTO.Response.Product;
using POS.Backend.DTO;
using POS.Backend.Infrastructure.DataLogic.Database.Interfaces;
using POS.Backend.Models.Configuration;
using System.Data;
using System.Net;
using POS.Backend.Helper.Logging;

namespace POS.Backend.Infrastructure.DataLogic.Database.Implementation
{
    public class ProductDb : IProductDb
    {
        private readonly LogHelper logHelper;
        private readonly IOptions<AppSettings> config;

        public ProductDb(LogHelper logHelper, IOptions<AppSettings> config)
        {
            this.logHelper = logHelper;
            this.config = config;
        }

        public async Task<BaseResponse<List<GetProductResponse>>> GetAllProducts()
        {
            var baseResponse = new BaseResponse<List<GetProductResponse>>
            {
                Data = new List<GetProductResponse>()
            };

            try
            {
                await using var conn = new NpgsqlConnection(config.Value.ConnectionStrings.PosDb);
                await conn.OpenAsync();

                // Start a transaction (cursor lives within this)
                await using var transaction = await conn.BeginTransactionAsync();

                await using var cmd = new NpgsqlCommand("sp_get_products", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Define parameters
                cmd.Parameters.Add(new NpgsqlParameter("out_cursor", NpgsqlTypes.NpgsqlDbType.Refcursor)
                {
                    Direction = ParameterDirection.Output
                });
                cmd.Parameters.Add(new NpgsqlParameter("out_is_success", NpgsqlTypes.NpgsqlDbType.Boolean)
                {
                    Direction = ParameterDirection.Output
                });
                cmd.Parameters.Add(new NpgsqlParameter("out_message", NpgsqlTypes.NpgsqlDbType.Text)
                {
                    Direction = ParameterDirection.Output
                });

                // Execute SP
                await cmd.ExecuteNonQueryAsync();

                //baseResponse.IsSuccess = (bool)cmd.Parameters["out_is_success"].Value;
                //baseResponse.Message = cmd.Parameters["out_message"].Value.ToString();

                var isSuccess = Convert.ToBoolean(cmd.Parameters["out_is_success"].Value);
                var message = cmd.Parameters["out_message"].Value?.ToString() ?? "";

                // Fetch data from cursor
                string cursorName = cmd.Parameters["out_cursor"].Value?.ToString() ?? "";

                if (!string.IsNullOrEmpty(cursorName))
                {
                    // ✅ Must use same connection & transaction context for FETCH
                    using var fetchCmd = new NpgsqlCommand($"FETCH ALL IN \"{cursorName}\";", conn);
                    using var reader = await fetchCmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        baseResponse.Data.Add(new GetProductResponse
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Code = reader.IsDBNull(2) ? null : reader.GetString(2),
                            CategoryId = reader.IsDBNull(3) ? null : reader.GetGuid(3),
                            Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                            UnitPrice = reader.GetDecimal(5),
                            CostPrice = reader.GetDecimal(6),
                            StockQuantity = reader.GetInt32(7),
                            CreatedDate = reader.GetDateTime(8),
                            CreatedBy = reader.GetGuid(9)
                        });
                    }
                }

                // Commit transaction only after fetching
                await transaction.CommitAsync();

                baseResponse.IsSuccess = isSuccess;
                baseResponse.Message = message;
                baseResponse.ResponseCode = isSuccess ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "Error retrieving products: " + ex.Message;
            }

            return baseResponse;
        }

    }
}
