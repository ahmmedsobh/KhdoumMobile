using KhdoumMobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace KhdoumMobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}