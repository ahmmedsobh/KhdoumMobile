using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.Models.ViewModels;
using KhdoumMobile.Views.MainViews;
using KhdoumMobile.Views.OrdersViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels.OrderViewModels
{
    class AddOrderViewModel:BaseViewModel
    {
        public IOrderService OrderServcie => DependencyService.Get<IOrderService>();
        public ICartService CartServcie => DependencyService.Get<ICartService>();

        public AddOrderViewModel()
        {
            FillDates();
            FillCities();
            FillStates();
            Order = new Order();
            CalcTotal();
            FillFormWithPersonalData();

        }

        public List<PickerViewModel<int>> DeliveryDates { get; set; }
        public List<PickerViewModel<int>> Cities { get; set; }
        public List<PickerViewModel<int>> States { get; set; }

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

        PickerViewModel<int> selectedDate;
        public PickerViewModel<int> SelectedDate
        {
            get => selectedDate;
            set
            {
                SetProperty(ref selectedDate, value);
            }
        }

        PickerViewModel<int> selectedCity;
        public PickerViewModel<int> SelectedCity
        {
            get => selectedCity;
            set
            {
                SetProperty(ref selectedCity, value);
            }
        }

        PickerViewModel<int> selectedState;
        public PickerViewModel<int> SelectedState
        {
            get => selectedState;
            set
            {
                SetProperty(ref selectedState, value);
                CalcTotalAmount(value);
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

                    if(SelectedDate == null)
                    {
                        Message = "موعد التسليم مطلوب";
                        MessageColor = "Red";
                        return;
                    }

                    if (SelectedCity == null)
                    {
                        Message = "المدينة مطلوبه";
                        MessageColor = "Red";
                        return;
                    }

                    if (SelectedState == null)
                    {
                        Message = "المنطقة مطلوبة";
                        MessageColor = "Red";
                        return;
                    }

                    Order.Date = DateTime.Now;
                    Order.Status = 1;
                    Order.CityId = SelectedCity.Value;
                    Order.StateId = SelectedState.Value;
                    Order.DeliveryData = SelectedDate.Value;

                    CalcTotal();
                    CalcTotalAmount(SelectedState);



                    var model = new OrderViewModel();
                    model.Order = Order;

                    var r = await OrderServcie.AddOrderAsync(model);
                    if(r)
                    {
                        FillSettingsWithFormPersonalData();
                        await CartServcie.DeleteAllCartItems();
                        await Shell.Current.GoToAsync($"//{nameof(OrdersPage)}");
                        //Message = "تم ارسال الطلب";
                        //MessageColor = "Green";
                        //return;
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


        void FillDates()
        {
            DeliveryDates = new List<PickerViewModel<int>>();
            

            var hour = DateTime.Now.Hour;
            var minute = DateTime.Now.Minute;

            if(minute > 45 )
            {
                hour = hour + 1;
            }

            if(hour < 10 || hour > 21)
            {
                hour = 10;
            }

            for (int i = hour; i < 22; i++)
            {
                    var AmOrPm = "";
                    var from = i;
                    var to = i+1;

                    if(i > 11)
                    {
                        AmOrPm = "مساءا";
                    }
                    else
                    {
                        AmOrPm = "صباحا";
                    }

                    if(i > 12)
                    {
                        from = i - 12;
                        to = (i + 1) - 12;
                    }

                    var date = new PickerViewModel<int>()
                    {
                        Value = i,
                        Name = $"من {from} الى {to} {AmOrPm}"
                    };

                    DeliveryDates.Add(date);
            }
            

            

        }

        void FillCities()
        {
            Cities = new List<PickerViewModel<int>>()
            {
                new PickerViewModel<int>{Value= 1,Name = "الجمالية",Value2= 5 }
            };
        }

        void FillStates()
        {
            States = new List<PickerViewModel<int>>()
            {
                new PickerViewModel<int>{Value= 2,Name = "الجمالية",Value2=5 },
                new PickerViewModel<int>{Value= 1,Name = "الزمالك",Value2=10 },
            };
        }

        async Task<IEnumerable<CartItem>> Items()
        {
            return await CartServcie.GetCartItems();
        }

        async void CalcTotal()
        {
            var Items = await this.Items();
            decimal Total= 0;
            if (Items != null)
            {
                if (Items.Count() > 0)
                {
                    Total = Items.Select(i => i.TotalPrice).Sum();
                }
            }

            Order.Total = Total;
        }

        async void CalcTotalAmount(PickerViewModel<int> State)
        {
            if(State != null)
            {
                var Items = await this.Items();
                var GroupsCount = Items.GroupBy(i => i.MarketId).Count();
                var DeliveryService = Convert.ToDecimal(State.Value2);

                if (GroupsCount > 1)
                {
                    DeliveryService = ((GroupsCount - 1) * 3) + DeliveryService;
                }

                var TotalAmount = Order.Total + DeliveryService;



                Order.TotalAmount = TotalAmount;
                Order.DeliveryService = DeliveryService;
            }
            
        }

        void FillFormWithPersonalData()
        {
            Order.CustomerName = Settings.Name;
            Order.Phone = Settings.Phone;
            Order.Address = Settings.Address;

            SelectedCity = Cities.FirstOrDefault(c => c.Value == Settings.City);
            SelectedState = States.FirstOrDefault(s => s.Value == Settings.State);
        }

        void FillSettingsWithFormPersonalData()
        {
            Settings.Name = Order.CustomerName;
            Settings.Phone = Order.Phone;
            Settings.Address = Order.Address;

            Settings.City = SelectedCity.Value;
            Settings.State = SelectedState.Value;
        }


    }
}
