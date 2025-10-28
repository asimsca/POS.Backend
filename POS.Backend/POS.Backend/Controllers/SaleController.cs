using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Backend.DTO;
using POS.Backend.DTO.Request.Sale;
using POS.Backend.Infrastructure.BusinessLogic.Interfaces;
using POS.Models;

namespace POS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleLogic saleLogic;

        public SaleController(ISaleLogic saleLogic)
        {
            this.saleLogic = saleLogic;
        }

        [HttpPost("AddSale")]
        public async Task<BaseResponse<string>> AddSale([FromBody] AddSaleRequest request)
        {
            var resp = await this.saleLogic.AddSale(request);
            return resp;
        }
    }
}
