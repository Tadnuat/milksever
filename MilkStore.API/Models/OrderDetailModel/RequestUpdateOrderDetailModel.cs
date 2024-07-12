namespace MilkStore.API.Models.OrderDetailModel
{
    public class RequestUpdateOrderDetailModel
    {
        public int? OrderID { get; set; } = null;
        public int? CustomerID { get; set; } // Added CustomerId property
        public int? ProductItemID { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string? ItemName { get; set; }
        public string? Image { get; set; }
        public int? OrderStatus { get; set; }
        public decimal? Discount { get; set; } // Added Discount property
        public int Delete { get; set; }
    }
}
