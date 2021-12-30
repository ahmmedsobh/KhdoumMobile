using KhdoumMobile.ViewModels;
using KhdoumMobile.ViewModels.AppViewModels;
using KhdoumMobile.Views;
using KhdoumMobile.Views.CartViews;
using KhdoumMobile.Views.MenuViews;
using KhdoumMobile.Views.NotifactionsPages;
using KhdoumMobile.Views.OrdersViews;
using KhdoumMobile.Views.ProductsViews;
using KhdoumMobile.Views.SupCategoryViews;
using KhdoumMobile.Views.UsersViews;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KhdoumMobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public static AppViewModel StaticAppViewModel = new AppViewModel();
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(SupCategoryPage), typeof(SupCategoryPage));
            Routing.RegisterRoute(nameof(ProductsPage), typeof(ProductsPage));
            Routing.RegisterRoute(nameof(AddOrderPage), typeof(AddOrderPage));
            Routing.RegisterRoute(nameof(OrderDetailsPage), typeof(OrderDetailsPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            //Routing.RegisterRoute(nameof(OrdersPage), typeof(OrdersPage));
            Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
            Routing.RegisterRoute(nameof(AddDeliveryOrderPage), typeof(AddDeliveryOrderPage));
            Routing.RegisterRoute(nameof(MenuPage), typeof(MenuPage));
            Routing.RegisterRoute(nameof(AboutKhdoumPage), typeof(AboutKhdoumPage));
            Routing.RegisterRoute(nameof(ContactUsPage), typeof(ContactUsPage));
            Routing.RegisterRoute(nameof(NotificationsPage), typeof(NotificationsPage));
            BindingContext = StaticAppViewModel;
        }
      
        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
