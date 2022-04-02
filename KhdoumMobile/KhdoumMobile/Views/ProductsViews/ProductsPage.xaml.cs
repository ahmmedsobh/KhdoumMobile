using KhdoumMobile.ViewModels.ProductsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KhdoumMobile.Views.ProductsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {
        ProductsViewModel ViewModel;

        public ProductsPage()
        {
            InitializeComponent();
            ViewModel = new ProductsViewModel();
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}