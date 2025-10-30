using POS.Backend.DTO.Request.Customer;
using POS.Backend.DTO.Response.Customer;
using POS.Backend.DTO;

namespace POS.Backend.Infrastructure.BusinessLogic.Interfaces
{
    public interface ICustomerLogic
    {
        Task<BaseResponse<string>> AddCustomer(AddCustomerRequest request);
        Task<BaseResponse<string>> UpdateCustomer(UpdateCustomerRequest request);
        Task<BaseResponse<string>> DeleteCustomer(DeleteCustomerRequest request);
        Task<BaseResponse<List<GetCustomerResponse>>> GetAllCustomers();
    }
}
