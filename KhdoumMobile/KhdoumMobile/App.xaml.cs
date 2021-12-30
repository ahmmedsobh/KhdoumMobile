using KhdoumMobile.Helpers;
using KhdoumMobile.Services;
using KhdoumMobile.Views;
using Plugin.FirebasePushNotification;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KhdoumMobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<UserService>();
            DependencyService.Register<CategoryService>();
            DependencyService.Register<ProductsService>();
            DependencyService.Register<CartItemService>();
            DependencyService.Register<OrderService>();
            DependencyService.Register<FavoriteService>();
            DependencyService.Register<NotificationService>();
            MainPage = new AppShell();

            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;


        }


        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            var Token = e.Token;
            if (!string.IsNullOrEmpty(Token))
            {
                Settings.FirebaseAppToken = Token;
            }
        }



        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
