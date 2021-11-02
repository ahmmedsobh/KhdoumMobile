using KhdoumMobile.ViewModels;
using KhdoumMobile.Views;
using KhdoumMobile.Views.CartViews;
using KhdoumMobile.Views.ProductsViews;
using KhdoumMobile.Views.SupCategoryViews;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KhdoumMobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(SupCategoryPage), typeof(SupCategoryPage));
            Routing.RegisterRoute(nameof(ProductsPage), typeof(ProductsPage));
            //Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
