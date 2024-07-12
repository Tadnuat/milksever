namespace MilkStore.API.Models.OrderModel
{
    public class RequestCreateOrderModel
    {
        public int OrderID { get; set; }
        public int? CustomerID { get; set; }

        public int? DeliveryManID { get; set; }

        public DateTime? OrderDate { get; set; } // Chuyển sang DateTime

        public string ShippingAddress { get; set; } = string.Empty;

        public decimal? TotalAmount { get; set; }

        public int? StorageID { get; set; }

        public string DeliveryName { get; set; } = string.Empty; // Đặt giá trị mặc định nếu cần

        public string DeliveryPhone { get; set; } = string.Empty; // Đặt giá trị mặc định nếu cần

        public string PaymentMethod { get; set; } = string.Empty; // Đặt giá trị mặc định nếu cần

        public string Status { get; set; } = string.Empty; // Đặt giá trị mặc định nếu cần

        public int Delete {  get; set; }
    }
}
