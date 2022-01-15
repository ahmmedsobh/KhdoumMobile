using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.Models.ViewModels;
using KhdoumMobile.Views.CartViews;
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
        public IStateService StateService => DependencyService.Get<IStateService>();
        public ICityService CityService => DependencyService.Get<ICityService>();
        public ISettingsService SettingsService => DependencyService.Get<ISettingsService>();

        public AddOrderViewModel()
        {

            //DeliveryDates = new List<PickerViewModel<int>>();
            Cities = new List<PickerViewModel<int>>();
            States = new List<PickerViewModel<int>>();
            
            FillCities();
            FillStates();
            Order = new Order();
            CalcTotal();
            FillFormWithPersonalData();
            FillDates();


        }

        List<PickerViewModel<int>> deliveryDates;
        public List<PickerViewModel<int>> DeliveryDates 
        {
            get => deliveryDates;
            set
            {
                SetProperty(ref deliveryDates, value);
            }
        }

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
                CalcTotalAmount(value);
            }
        }

        bool isOrderBtnEnabled;
        public bool IsOrderBtnEnabled
        {
            get => isOrderBtnEnabled;
            set
            {
                SetProperty(ref isOrderBtnEnabled, value);
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

                    if(IsOrderBtnEnabled == false)
                    {
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
                        await Shell.Current.GoToAsync($"//{nameof(CartPage)}");
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

        

        async void FillDates()
        {
            try
            {
                 await SettingsService.ShowDeliveryDatesState().ContinueWith(async(s) => {

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
                        var to = i + 1;

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
            catch(Exception ex)
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
            IsBusy = true;
            //IsOrderBtnEnabled = false;
            if(State != null)
            {
                var Items = await this.Items();
                var Groups = Items.GroupBy(i => new {i.MarketId,i.StateId});
                var DeliveryService = 0M;
                var Counter = 0;
                var StatesId = new List<int>();
                foreach(var g in Groups)
                {
                    var delivery2 = await OrderServcie.GeneralDelivery(g.Key.StateId, g.Key.StateId);

                    if (g.Key.StateId == State.Value)
                    {
                        if(Counter == 0)
                        {
                            var delivery = await OrderServcie.GeneralDelivery(State.Value, g.Key.StateId);
                            DeliveryService = delivery.DeliveryService + DeliveryService;
                            Counter = Counter + 1;
                        }
                        else
                        {
                            DeliveryService = 3 + DeliveryService;
                        }
                    }
                    else
                    {
                        var delivery = await OrderServcie.GeneralDelivery(State.Value, g.Key.StateId);
                        if (delivery != null)
                        {
                            int StateId = StatesId.FirstOrDefault(s => s == g.Key.StateId);
                            if (StateId == 0)
                            {
                                StatesId.Add(g.Key.StateId);
                                DeliveryService = delivery.DeliveryService + DeliveryService;
                            }
                            else
                            {
                                DeliveryService = 3 + DeliveryService;
                            }
                        }
                    }

                    
                    
                }

                

                var TotalAmount = Order.Total + DeliveryService;



                Order.TotalAmount = TotalAmount;
                Order.DeliveryService = DeliveryService;
            }

            IsBusy = false;
            
            IsOrderBtnEnabled = true;
            
        }

        void FillFormWithPersonalData()
        {
            Order.CustomerName = Settings.Name;
            Order.Phone = string.IsNullOrEmpty(Settings.Phone) ? Settings.Username : Settings.Phone;
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
