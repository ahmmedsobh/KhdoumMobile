using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.Models.ViewModels;
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
        public List<PickerViewModel<int>> Status { get; }
        public Command LoadOrdersCommand { get; }
        public Command<Order> OrderTapped { get; }

        public OrdersViewModel()
        {
            Orders = new ObservableCollection<Order>();
            Status = StatusList;

           

            OrderTapped = new Command<Order>(OnOrderSelected);
            LoadOrdersCommand = new Command(() => ExecuteLoadOrdersCommand());

            AppShell.StaticAppViewModel.Value = null;
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
            AppShell.StaticAppViewModel.Value = null;
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

        PickerViewModel<int> selectedStatus;
        public PickerViewModel<int> SelectedStatus
        {
            get => selectedStatus;
            set
            {
                SetProperty(ref selectedStatus, value);
                FillOrders(value.Value);
            }
        }

        async void OnOrderSelected(Order Order)
        {
            if (Order == null)
                return;

            SelectedOrder = null;
            await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}?{nameof(OrderDetailsViewModel.OrderId)}={Order.ID}");
        }

        async void FillOrders(int Status = 1)
        {
            Orders.Clear();
            var orders = await OrderService.GetOrdersAsync(Status);
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
