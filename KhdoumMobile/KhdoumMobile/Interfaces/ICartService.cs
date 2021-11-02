using KhdoumMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Interfaces
{
    interface ICartService
    {
        Task<IEnumerable<CartItem>> GetCartItems();
        Task<CartItem> GetCartItem(long Id);
        Task<bool> AddCartItem(CartItem Item);
        Task<bool> DeleteCartItem(long Id);
        Task<bool> UpdateCartItem(CartItem Item,List<CartItem> CartItems);
    }
}
