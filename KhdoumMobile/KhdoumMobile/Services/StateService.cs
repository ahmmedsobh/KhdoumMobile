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
    class StateService : IStateService
    {
        public async Task<IEnumerable<State>> GetStates()
        {
            var client = new HttpClient();

            var accessToken = Settings.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/States");

            IEnumerable<State> states = new List<State>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                states = JsonConvert.DeserializeObject<IEnumerable<State>>(json);
            }

            return states;
        }
    }
}
