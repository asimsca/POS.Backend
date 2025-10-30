namespace POS.Backend.DTO.Request.Product
{
    public class AddProductRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid? CategoryId { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; }
    }
}
