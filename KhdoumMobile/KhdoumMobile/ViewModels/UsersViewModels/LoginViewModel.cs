using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.Services;
using KhdoumMobile.Views.MainViews;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels.UsersViewModels
{
    class LoginViewModel:BaseViewModel
    {
        public IUserService<User> Users => DependencyService.Get<IUserService<User>>();

        string username;
        public string Username
        {
            get => username;
            set
            {
                SetProperty(ref username, value);
            }
        }

        string password;
        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
            }
        }

        

        string message;
        public string Message
        {
            get => message;
            set
            {
                SetProperty(ref message, value);
            }
        }

        string messageColor;
        public string MessageColor
        {
            get => messageColor;
            set
            {
                SetProperty(ref messageColor, value);
            }
        }
        public string Logo
        {
            get => "KhdoumMobile.Resources.Images.KhdoumLogo.png";
        }

        public ICommand LoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (Username == "" || Username == null)
                    {
                        Message = "رقم الهاتف مطلوب";
                        MessageColor = "Red";
                        return;
                    }

                    if (Password == "" || Password == null)
                    {
                        Message = "كلمة المرور مطلوبة";
                        MessageColor = "Red";
                        return;
                    }

                    var isLogined = await Users.LoginAsync
                        (Username, Password);

                   

                    if (isLogined != "")
                    {
                        Settings.Username = Username;
                        Settings.Password = Password;
                        Settings.AccessToken = isLogined;

                        Message = "تم التسجيل";
                        MessageColor = "Green";
                    }
                    else
                    {
                        Message = "حدث خطأ ، حاول مجددا";
                        MessageColor = "Red";
                        return;
                    }
                });
            }
        }

        public ICommand GoToMainCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                });
            }
        }
    }
}
