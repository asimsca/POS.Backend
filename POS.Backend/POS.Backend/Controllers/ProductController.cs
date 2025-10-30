using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Backend.DTO.Response.Product;
using POS.Backend.DTO;
using POS.Backend.Infrastructure.BusinessLogic.Interfaces;

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
    }
}
