namespace MilkStore.API.Models.PaymentModel
{
    public class ResponsePaymentModel
    {
        public int PaymentID { get; set; }
        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public int? OrderID { get; set; }
        public int Delete { get; set; }
    }
}
