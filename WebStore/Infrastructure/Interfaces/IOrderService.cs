using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Orders;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetUserOrders(string UserName);

        Task<Order> GetOrderById(int id);

        Task<Order> CreateOrder(string UserName, CartViewModel Cart, OrderViewModel OrderModel);
    }
}
