using KhdoumMobile.ViewModels.NotificationsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KhdoumMobile.Views.NotifactionsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationsPage : ContentPage
    {
        NotificationsViewModel ViewModel = new NotificationsViewModel();
        public NotificationsPage()
        {
            InitializeComponent();
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                ViewModel.OnAppearing();
            }
            catch(Exception ex)
            {

            }
            
        }
    }
}