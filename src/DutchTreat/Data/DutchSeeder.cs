using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext ctx, IHostingEnvironment hosting, UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            var user = await _userManager.FindByEmailAsync("lsamorim.it@gmail.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Lucas",
                    LastName = "Amorim",
                    Email = "lsamorim.it@gmail.com",
                    UserName = "lsamorim.it@gmail.com"
                };

                var result = await _userManager.CreateAsync(user, "###Pass0rd###");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
            }

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
                order.User = user;

                _ctx.Orders.Add(order);

                _ctx.SaveChanges();
            }
        }
    }
}
