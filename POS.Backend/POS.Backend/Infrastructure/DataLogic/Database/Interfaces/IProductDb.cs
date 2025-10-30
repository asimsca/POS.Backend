using POS.Backend.DTO.Response.Product;
using POS.Backend.DTO;

namespace POS.Backend.Infrastructure.DataLogic.Database.Interfaces
{
    public interface IProductDb
    {
        Task<BaseResponse<List<GetProductResponse>>> GetAllProducts();
    }
}
