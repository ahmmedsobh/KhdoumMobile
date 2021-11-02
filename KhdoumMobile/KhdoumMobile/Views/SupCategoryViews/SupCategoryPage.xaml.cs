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
        public SupCategoryPage()
        {
            InitializeComponent();
            BindingContext = new SupCategoryViewModel();
        }
    }
}