using KhdoumMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Interfaces
{
    interface ICategoryService
    {
        Task<IEnumerable<Category>> GetItemsAsync(bool AllItems = true);
        Task<IEnumerable<SupCategory>> GetChildCategories(long CategoryId);
        Task<Category> GetItemsForCategoryAsync(int ID);
        Task<Item> GetItemAsync(string ItemID, int CategoryID = 0);
    }
}
