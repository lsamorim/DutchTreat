namespace DutchTreat.Data.Entities
{
    public class OrderItem : Entity
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Order Order { get; set; }
    }
}