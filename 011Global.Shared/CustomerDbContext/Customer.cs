namespace _011Global.Shared.CustomerDbContext
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public int ShippingAddressID { get; set; }
        public int BillingAddressID { get; set; }
        public decimal MonthlyFee { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Subscribed {  get; set; }
    }
}
