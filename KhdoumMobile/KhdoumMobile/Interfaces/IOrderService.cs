using KhdoumMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Interfaces
{
    interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync(string UserId);
        Task<bool> AddOrderAsync(OrderViewModel Order);
    }
}
