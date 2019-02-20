using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IHostingEnvironment _hosting;

        public DutchSeeder(DutchContext ctx, IHostingEnvironment hosting)
        {
            _ctx = ctx;
            _hosting = hosting;
        }

        public void Seed()
        {
            _ctx.Database.EnsureCreated();

            if (!_ctx.Products.Any())
            {
                // Need to create sample data
                var filePath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);

                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _ctx.Products.AddRange(products);
                
                var order = new Order
                {
                    OrderDate = DateTime.UtcNow,
                    OrderNumber = "12345"
                };
                order.Items = new List<OrderItem>()
                {
                    new OrderItem
                    {
                        Product = products.First(),
                        UnitPrice = products.First().Price,
                        Quantity = 5,
                    }
                };
                
                _ctx.Orders.Add(order);

                _ctx.SaveChanges();
            }
        }
    }
}
