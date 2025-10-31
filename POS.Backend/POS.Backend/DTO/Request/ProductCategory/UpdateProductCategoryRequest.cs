namespace POS.Backend.DTO.Request.ProductCategory
{
    public class UpdateProductCategoryRequest : AddProductCategoryRequest
    {
        public Guid ProductCategoryId { get; set; }
    }
}
