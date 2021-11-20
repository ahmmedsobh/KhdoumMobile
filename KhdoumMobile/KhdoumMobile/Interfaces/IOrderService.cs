using KhdoumMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Interfaces
{
    interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<OrderViewModel> GetOrderAsync(long Id);
        Task<bool> AddOrderAsync(OrderViewModel Order);
    }
}
