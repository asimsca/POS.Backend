using POS.Backend.DTO;
using POS.Backend.DTO.Request.Customer;
using POS.Backend.DTO.Response.Customer;
using POS.Backend.Helper.Logging;
using POS.Backend.Infrastructure.BusinessLogic.Interfaces;
using POS.Backend.Infrastructure.DataLogic.Database.Interfaces;
using System.Net;

namespace POS.Backend.Infrastructure.BusinessLogic.Implementation
{
    public class CustomerLogic : ICustomerLogic
    {
        private readonly ICustomerDb customerDb;
        private readonly LogHelper logHelper;

        public CustomerLogic(ICustomerDb customerDb, LogHelper logHelper)
        {
            this.customerDb = customerDb;
            this.logHelper = logHelper;
        }

        public async Task<BaseResponse<string>> AddCustomer(AddCustomerRequest request)
        {
            var baseResponse = new BaseResponse<string>();

            try
            {
                baseResponse = await this.customerDb.AddCustomer(request);
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

        public async Task<BaseResponse<string>> UpdateCustomer(UpdateCustomerRequest request)
        {
            var baseResponse = new BaseResponse<string>();

            try
            {
                baseResponse = await this.customerDb.UpdateCustomer(request);
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

        public async Task<BaseResponse<string>> DeleteCustomer(DeleteCustomerRequest request)
        {
            var baseResponse = new BaseResponse<string>();

            try
            {
                baseResponse = await this.customerDb.DeleteCustomer(request);
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

        public async Task<BaseResponse<List<GetCustomerResponse>>> GetAllCustomers()
        {
            var baseResponse = new BaseResponse<List<GetCustomerResponse>>();

            try
            {
                baseResponse = await this.customerDb.GetAllCustomers();
            }
            catch (Exception ex)
            {
                baseResponse.IsSuccess = false;
                baseResponse.ResponseCode = HttpStatusCode.InternalServerError;
                baseResponse.Message = ex.Message;
                baseResponse.Data = null;
                var logRequest = "";
                this.logHelper.LogException(logRequest, ex);
            }

            return baseResponse;
        }
    }
}
