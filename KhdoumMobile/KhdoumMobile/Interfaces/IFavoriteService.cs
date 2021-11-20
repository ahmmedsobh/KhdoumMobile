using KhdoumMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Interfaces
{
    interface IFavoriteService
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(long Id);
        Task<bool> AddProduct(Product Item);
        Task<bool> DeleteProduct(long Id);
        Task<bool> DeleteAllProducts();
    }
}
