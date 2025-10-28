using System.Net;
using POS.Backend.DTO;
using POS.Backend.DTO.Request.Sale;
using POS.Backend.Helper.Logging;
using POS.Backend.Infrastructure.BusinessLogic.Interfaces;
using POS.Backend.Infrastructure.DataLogic.Database.Interfaces;
using POS.Models;

namespace POS.Backend.Infrastructure.BusinessLogic.Implementation
{
    public class SaleLogic : ISaleLogic
    {
        private readonly ISaleDb saleDb;
        private readonly LogHelper logHelper;

        public SaleLogic(ISaleDb saleDb, LogHelper logHelper)
        {
            this.saleDb = saleDb;
            this.logHelper = logHelper;
        }

        public async Task<BaseResponse<string>> AddSale(AddSaleRequest request)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();

            try
            {
                baseResponse = await this.saleDb.AddSale(request);
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
