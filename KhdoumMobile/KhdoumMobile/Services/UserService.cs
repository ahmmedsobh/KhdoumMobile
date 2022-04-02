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
using System.Linq;
using Xamarin.Essentials;
using System.IO;
using KhdoumMobile.Models.ViewModels;

namespace KhdoumMobile.Services
{
    class Img
    {
        public string ImgUrl { get; set; }
    }

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
            var name = jwtDynamic.Value<string>("name");

            Settings.AccessTokenExpirationDate = accessTokenExpiration;
            Settings.Name = name;


            return accessToken;
        }

        public async Task<Response> RegisterAsync(string Phone, string Password, string ConfirmPassword,string Name)
        {
            var client = new HttpClient();

             //Phone = "01097613604";
             //Password = "AAaa123456789##";

            var json = JsonConvert.SerializeObject(new {UserName = Phone,Password=Password,Name=Name });

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(
                Constants.BaseApiAddress + "api/Authenticate/register", httpContent);

            var ReturnedJson = await response.Content.ReadAsStringAsync();
            var message = JsonConvert.DeserializeObject<Response>(ReturnedJson);
            return message;



        }

        public async Task<bool> IsLoggedIn()
        {
            var client = new HttpClient();

            var accessToken = Settings.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);


            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/categories/GetFrom1To2LevelCategories");

            IEnumerable<Item> items = new List<Item>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false); 
        }

        public async Task<string> ChangeUserImage(FileResult ImgFile)
        {
            var Client = new HttpClient();

            var accessToken = Settings.AccessToken;

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            MultipartFormDataContent content = new MultipartFormDataContent();

           
            if (ImgFile != null)
            {
                var FileContent = new StreamContent(await ImgFile.OpenReadAsync());
                content.Add(FileContent, "ImgFile", ImgFile.FileName);
            }


            var response = await Client.PostAsync(Constants.BaseApiAddress + "api/Clients/ChangeClientImg", content);
            var Img = new Img();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Img = JsonConvert.DeserializeObject<Img>(json);
            }

            return Img.ImgUrl;
        }

        
    }
}
