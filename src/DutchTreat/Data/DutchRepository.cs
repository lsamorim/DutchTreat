using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _ctx;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }
        
        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                return from p in _ctx.Products
                       orderby p.Title
                       select p;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to get all products: {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            try
            {
                return from p in _ctx.Products
                       where p.Category == category
                       select p;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to get products by category: {ex}");
                return null;
            }
        }

        public IEnumerable<Order> GetAllOrders()
        {
            try
            {
                return _ctx.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to get all orders: {ex}");
                return null;
            }
        }
        public IEnumerable<Order> GetAllOrdersByUser(string username)
        {
            try
            {
                return _ctx.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .Where(o => o.User.UserName == username);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to get all orders: {ex}");
                return null;
            }
        }

        public Order GetOrderById(string username, int id)
        {
            return _ctx.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .Where(o => o.Id == id && o.User.UserName == username).FirstOrDefault();
        }

        public void AddEntity(object entity)
        {
            _ctx.Add(entity);
        }

        public bool SaveChanges()
        {
            return _ctx.SaveChanges() > 0;
        }
    }
}
