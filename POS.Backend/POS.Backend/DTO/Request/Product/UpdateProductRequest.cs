namespace POS.Backend.DTO.Request.Product
{
    public class UpdateProductRequest : AddProductRequest
    {
        public Guid ProductId { get; set; }
    }
}
