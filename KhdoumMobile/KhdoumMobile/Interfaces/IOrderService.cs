using KhdoumMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync(int Status);
        Task<OrderViewModel> GetOrderAsync(long Id);
        Task<bool> AddOrderAsync(OrderViewModel Order);
        Task<bool> AddDeliveryOrderAsync(OrderViewModel Order);
        Task<GeneralDelivery> GeneralDelivery(int State1 , int State2);
        Task<bool> UpdateOrderAsync(Order order);
        //Task<IEnumerable<State>> States();
    }
}
