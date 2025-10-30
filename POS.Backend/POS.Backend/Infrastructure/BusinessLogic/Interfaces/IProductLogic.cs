using POS.Backend.DTO.Response.Product;
using POS.Backend.DTO;

namespace POS.Backend.Infrastructure.BusinessLogic.Interfaces
{
    public interface IProductLogic
    {
        Task<BaseResponse<List<GetProductResponse>>> GetAllProducts();
    }
}
