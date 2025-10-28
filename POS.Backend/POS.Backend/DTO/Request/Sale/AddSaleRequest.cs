using POS.Backend.DTO.Request.Sale;
using System;
using System.Collections.Generic;

namespace POS.Models
{
    public class AddSaleRequest
    {
        public Guid CustomerId { get; set; }
        public Guid PaymentTypeId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SaleItem> Items { get; set; } = new();
        public string? Remarks { get; set; }
    }
}
