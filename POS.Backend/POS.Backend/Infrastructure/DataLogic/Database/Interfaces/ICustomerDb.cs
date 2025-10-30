using POS.Backend.DTO.Response.Customer;
using POS.Backend.DTO;
using POS.Backend.DTO.Request.Customer;

namespace POS.Backend.Infrastructure.DataLogic.Database.Interfaces
{
    public interface ICustomerDb
    {
        Task<BaseResponse<string>> AddCustomer(AddCustomerRequest request);
        Task<BaseResponse<string>> UpdateCustomer(UpdateCustomerRequest request);
        Task<BaseResponse<string>> DeleteCustomer(DeleteCustomerRequest request);
        Task<BaseResponse<List<GetCustomerResponse>>> GetAllCustomers();
    }
}
