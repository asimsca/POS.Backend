using Microsoft.Extensions.Options;
using Npgsql;
using POS.Backend.DTO.Request.ProductCategory;
using POS.Backend.DTO.Response.ProductCategory;
using POS.Backend.DTO;
using POS.Backend.Helper.Auth;
using POS.Backend.Infrastructure.DataLogic.Database.Interfaces;
using POS.Backend.Models.Configuration;
using System.Data;
using System.Net;
using POS.Backend.Helper.Logging;

namespace POS.Backend.Infrastructure.DataLogic.Database.Implementation
{
    public class ProductCategoryDb : IProductCategoryDb
    {
        private readonly LogHelper logHelper;
        private readonly IOptions<AppSettings> config;
        private readonly UserAccessor userAccessor;

        public ProductCategoryDb(LogHelper logHelper, IOptions<AppSettings> config, UserAccessor userAccessor)
        {
            this.logHelper = logHelper;
            this.config = config;
            this.userAccessor = userAccessor;
        }

        public async Task<BaseResponse<List<GetProductCategoryResponse>>> GetAllProductCategories()
        {
            var baseResponse = new BaseResponse<List<GetProductCategoryResponse>>
            {
                Data = new List<GetProductCategoryResponse>()
            };

            try
            {
                await using var conn = new NpgsqlConnection(config.Value.ConnectionStrings.PosDb);
                await conn.OpenAsync();
                await using var transaction = await conn.BeginTransactionAsync();

                await using var cmd = new NpgsqlCommand("sp_get_product_categories", conn)
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

                string cursorName = cmd.Parameters["out_cursor"].Value?.ToString() ?? "";
                bool isSuccess = Convert.ToBoolean(cmd.Parameters["out_is_success"].Value);
                string message = cmd.Parameters["out_message"].Value?.ToString() ?? "";

                if (!string.IsNullOrEmpty(cursorName))
                {
                    await using var fetchCmd = new NpgsqlCommand($"FETCH ALL IN \"{cursorName}\";", conn);
                    await using var reader = await fetchCmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        baseResponse.Data.Add(new GetProductCategoryResponse
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            CreatedDate = reader.GetDateTime(3),
                            CreatedBy = reader.GetGuid(4)
                        });
                    }
                }

                await transaction.CommitAsync();

                baseResponse.IsSuccess = isSuccess;
                baseResponse.Message = baseResponse.Data.Count == 0 ? "No Record Found" : message;
                baseResponse.ResponseCode = isSuccess ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "Error retrieving product categories: " + ex.Message;
                this.logHelper.LogException(null, ex);
            }

            return baseResponse;
        }

        public async Task<BaseResponse<string>> AddProductCategory(AddProductCategoryRequest request)
        {
            var baseResponse = new BaseResponse<string>();

            try
            {
                await using var conn = new NpgsqlConnection(config.Value.ConnectionStrings.PosDb);
                await using var cmd = new NpgsqlCommand("sp_add_product_category", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("in_name", request.Name);
                cmd.Parameters.AddWithValue("in_description", (object?)request.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("in_user_id", Guid.Parse(this.userAccessor.UserId));

                cmd.Parameters.Add(new NpgsqlParameter("out_message", NpgsqlTypes.NpgsqlDbType.Text)
                { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_is_success", NpgsqlTypes.NpgsqlDbType.Boolean)
                { Direction = ParameterDirection.Output });

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                bool isSuccess = Convert.ToBoolean(cmd.Parameters["out_is_success"].Value);
                string message = cmd.Parameters["out_message"].Value?.ToString() ?? "";

                baseResponse.IsSuccess = isSuccess;
                baseResponse.Message = message;
                baseResponse.Data = message;
                baseResponse.ResponseCode = isSuccess ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "Error adding product category: " + ex.Message;
                this.logHelper.LogException(request, ex);
            }

            return baseResponse;
        }

        public async Task<BaseResponse<string>> UpdateProductCategory(UpdateProductCategoryRequest request)
        {
            var baseResponse = new BaseResponse<string>();

            try
            {
                await using var conn = new NpgsqlConnection(config.Value.ConnectionStrings.PosDb);
                await using var cmd = new NpgsqlCommand("sp_update_product_category", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("in_id", request.ProductCategoryId);
                cmd.Parameters.AddWithValue("in_name", request.Name);
                cmd.Parameters.AddWithValue("in_description", (object?)request.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("in_user_id", Guid.Parse(this.userAccessor.UserId));

                cmd.Parameters.Add(new NpgsqlParameter("out_message", NpgsqlTypes.NpgsqlDbType.Text)
                { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_is_success", NpgsqlTypes.NpgsqlDbType.Boolean)
                { Direction = ParameterDirection.Output });

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                bool isSuccess = Convert.ToBoolean(cmd.Parameters["out_is_success"].Value);
                string message = cmd.Parameters["out_message"].Value?.ToString() ?? "";

                baseResponse.IsSuccess = isSuccess;
                baseResponse.Message = message;
                baseResponse.Data = message;
                baseResponse.ResponseCode = isSuccess ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "Error updating product category: " + ex.Message;
                this.logHelper.LogException(request, ex);
            }

            return baseResponse;
        }

        public async Task<BaseResponse<string>> DeleteProductCategory(DeleteProductCategoryRequest request)
        {
            var baseResponse = new BaseResponse<string>();

            try
            {
                await using var conn = new NpgsqlConnection(config.Value.ConnectionStrings.PosDb);
                await using var cmd = new NpgsqlCommand("sp_delete_product_category", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("in_id", request.ProductCategoryId);
                cmd.Parameters.AddWithValue("in_user_id", Guid.Parse(this.userAccessor.UserId));

                cmd.Parameters.Add(new NpgsqlParameter("out_message", NpgsqlTypes.NpgsqlDbType.Text)
                { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new NpgsqlParameter("out_is_success", NpgsqlTypes.NpgsqlDbType.Boolean)
                { Direction = ParameterDirection.Output });

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                bool isSuccess = Convert.ToBoolean(cmd.Parameters["out_is_success"].Value);
                string message = cmd.Parameters["out_message"].Value?.ToString() ?? "";

                baseResponse.IsSuccess = isSuccess;
                baseResponse.Message = message;
                baseResponse.Data = message;
                baseResponse.ResponseCode = isSuccess ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.Message = "Error deleting product category: " + ex.Message;
                this.logHelper.LogException(request, ex);
            }

            return baseResponse;
        }
    }
}
