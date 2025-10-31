using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Backend.DTO.Request.ProductCategory;
using POS.Backend.DTO.Response.ProductCategory;
using POS.Backend.DTO;
using POS.Backend.Infrastructure.BusinessLogic.Interfaces;

namespace POS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryLogic productCategoryLogic;

        public ProductCategoryController(IProductCategoryLogic categoryLogic)
        {
            this.productCategoryLogic = categoryLogic;
        }

        [HttpGet("GetAllProductCategories")]
        public async Task<BaseResponse<List<GetProductCategoryResponse>>> GetAllProductCategories()
        {
            var resp = await this.productCategoryLogic.GetAllProductCategories();
            return resp;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddProductCategory(AddProductCategoryRequest request)
        {
            var response = await productCategoryLogic.AddProductCategory(request);
            return StatusCode((int)response.ResponseCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProductCategory(UpdateProductCategoryRequest request)
        {
            var response = await productCategoryLogic.UpdateProductCategory(request);
            return StatusCode((int)response.ResponseCode, response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteProductCategory(DeleteProductCategoryRequest request)
        {
            var response = await productCategoryLogic.DeleteProductCategory(request);
            return StatusCode((int)response.ResponseCode, response);
        }
    }
}
