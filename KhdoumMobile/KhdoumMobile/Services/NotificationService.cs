using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Services
{
    class NotificationService : INotificationService
    {
        public async Task<IEnumerable<Notification>> GetNotificationsForUser()
        {
            var client = new HttpClient();

            var accessToken = Settings.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/Notifications/GetNotificationsForUser");

            IEnumerable<Notification> notifications = new List<Notification>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                notifications = JsonConvert.DeserializeObject<IEnumerable<Notification>>(json);
            }

            return await Task.FromResult(notifications);
        }

        public async Task<bool> SaveFirebaseAppToken(string Token)
        {
            var client = new HttpClient();

            var accessToken = Settings.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/Notifications/SaveFirebaseAppToken/{Token}");

            return await Task.FromResult(response.IsSuccessStatusCode);
        }
    }
}
