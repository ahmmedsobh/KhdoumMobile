using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels.AppViewModels
{
    public class AppViewModel:BaseViewModel
    {
        public IOrderService OrderService => DependencyService.Get<IOrderService>();

        public ObservableCollection<Order> Orders { get; }

        public AppViewModel()
        {
            ShowNotifcations();
            Value = null;
            Orders = new ObservableCollection<Order>();
            FillOrdersList();
        }


        int? value;
        public int? Value
        {
            get => value;
            set
            {
                SetProperty(ref this.value, value);
            }
        }

        int? cartValue;
        public int? CartValue
        {
            get => cartValue;
            set
            {
                SetProperty(ref this.cartValue, value);
            }
        }


        void ShowNotifcations()
        {
            Device.StartTimer(TimeSpan.FromMinutes(1),() =>
            {
                FillOrdersList();
                return true; // True = Repeat again, False = Stop the timer
            });
        }

        async void FillOrdersList()
        {
            var orders = await OrderService.GetOrdersAsync(0);
            if(Orders.Count > 0)
            {
                foreach (var item in orders)
                {
                    var order = Orders.FirstOrDefault(o => o.ID == item.ID);
                    if (order != null)
                    {
                        if (order.Status != item.Status)
                        {
                            order.Status = item.Status;

                            if(Value == null)
                            {
                                Value = 0;
                            }

                            if (Value < 10)
                            {
                                SoundPlayer.Play("notification1");
                                Value = Value + 1;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var item in orders)
                {
                    Orders.Add(item);
                }
            }
            
        }
             
        public static void ChangeValue()
        {
            
        }
    }
}
