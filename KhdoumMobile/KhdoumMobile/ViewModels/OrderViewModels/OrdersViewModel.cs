using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.Views.OrdersViews;
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
    class OrdersViewModel:BaseViewModel
    {
        public IOrderService OrderService => DependencyService.Get<IOrderService>();



        private Order _selectedOrder;

        public ObservableCollection<Order> Orders { get; }
        public Command LoadOrdersCommand { get; }
        public Command<Order> OrderTapped { get; }

        public OrdersViewModel()
        {
            Orders = new ObservableCollection<Order>();

            OrderTapped = new Command<Order>(OnOrderSelected);
            LoadOrdersCommand = new Command(() => ExecuteLoadOrdersCommand());
        }

         void ExecuteLoadOrdersCommand()
        {

            //var IsConnected = await connectionService.IsConnected();

            //if (!IsConnected)
            //    return;

            IsBusy = true;

            try
            {
                FillOrders();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedOrder = null;
        }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                SetProperty(ref _selectedOrder, value);
                OnOrderSelected(value);
            }
        }

       
        async void OnOrderSelected(Order Order)
        {
            if (Order == null)
                return;

            SelectedOrder = null;
            await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}?{nameof(OrderDetailsViewModel.OrderId)}={Order.ID}");
        }

        async void FillOrders()
        {
            Orders.Clear();
            var orders = await OrderService.GetOrdersAsync();
            var index = 0;
            foreach (var item in orders)
            {
                ++index;
                item.Index = index;
                item.TotalAmount = item.Total + item.DeliveryService;
                Orders.Add(item);
            }
        }
    }
}
