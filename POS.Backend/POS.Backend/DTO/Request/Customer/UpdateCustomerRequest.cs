namespace POS.Backend.DTO.Request.Customer
{
    public class UpdateCustomerRequest : AddCustomerRequest
    {
        public Guid CustomerId { get; set; }
    }
}
