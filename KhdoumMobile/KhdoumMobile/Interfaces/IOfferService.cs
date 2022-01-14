using KhdoumMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Interfaces
{
    public interface IOfferService
    {
        Task<IEnumerable<Product>> GetOffersAsync();
    }
}
