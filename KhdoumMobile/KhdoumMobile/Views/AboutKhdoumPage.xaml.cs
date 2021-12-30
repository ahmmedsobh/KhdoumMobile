using KhdoumMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KhdoumMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutKhdoumPage : ContentPage
    {
        public AboutKhdoumPage()
        {
            InitializeComponent();
            BindingContext = new BaseViewModel();
        }
    }
}