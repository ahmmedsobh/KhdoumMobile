﻿using KhdoumMobile.ViewModels.OrderViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KhdoumMobile.Views.OrdersViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailsPage : ContentPage
    {
        public OrderDetailsPage()
        {
            InitializeComponent();
            BindingContext = new OrderDetailsViewModel();
        }
    }
}