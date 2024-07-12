using System;
using System.Collections.Generic;

namespace MilkStore.Repo.Entities
{
    public partial class Order
    {
        public int OrderID { get; set; }
        public int? CustomerID { get; set; }
        public int? DeliveryManID { get; set; }
        public DateTime? OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? StorageID { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryPhone { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual DeliveryMan DeliveryMan { get; set; }
        public int Delete { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual Storage Storage { get; set; }
    }
}