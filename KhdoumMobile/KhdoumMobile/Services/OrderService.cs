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

        public async Task<IEnumerable<Order>> GetOrdersAsync(int Status)
        {
           

            var client = new HttpClient();

            var accessToken = Settings.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/Orders/GetOrdersByStatusWithoutDetailsForUser/{Status}");

            IEnumerable<Order> orders = new List<Order>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<IEnumerable<Order>>(json);
            }

            orders = from o in orders
                     orderby o.Status
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
                         StatusIconTitle = GetStatusTitleForIcon(o.Status),
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
            order.Order.StatusIconTitle = GetStatusTitleForIcon(order.Order.Status);
            order.Order.ChangeStatusBtnVisible = order.Order.Status == 1 ? true:false;



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
                                        ProductId = i.ProductId,
                                        MarketName = i.MarketName,
                                        MarketId = i.MarketId,
                                   }).ToList();

                    //TotalPrice = CartItems.Select(i => i.TotalPrice).Sum();

                    //Order.Order.Total = TotalPrice;
                    Order.OrderDetails = OrderDetails;
                }
            }

            return await Task.FromResult(Order);


        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            var Client = new HttpClient();

            var accessToken = Settings.AccessToken;

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            var json = JsonConvert.SerializeObject(order);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await Client.PutAsync(Constants.BaseApiAddress + "api/Orders", content);
            if (response.IsSuccessStatusCode)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
        string GetStatusTitle(int Status)
        {
            string StatusTitle = "";

            switch(Status)
            {
                case 1:
                    StatusTitle = "تم استلام الطلب";
                    break;
                case 2:
                    StatusTitle = "جاري تجهيز الطلب";
                    break;
                case 3:
                    StatusTitle = "تم تجهيز الطلب";
                    break;
                case 4:
                    StatusTitle = "جاري تسليم الطلب";
                    break;
                case 5:
                    StatusTitle = "تم تسليم الطلب";
                    break;
                case 6:
                    StatusTitle = "تم الغاء الطلب";
                    break;
            }

            return StatusTitle;
        }

        string GetStatusTitleForIcon(int Status)
        {
            string StatusTitle = "";

            switch (Status)
            {
                case 1:
                    StatusTitle = "انتظار";
                    break;
                case 2:
                    StatusTitle = "يجهز";
                    break;
                case 3:
                    StatusTitle = "مجهز";
                    break;
                case 4:
                    StatusTitle = "يسلم";
                    break;
                case 5:
                    StatusTitle = "مكتمل";
                    break;
                case 6:
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
                    StatusIcon = "\uf46d";
                    break;
                case 4:
                    StatusIcon = "\uf4cf";
                    break;
                case 5:
                    StatusIcon = "\uf058";
                    break;
                case 6:
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
                    StatusColor = "#c15a00";
                    break;
                case 4:
                    StatusColor = "#00c17a";
                    break;
                case 5:
                    StatusColor = "#0ec100";
                    break;
                case 6:
                    StatusColor = "#e70000";
                    break;
            }

            return StatusColor;
        }

        
        public async Task<GeneralDelivery> GeneralDelivery(int State1, int State2)
        {
            var client = new HttpClient();

            var accessToken = Settings.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/GeneralDelivery/{State1}/{State2}");

            GeneralDelivery delivery = new GeneralDelivery();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                delivery = JsonConvert.DeserializeObject<GeneralDelivery>(json);
            }

            return await Task.FromResult(delivery);
        }
        public async Task<bool> AddDeliveryOrderAsync(OrderViewModel Order)
        {
            var Client = new HttpClient();

            var accessToken = Settings.AccessToken;

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            Order.OrderDetails = new List<OrderDetails>();

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
        public async Task<IEnumerable<State>> States()
        {
            var client = new HttpClient();

            var accessToken = Settings.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/States");

            IEnumerable<State> states = new List<State>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                states = JsonConvert.DeserializeObject<IEnumerable<State>>(json);
            }

            return await Task.FromResult(states);
        }
    }
}
