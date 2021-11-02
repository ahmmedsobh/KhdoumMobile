using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.Views.MainViews;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels.OrderViewModels
{
    class AddOrderViewModel:BaseViewModel
    {
        public IOrderService OrderServcie => DependencyService.Get<IOrderService>();


        public Order Order { get; set; }

        string message;
        public string Message
        {
            get => message;
            set
            {
                SetProperty(ref message, value);
            }
        }

        string messageColor;
        public string MessageColor
        {
            get => messageColor;
            set
            {
                SetProperty(ref messageColor, value);
            }
        }

       

        public ICommand AddOrderCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (Order.CustomerName == "" || Order.CustomerName == null)
                    {
                        Message = "الاسم مطلوب";
                        MessageColor = "Red";
                        return;
                    }

                    if (Order.Phone == "" || Order.Phone == null)
                    {
                        Message = "رقم الهاتف مطلوب";
                        MessageColor = "Red";
                        return;
                    }

                    if (Order.Address == "" || Order.Address == null)
                    {
                        Message = "العنوان مطلوب";
                        MessageColor = "Red";
                        return;
                    }

                    Order.Date = DateTime.Now;
                    Order.Status = 1;
                    var model = new OrderViewModel();
                    model.Order = Order;

                    var r = await OrderServcie.AddOrderAsync(model);
                    if(r)
                    {
                        Message = "تم ارسال الطلب";
                        MessageColor = "Green";
                        return;
                    }
                    else
                    {
                        Message = "حدث خطأ، يرجى المحاولة";
                        MessageColor = "Red";
                        return;
                    }


                    
                });
            }
        }

        public ICommand GoToMainCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                });
            }
        }
    }
}
