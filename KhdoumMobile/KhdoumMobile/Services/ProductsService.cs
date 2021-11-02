using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Services
{
    class ProductsService : IProductsService
    {

        public async Task<Product> GetProductAsync(long ProductId)
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/Products/GetViewProduct/{ProductId}");

            Product product = new Product();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                product = JsonConvert.DeserializeObject<Product>(json);
            }

            return await Task.FromResult(product);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(long CategoryId)
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/Products/GetProductsByCategoryId/{CategoryId}");

            IEnumerable<Product> products = new List<Product>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
            }

            return await Task.FromResult(products);
        }
    }
}
