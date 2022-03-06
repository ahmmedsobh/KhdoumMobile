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
    class AddDeliveryOrderViewModel:BaseViewModel
    {
        public IOrderService OrderServcie => DependencyService.Get<IOrderService>();
        public IStateService StateService => DependencyService.Get<IStateService>();
        public ICityService CityService => DependencyService.Get<ICityService>();
        public ICartService CartServcie => DependencyService.Get<ICartService>();
        public ISettingsService SettingsService => DependencyService.Get<ISettingsService>();


        public AddDeliveryOrderViewModel()
        {
            FillDates();
            FillCities();
            FillStates();
            Order = new Order();
        }

        public List<PickerViewModel<int>> DeliveryDates { get; set; }

        List<PickerViewModel<int>> cities;
        public List<PickerViewModel<int>> Cities 
        {
            get => cities;
            set
            {
                SetProperty(ref cities, value); 
            }
        }

        List<PickerViewModel<int>> states;
        public List<PickerViewModel<int>> States 
        {
            get => states;
            set
            {
                SetProperty(ref states, value);
            }
        }

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
            }
        }

        PickerViewModel<int> selectedStateTo;
        public PickerViewModel<int> SelectedStateTo
        {
            get => selectedStateTo;
            set
            {
                SetProperty(ref selectedStateTo, value);
                CalcTotalAmount(SelectedState,value);
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

                    if (SelectedDate == null)
                    {
                        Message = "موعد التسليم مطلوب";
                        MessageColor = "Red";
                        return;
                    }

                    //if (SelectedCity == null)
                    //{
                    //    Message = "المدينة مطلوبه";
                    //    MessageColor = "Red";
                    //    return;
                    //}

                    if (SelectedState == null)
                    {
                        Message = "من منطقة مطلوبة";
                        MessageColor = "Red";
                        return;
                    }

                    if (SelectedStateTo == null)
                    {
                        Message = "الى منطقة مطلوبة";
                        MessageColor = "Red";
                        return;
                    }

                    if(Order.Total == 0)
                    {
                        Message = "الاجمالى مطلوب";
                        MessageColor = "Red";
                        return;
                    }

                    Order.Date = DateTime.Now;
                    Order.Status = 1;
                    //Order.CityId = SelectedCity.Value;
                    Order.StateId = SelectedState.Value;
                    Order.DeliveryData = SelectedDate.Value;
                    

                    CalcTotalAmount(SelectedState,SelectedStateTo);



                    var model = new OrderViewModel();
                    model.Order = Order;

                    var r = await OrderServcie.AddDeliveryOrderAsync(model);
                    if (r)
                    {
                        await Shell.Current.GoToAsync($"//{nameof(OrdersPage)}");
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


        async void FillDates()
        {
            try
            {
                await SettingsService.ShowDeliveryDatesState().ContinueWith(async (s) => {

                    var State = await s;

                    if (!State)
                        return;

                    DeliveryDates = new List<PickerViewModel<int>>();
                    var hour = DateTime.Now.Hour;
                    var minute = DateTime.Now.Minute;

                    if (minute > 45)
                    {
                        hour = hour + 1;
                    }

                    if (hour < 10 || hour > 21)
                    {
                        hour = 10;
                    }

                    for (int i = hour; i < 22; i++)
                    {
                        var AmOrPm = "";
                        var from = i;
                        var to = (i + 1) > 12 ? 1 : (i + 1);

                        if (i > 11)
                        {
                            AmOrPm = "مساءا";
                        }
                        else
                        {
                            AmOrPm = "صباحا";
                        }

                        if (i > 12)
                        {
                            from = i - 12;
                            to = from + 1;
                        }

                        var date = new PickerViewModel<int>()
                        {
                            Value = i,
                            Name = $"من {from} الى {to} {AmOrPm}"
                        };

                        DeliveryDates.Add(date);
                    }

                });





            }
            catch (Exception ex)
            {

            }








        }

        async void FillCities()
        {
            var cities = await CityService.GetCities();
            Cities = new List<PickerViewModel<int>>();
            Cities = cities.Select(c => new PickerViewModel<int> { Name = c.Name, Value = c.ID, Value2 = c.DeliveryService }).ToList();
        }

        async void FillStates()
        {
            var states = await StateService.GetStates();
            States = new List<PickerViewModel<int>>();
            States = states.Select(s => new PickerViewModel<int> { Name = s.Name, Value = s.ID, Value2 = s.DeliveryService }).ToList();
        }

        async Task<IEnumerable<CartItem>> Items()
        {
            return await CartServcie.GetCartItems();
        }

        async void CalcTotalAmount(PickerViewModel<int> State, PickerViewModel<int> StateTo)
        {

            if (State != null && StateTo != null)
            {
                var delivery = await OrderServcie.GeneralDelivery(State.Value, StateTo.Value);
                if(delivery != null)
                {
                    Order.DeliveryService = delivery.DeliveryService;
                    Order.TotalAmount = Order.Total + delivery.DeliveryService;
                }
            }
        }
    }
}
