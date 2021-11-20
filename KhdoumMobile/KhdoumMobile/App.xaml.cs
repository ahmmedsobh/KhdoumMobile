using KhdoumMobile.Services;
using KhdoumMobile.Views;
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
            MainPage = new AppShell();
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
