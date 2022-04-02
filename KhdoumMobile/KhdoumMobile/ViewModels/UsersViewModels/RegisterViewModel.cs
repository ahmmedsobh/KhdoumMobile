using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.Services;
using KhdoumMobile.Views.MainViews;
using KhdoumMobile.Views.UsersViews;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels.UsersViewModels
{
    class RegisterViewModel:BaseViewModel
    {
        public IUserService<User> Users => DependencyService.Get<IUserService<User>>();

        public RegisterViewModel()
        {
            var pass = PasswordFactory.GenerateRandomPassword();
            Password = pass;
            confirmPassword = pass;
        }

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

        string name;
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
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
                    

                    if (Name == "" || Name == null)
                    {
                        Message = "الاسم مطلوب";
                        MessageColor = "Red";
                        return;
                    }

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

                    //var passwordRegex = @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W]).{8,64})";
                    //var IsValid = Regex.IsMatch(Password, passwordRegex);

                    //if(!IsValid)
                    //{
                    //    Message = "";
                    //    MessageColor = "Red";
                    //    return;
                    //}
                    IsBusy = true;


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
                        (Username, Password, ConfirmPassword,Name);

                    Settings.Username = Username;
                    Settings.Password = Password;
                    Settings.Name = Name;

                    if (isRegistered.Status == "Success")
                    {
                        //Message = "تم التسجيل";
                        //MessageColor = "Green";
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    }
                    else
                    {
                        IsBusy = false;
                        Message = isRegistered.Message;
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

        public ICommand GoToLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                });
            }
        }
    }
}
