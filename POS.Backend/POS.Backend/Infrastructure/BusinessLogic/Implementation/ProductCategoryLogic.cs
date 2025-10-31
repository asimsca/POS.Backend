using POS.Backend.DTO.Request.ProductCategory;
using POS.Backend.DTO.Response.ProductCategory;
using POS.Backend.DTO;
using POS.Backend.Infrastructure.BusinessLogic.Interfaces;
using System.Net;
using POS.Backend.Infrastructure.DataLogic.Database.Interfaces;
using POS.Backend.Helper.Logging;

namespace POS.Backend.Infrastructure.BusinessLogic.Implementation
{
    public class ProductCategoryLogic : IProductCategoryLogic
    {
        private readonly IProductCategoryDb productCategoryDb;
        private readonly LogHelper logHelper;

        public ProductCategoryLogic(IProductCategoryDb productCategoryDb, LogHelper logHelper)
        {
            this.productCategoryDb = productCategoryDb;
            this.logHelper = logHelper;
        }

        public async Task<BaseResponse<string>> AddProductCategory(AddProductCategoryRequest request)
        {
            var response = new BaseResponse<string>();
            try
            {
                response = await productCategoryDb.AddProductCategory(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ResponseCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                logHelper.LogException($"AddProductCategory Request: {request}", ex);
            }
            return response;
        }

        public async Task<BaseResponse<string>> UpdateProductCategory(UpdateProductCategoryRequest request)
        {
            var response = new BaseResponse<string>();
            try
            {
                response = await productCategoryDb.UpdateProductCategory(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ResponseCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                logHelper.LogException($"UpdateProductCategory Request: {request}", ex);
            }
            return response;
        }

        public async Task<BaseResponse<string>> DeleteProductCategory(DeleteProductCategoryRequest request)
        {
            var response = new BaseResponse<string>();
            try
            {
                response = await productCategoryDb.DeleteProductCategory(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ResponseCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                logHelper.LogException($"DeleteProductCategory Request: {request}", ex);
            }
            return response;
        }

        public async Task<BaseResponse<List<GetProductCategoryResponse>>> GetAllProductCategories()
        {
            var response = new BaseResponse<List<GetProductCategoryResponse>>();
            try
            {
                response = await productCategoryDb.GetAllProductCategories();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ResponseCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                logHelper.LogException($"GetAllCategories Exception", ex);
            }
            return response;
        }
    }
}
