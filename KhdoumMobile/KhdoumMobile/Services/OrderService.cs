using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KhdoumMobile.Services
{
    class OrderService : IOrderService
    {
        public ICartService CartService => DependencyService.Get<ICartService>();

        public async Task<bool> AddOrderAsync(OrderViewModel Order)
        {
            var Client = new HttpClient();

            Order = await FillOrderDetails(Order);

            if(Order.OrderDetails.Count() == 0)
            {
                return await Task.FromResult(false);
            }

            var json = JsonConvert.SerializeObject(Order);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await Client.PostAsync(Constants.BaseApiAddress + "api/Orders", content);
            if (response.IsSuccessStatusCode)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(string UserId)
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/Orders/GetOrdersWithoutDetailsForUser/{UserId}");

            IEnumerable<Order> orders = new List<Order>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<IEnumerable<Order>>(json);
            }

            return await Task.FromResult(orders);
        }

        async Task<OrderViewModel> FillOrderDetails(OrderViewModel Order)
        {
            var OrderDetails = new List<OrderDetails>();
            var CartItems = await CartService.GetCartItems();
            decimal TotalPrice = 0;

            if(CartItems != null)
            {
                if(CartItems.Count() > 0)
                {
                    OrderDetails = (from i in CartItems
                                   select new OrderDetails()
                                   {
                                        Quantity = i.CounterValue,
                                        Price = i.Price,
                                        Value = i.TotalPrice,
                                        ProductId = i.ProductId
                                   }).ToList();

                    TotalPrice = CartItems.Select(i => i.TotalPrice).Sum();

                    Order.Order.Total = TotalPrice;
                    Order.OrderDetails = OrderDetails;
                }
            }

            return await Task.FromResult(Order);


        }
    }
}
