using KhdoumMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Interfaces
{
    interface IProductsService
    {
        Task<IEnumerable<Product>> GetProductsAsync(long CategoryId);
        Task<Product> GetProductAsync(long ProductId);
    }
}
