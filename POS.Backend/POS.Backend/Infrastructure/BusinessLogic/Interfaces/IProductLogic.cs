using POS.Backend.DTO.Response.Product;
using POS.Backend.DTO;
using POS.Backend.DTO.Request.Product;

namespace POS.Backend.Infrastructure.BusinessLogic.Interfaces
{
    public interface IProductLogic
    {
        Task<BaseResponse<List<GetProductResponse>>> GetAllProducts();
        Task<BaseResponse<string>> AddProduct(AddProductRequest request);
        Task<BaseResponse<string>> UpdateProduct(UpdateProductRequest request);
        Task<BaseResponse<string>> DeleteProduct(DeleteProductRequest request);
    }
}
