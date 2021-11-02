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
    class RegisterViewModel:BaseViewModel
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

        string confirmPassword;
        public string ConfirmPassword 
        {
            get => confirmPassword;
            set
            {
                SetProperty(ref confirmPassword, value);
            }
        }

        string message;
        public string Message 
        {
            get =>  message;
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

        public ICommand RegisterCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if(Username == "" || Username == null)
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

                    if (ConfirmPassword == "" || ConfirmPassword == null)
                    {
                        Message = "تأكيد كلمة المرور مطلوب";
                        MessageColor = "Red";
                        return;
                    }

                    if (Password != ConfirmPassword)
                    {
                        Message = "كلمات المررو غير متساويه";
                        MessageColor = "Red";
                        return;
                    }

                    var isRegistered = await Users.RegisterAsync
                        (Username, Password, ConfirmPassword);

                    Settings.Username = Username;
                    Settings.Password = Password;

                    if (isRegistered)
                    {
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
