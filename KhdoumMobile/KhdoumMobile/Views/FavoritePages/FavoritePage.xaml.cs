using KhdoumMobile.ViewModels.FavoriteViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KhdoumMobile.Views.FavoritePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritePage : ContentPage
    {
        FavoriteViewModel ViewModel = new FavoriteViewModel();
        public FavoritePage()
        {
            InitializeComponent();
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}