using POS.Backend.DTO;
using POS.Backend.DTO.Request.Sale;
using POS.Models;

namespace POS.Backend.Infrastructure.BusinessLogic.Interfaces
{
    public interface ISaleLogic
    {
        Task<BaseResponse<string>> AddSale(AddSaleRequest request);
    }
}