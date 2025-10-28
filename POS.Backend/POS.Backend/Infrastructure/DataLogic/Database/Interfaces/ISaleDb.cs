using POS.Backend.DTO;
using POS.Backend.DTO.Request.Sale;
using POS.Models;

namespace POS.Backend.Infrastructure.DataLogic.Database.Interfaces
{
    public interface ISaleDb
    {
        Task<BaseResponse<string>> AddSale(AddSaleRequest request);
    }
}