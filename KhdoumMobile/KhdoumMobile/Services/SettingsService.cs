using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Services
{
    class SettingsService : ISettingsService
    {
        public async Task<bool> ShowDeliveryDatesState()
        {
            var client = new HttpClient();

            var accessToken = Settings.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/Settings/ShowDeliveryDatesState");

            bool state = true;

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                state = JsonConvert.DeserializeObject<bool>(json);
            }

            return state;
        }
    }
}
