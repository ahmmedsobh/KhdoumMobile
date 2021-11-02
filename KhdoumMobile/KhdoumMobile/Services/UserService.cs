using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Services
{
    class UserService : IUserService<User>
    {
        public async Task<string> LoginAsync(string Phone, string Password)
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(new { UserName = Phone, Password = Password });

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(
                Constants.BaseApiAddress + "api/Authenticate/login", httpContent);

            var content = await response.Content.ReadAsStringAsync();

            JObject jwtDynamic = JObject.Parse(content);

            var accessTokenExpiration = jwtDynamic.Value<DateTime>("expiration");
            var accessToken = jwtDynamic.Value<string>("token");

            Settings.AccessTokenExpirationDate = accessTokenExpiration;


            return accessToken;
        }

        public async Task<bool> RegisterAsync(string Phone, string Password, string ConfirmPassword)
        {
            var client = new HttpClient();

             //Phone = "01097613604";
             //Password = "AAaa123456789##";

            var json = JsonConvert.SerializeObject(new {UserName = Phone,Password=Password });

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(
                Constants.BaseApiAddress + "api/Authenticate/register", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsLoggedIn()
        {
            var client = new HttpClient();


            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/categories/GetFrom1To2LevelCategories");

            IEnumerable<Item> items = new List<Item>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false); 
        }
    }
}
