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
    class CityService : ICityService
    {
        public async Task<IEnumerable<City>> GetCities()
        {
            var client = new HttpClient();

            var accessToken = Settings.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/Cities");

            IEnumerable<City> cities = new List<City>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                cities = JsonConvert.DeserializeObject<IEnumerable<City>>(json);
            }

            return cities;
        }
    }
}
