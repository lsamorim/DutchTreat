using System;
using System.Collections.Generic;

namespace DutchTreat.Data.Entities
{
    public class Order : Entity
    {
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public StoreUser User { get; set; }
    }
}
