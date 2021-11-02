using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Services
{
    class CategoryService : ICategoryService
    {
        public async Task<IEnumerable<SupCategory>> GetChildCategories(long CategoryId)
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/categories/GetChildCategories/{CategoryId}");

            IEnumerable<SupCategory> categories = new List<SupCategory>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<IEnumerable<SupCategory>>(json);
            }

            return await Task.FromResult(categories);
        }

        public Task<Item> GetItemAsync(string ItemID, int CategoryID = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetItemsAsync(bool Short = true)
        {
            var client = new HttpClient();


            HttpResponseMessage response = await client.GetAsync($"{Constants.BaseApiAddress}api/categories/GetFrom1To2LevelCategories");

            IEnumerable<Item> items = new List<Item>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                items = JsonConvert.DeserializeObject<IEnumerable<Item>>(json);
            }
            //if (Short)
            //{
            //    return (from i in items
            //            group i by new { i.CategoryId, i.CategoryName, i.CategoryIcon, i.CategoryIconColor } into Group
            //            select new Category(Group.Key.CategoryId, Group.Key.CategoryName, Group.Key.CategoryIcon, Group.Key.CategoryIconColor, Group.ToList().Take(9).ToList())).ToList();
            //}
            //else
            //{
            return (from i in items
                        group i by new { i.CategoryId, i.CategoryName, i.CategoryIcon, i.CategoryIconColor } into Group
                        select new Category(Group.Key.CategoryId, Group.Key.CategoryName, Group.Key.CategoryIcon, Group.Key.CategoryIconColor, Group.ToList())).ToList();
            //}
        }

        public async Task<Category> GetItemsForCategoryAsync(int ID)
        {
            var Items = await GetItemsAsync(false);
            var category = Items.FirstOrDefault(c => c.Id == ID);
            return category;
        }
    }
}
