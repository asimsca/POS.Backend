using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Backend.DTO.Response.Product;
using POS.Backend.DTO;
using POS.Backend.Infrastructure.BusinessLogic.Interfaces;
using POS.Backend.Infrastructure.BusinessLogic.Implementation;
using POS.Backend.DTO.Request.Product;

namespace POS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductLogic productLogic;

        public ProductController(IProductLogic productLogic)
        {
            this.productLogic = productLogic;
        }

        [HttpGet("GetAllProducts")]
        public async Task<BaseResponse<List<GetProductResponse>>> GetAllProducts()
        {
            var resp = await this.productLogic.GetAllProducts();
            return resp;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            var response = await productLogic.AddProduct(request);
            return StatusCode((int)response.ResponseCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProduct(UpdateProductRequest request)
        {
            var response = await productLogic.UpdateProduct(request);
            return StatusCode((int)response.ResponseCode, response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteProduct(DeleteProductRequest request)
        {
            var response = await productLogic.DeleteProduct(request);
            return StatusCode((int)response.ResponseCode, response);
        }
    }
}
