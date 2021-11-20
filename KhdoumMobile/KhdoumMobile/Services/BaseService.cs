using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.Views.UsersViews;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace KhdoumMobile.Services
{
    class BaseService
    {
        public IUserService<User> Users => DependencyService.Get<IUserService<User>>();

        public BaseService()
        {
            //IsLoggedIn();
        }
                
        protected async void IsLoggedIn()
        {
            var IsLoggedIn = await Users.IsLoggedIn();
            if (IsLoggedIn == false)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}
