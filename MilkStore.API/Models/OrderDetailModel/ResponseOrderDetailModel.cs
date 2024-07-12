namespace MilkStore.API.Models.OrderDetailModel
{
    public class ResponseOrderDetailModel
    {
        public int OrderDetailID { get; set; }

        public int? OrderID { get; set; } = null;

        public int? CustomerID { get; set; }

        public int? ProductItemID { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        public string ItemName { get; set; } = null!;

        public string? Image { get; set; }

        public int? OrderStatus { get; set; }

        public decimal? Discount { get; set; }

        public decimal Total { get; set; } // Add Total property
        public int Delete { get; set; }
    }
}
