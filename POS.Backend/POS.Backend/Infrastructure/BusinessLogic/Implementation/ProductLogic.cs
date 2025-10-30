using POS.Backend.DTO.Response.Product;
using POS.Backend.DTO;
using POS.Backend.Infrastructure.BusinessLogic.Interfaces;
using System.Net;
using POS.Backend.Helper.Logging;
using POS.Backend.Infrastructure.DataLogic.Database.Interfaces;
using POS.Backend.DTO.Request.Product;

namespace POS.Backend.Infrastructure.BusinessLogic.Implementation
{
    public class ProductLogic : IProductLogic
    {
        private readonly IProductDb productDb;
        private readonly LogHelper logHelper;

        public ProductLogic(IProductDb productDb, LogHelper logHelper)
        {
            this.productDb = productDb;
            this.logHelper = logHelper;
        }

        

        public async Task<BaseResponse<string>> DeleteProduct(DeleteProductRequest request)
        {
            var baseResponse = new BaseResponse<string>();

            try
            {
                baseResponse = await this.productDb.DeleteProduct(request);
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.ResponseCode = HttpStatusCode.InternalServerError;
                baseResponse.Message = ex.Message;
                baseResponse.Data = null;
                var logRequest = $"Request : {request}";
                this.logHelper.LogException(logRequest, ex);
            }

            return baseResponse;
        }
        public async Task<BaseResponse<string>> AddProduct(AddProductRequest request)
        {
            var baseResponse = new BaseResponse<string>();

            try
            {
                baseResponse = await this.productDb.AddProduct(request);
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.ResponseCode = HttpStatusCode.InternalServerError;
                baseResponse.Message = ex.Message;
                baseResponse.Data = null;
                var logRequest = $"Request : {request}";
                this.logHelper.LogException(logRequest, ex);
            }

            return baseResponse;
        }

        public async Task<BaseResponse<List<GetProductResponse>>> GetAllProducts()
        {
            var baseResponse = new BaseResponse<List<GetProductResponse>>();

            try
            {
                baseResponse = await this.productDb.GetAllProducts();
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.ResponseCode = HttpStatusCode.InternalServerError;
                baseResponse.Message = ex.Message;
                baseResponse.Data = null;
                var logRequest = $"";
                this.logHelper.LogException(logRequest, ex);
            }

            return baseResponse;
        }


        public async Task<BaseResponse<string>> UpdateProduct(UpdateProductRequest request)
        {
            var baseResponse = new BaseResponse<string>();

            try
            {
                baseResponse = await this.productDb.UpdateProduct(request);
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.ResponseCode = HttpStatusCode.InternalServerError;
                baseResponse.Message = ex.Message;
                baseResponse.Data = null;
                var logRequest = $"Request : {request}";
                this.logHelper.LogException(logRequest, ex);
            }

            return baseResponse;
        }
    }
}
