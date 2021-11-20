using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.Views.UsersViews;
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
    class OrderService : BaseService ,IOrderService
    {
        public ICartService CartService => DependencyService.Get<ICartService>();
        public async Task<bool> AddOrderAsync(OrderViewModel Order)
        {
            var Client = new HttpClient();

            var accessToken = Settings.AccessToken;

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

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
        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
           

            var client = new HttpClient();

            var accessToken = Settings.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/Orders/GetOrdersWithoutDetailsForUser");

            IEnumerable<Order> orders = new List<Order>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<IEnumerable<Order>>(json);
            }

            orders = from o in orders
                     select new Order
                     {
                         ID = o.ID,
                         CustomerName = o.CustomerName,
                         Total = o.Total,
                         Date = o.Date,
                         Address = o.Address,
                         Notes = o.Notes,
                         Phone = o.Phone,
                         DeliveryService = o.DeliveryService,
                         IsActive = o.IsActive,
                         DeliveryData = o.DeliveryData,
                         Status = o.Status,
                         StateId = o.StateId,
                         CityId = o.CityId,
                         UserId = o.UserId,
                         StatusTitle = GetStatusTitle(o.Status),
                         StatusIcon = GetStatusIcon(o.Status),
                         StatusColor = GetStatusColor(o.Status),
                     };

            return await Task.FromResult(orders);
        }
        public async Task<OrderViewModel> GetOrderAsync(long Id)
        {
            var client = new HttpClient();

            var accessToken = Settings.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/Orders/{Id}");

            OrderViewModel order = new OrderViewModel();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                order = JsonConvert.DeserializeObject<OrderViewModel>(json);
            }
           
            order.Order.StatusTitle = GetStatusTitle(order.Order.Status);
            order.Order.StatusIcon = GetStatusIcon(order.Order.Status);
            order.Order.StatusColor = GetStatusColor(order.Order.Status);
            

            return await Task.FromResult(order);
        }
        async Task<OrderViewModel> FillOrderDetails(OrderViewModel Order)
        {
            var OrderDetails = new List<OrderDetails>();
            var CartItems = await CartService.GetCartItems();
            //decimal TotalPrice = 0;

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

                    //TotalPrice = CartItems.Select(i => i.TotalPrice).Sum();

                    //Order.Order.Total = TotalPrice;
                    Order.OrderDetails = OrderDetails;
                }
            }

            return await Task.FromResult(Order);


        }
        string GetStatusTitle(int Status)
        {
            string StatusTitle = "";

            switch(Status)
            {
                case 1:
                    StatusTitle = "انتظار";
                    break;
                case 2:
                    StatusTitle = "ينفذ";
                    break;
                case 3:
                    StatusTitle = "مكتمل";
                    break;
                case 4:
                    StatusTitle = "ملغى";
                    break;
            }

            return StatusTitle;
        }
        string GetStatusIcon(int Status)
        {
            string StatusIcon = "";

            switch (Status)
            {
                case 1:
                    StatusIcon = "\uf110";
                    break;
                case 2:
                    StatusIcon = "\uf085";
                    break;
                case 3:
                    StatusIcon = "\uf058";
                    break;
                case 4:
                    StatusIcon = "\uf057";
                    break;
            }

            return StatusIcon;
        }
        string GetStatusColor(int Status)
        {
            string StatusColor = "";

            switch (Status)
            {
                case 1:
                    StatusColor = "#ffc000";
                    break;
                case 2:
                    StatusColor = "#1000dd";
                    break;
                case 3:
                    StatusColor = "#0ec100";
                    break;
                case 4:
                    StatusColor = "#e70000";
                    break;
            }

            return StatusColor;
        }
    }
}
