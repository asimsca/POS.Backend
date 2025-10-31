using POS.Backend.DTO.Request.ProductCategory;
using POS.Backend.DTO.Response.ProductCategory;
using POS.Backend.DTO;

namespace POS.Backend.Infrastructure.BusinessLogic.Interfaces
{
    public interface IProductCategoryLogic
    {
        Task<BaseResponse<string>> AddProductCategory(AddProductCategoryRequest request);
        Task<BaseResponse<string>> UpdateProductCategory(UpdateProductCategoryRequest request);
        Task<BaseResponse<string>> DeleteProductCategory(DeleteProductCategoryRequest request);
        Task<BaseResponse<List<GetProductCategoryResponse>>> GetAllProductCategories();
    }
}
