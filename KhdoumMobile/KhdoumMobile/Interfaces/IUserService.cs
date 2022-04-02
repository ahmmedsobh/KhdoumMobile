using KhdoumMobile.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KhdoumMobile.Interfaces
{
    public interface IUserService<T>
    {
        Task<string> LoginAsync(string Phone, string Password);
        Task<Response> RegisterAsync(string Phone, string Password,string ConfirmPassword,string Name);
        Task<bool> IsLoggedIn();
        Task<string> ChangeUserImage(FileResult ImgFile);
    }
}
