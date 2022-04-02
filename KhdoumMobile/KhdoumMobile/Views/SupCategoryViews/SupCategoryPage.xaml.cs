using KhdoumMobile.ViewModels.SupCategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KhdoumMobile.Views.SupCategoryViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupCategoryPage : ContentPage
    {
        SupCategoryViewModel ViewModel;
        public SupCategoryPage()
        {
            InitializeComponent();
            ViewModel = new SupCategoryViewModel();
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}