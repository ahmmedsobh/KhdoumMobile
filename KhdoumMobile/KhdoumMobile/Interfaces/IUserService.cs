using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Interfaces
{
    interface IUserService<T>
    {
        Task<string> LoginAsync(string Phone, string Password);
        Task<bool> RegisterAsync(string Phone, string Password,string ConfirmPassword);
        Task<bool> IsLoggedIn();
    }
}
