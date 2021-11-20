using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels.OrderViewModels
{
    [QueryProperty(nameof(OrderId), nameof(OrderId))]

    class OrderDetailsViewModel :BaseViewModel
    {
        public IOrderService OrderService => DependencyService.Get<IOrderService>();

        Order order;
        public Order Order 
        {
            get => order;
            set
            {
                SetProperty(ref order, value);
            }
        }
        public ObservableCollection<OrderDetails> OrderDetails { get; }

        public OrderDetailsViewModel()
        {
            Order = new Order();
            OrderDetails = new ObservableCollection<OrderDetails>();
        }

        async void ExecuteLoadOrderCommand(long OrderId)
        {

            //var IsConnected = await connectionService.IsConnected();

            //if (!IsConnected)
            //    return;

            IsBusy = true;

            try
            {
                OrderDetails.Clear();
                var order = await OrderService.GetOrderAsync(OrderId);

                Order = order.Order;
                Order.TotalAmount = Order.Total + Order.DeliveryService;

                foreach (var item in order.OrderDetails)
                {
                    OrderDetails.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                //IsBusy = false;
            }
        }

        //public void OnAppearing()
        //{
        //    IsBusy = true;
        //}

        long orderId;
        public long OrderId
        {
            get => orderId;
            set
            {
                SetProperty(ref orderId, value);
                ExecuteLoadOrderCommand(value);
            }
        }

       

        





        

        

       

        

        
    }
}
