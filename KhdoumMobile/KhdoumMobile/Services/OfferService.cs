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
    class OfferService : IOfferService
    {
        public async Task<IEnumerable<Product>> GetOffersAsync()
        {
            var client = new HttpClient();

            var accessToken = Settings.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/Offers/GetOffersForMobileApp");

            IEnumerable<Product> offers = new List<Product>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                offers = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
            }

            return await Task.FromResult(offers);
        }
    }
}
