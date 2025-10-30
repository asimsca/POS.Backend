using Microsoft.AspNetCore.Mvc;
using POS.Backend.DTO;
using POS.Backend.DTO.Request.Customer;
using POS.Backend.DTO.Response.Customer;
using POS.Backend.Infrastructure.BusinessLogic.Interfaces;

namespace POS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerLogic customerLogic;

        public CustomerController(ICustomerLogic customerLogic)
        {
            this.customerLogic = customerLogic;
        }

        [HttpGet("GetAllCustomers")]
        public async Task<BaseResponse<List<GetCustomerResponse>>> GetAllCustomers()
        {
            var resp = await this.customerLogic.GetAllCustomers();
            return resp;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddCustomer(AddCustomerRequest request)
        {
            var response = await customerLogic.AddCustomer(request);
            return StatusCode((int)response.ResponseCode, response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerRequest request)
        {
            var response = await customerLogic.UpdateCustomer(request);
            return StatusCode((int)response.ResponseCode, response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteCustomer(DeleteCustomerRequest request)
        {
            var response = await customerLogic.DeleteCustomer(request);
            return StatusCode((int)response.ResponseCode, response);
        }
    }
}
