using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.Views.MainViews;
using KhdoumMobile.Views.UsersViews;
using Plugin.LatestVersion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels.UsersViewModels
{
    class LoadingViewModel:BaseViewModel
    {
        public IUserService<User> Users => DependencyService.Get<IUserService<User>>();

        public Command LoginCommand { get; }
        public LoadingViewModel()
        {
            //reset client login data to show login page
            //Preferences.Set("Code","");
            //Preferences.Set("Password","");
            LoginCommand = new Command(IsLoggedIn);
            IsLoggedIn();
        }
        bool loginBtnVisible;
        public bool LoginBtnVisible
        {
            get => loginBtnVisible;
            set
            {
                SetProperty(ref loginBtnVisible, value);
            }
        }

        public string Logo
        {
            get => "KhdoumMobile.Resources.Images.KhdoumLogo.png";
        }

        async void IsLoggedIn()
        {
            IsBusy = true;
            LoginBtnVisible = false;
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("رسالة", "تأكد من الاتصال بالانترنت", "موافق");
                IsBusy = false;
                LoginBtnVisible = true;
                return;
            }

            //var isLatest = await CrossLatestVersion.Current.IsUsingLatestVersion();
            var isLatest = true;
            if (!isLatest)
            {
                await Shell.Current.DisplayAlert("تحديث جديد", "هناك تحديث جديد من التطبيق قم بالتحديث","موافق");
                await CrossLatestVersion.Current.OpenAppInStore();
                IsBusy = false;
                LoginBtnVisible = true;
                return;
            }

            var isLoggedIn = await Users.IsLoggedIn();
            //var isLoggedIn = true;
            //await Task.Delay(10000);

            if (isLoggedIn)
            {
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}
