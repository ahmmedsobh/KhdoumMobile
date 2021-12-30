using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KhdoumMobile.Services
{
    class CartItemService : ICartService
    {
        string FileName = "CartFile";
        string FolderName = "CartFolder";
        public IProductsService ProductsService => DependencyService.Get<IProductsService>();

        public async Task<bool> AddCartItem(CartItem Item)
        {
            var result = false;
            if (await PCLFileStorage.IsFolderExistAsync(FolderName))
            {
                if (await PCLFileStorage.IsFileExistAsync(FileName, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName)))
                {
                    result = await AddItem(Item);
                }
                else
                {
                    await PCLFileStorage.CreateFile(FileName,await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName));
                    if (await PCLFileStorage.IsFolderExistAsync(FolderName))
                    {
                        if (await PCLFileStorage.IsFileExistAsync(FileName, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName)))
                        {
                            result = await AddItem(Item);
                        }
                    }
                }
            }
            else
            {
                await PCLFileStorage.CreateFolder(FolderName);
                await PCLFileStorage.CreateFile(FileName, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName));
                if (await PCLFileStorage.IsFolderExistAsync(FolderName))
                {
                    if (await PCLFileStorage.IsFileExistAsync(FileName, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName)))
                    {
                        result = await AddItem(Item);
                    }
                }
            }

            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteAllCartItems()
        {
            var CartItems = new List<CartItem>();
            var ItemsToJson = JsonConvert.SerializeObject(CartItems);
            var result = await PCLFileStorage.WriteTextAllAsync(FileName, ItemsToJson, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName));
            if(result)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);

        }

        public async Task<bool> DeleteCartItem(long Id)
        {
            var result = false;
            if (await PCLFileStorage.IsFolderExistAsync(FolderName))
            {
                if (await PCLFileStorage.IsFileExistAsync(FileName, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName)))
                {
                    var CartItems = await CartItemsList();
                    var item = CartItems.FirstOrDefault(i => i.Id == Id);
                    CartItems.Remove(item);
                    var ItemsToJson = JsonConvert.SerializeObject(CartItems);
                    result = await PCLFileStorage.WriteTextAllAsync(FileName, ItemsToJson, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName));
                }
            }

            return await Task.FromResult(result);
        }

        public Task<CartItem> GetCartItem(long Id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<CartItem>> GetCartItems()
        {
            IEnumerable<CartItem> CartItems = new List<CartItem>();

            try
            {
                if (await PCLFileStorage.IsFileExistAsync(FileName, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName)))
                {
                    string json = await PCLFileStorage.ReadAllTextAsync(FileName, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName));
                    CartItems = JsonConvert.DeserializeObject<IEnumerable<CartItem>>(json);
                    return CartItems;
                }
            }
            catch (Exception ex)
            {

            }


            return CartItems;
        }
        public async Task<bool> UpdateCartItem(CartItem Item ,List<CartItem> cartItems)
        {
            var result = false;
            if (await PCLFileStorage.IsFolderExistAsync(FolderName))
            {
                if (await PCLFileStorage.IsFileExistAsync(FileName, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName)))
                {
                    //var CartItems = await CartItemsList();
                    var CartItems = new List<CartItem>();
                    if (cartItems != null)
                    {
                        if(cartItems.Count > 0)
                        {
                            CartItems = cartItems;
                        }
                    }

                    var index = CartItems.FindIndex(i => i.Id == Item.Id);

                    CartItems[index].CounterValue = Item.CounterValue;
                    CartItems[index].Price = Item.Price;
                    CartItems[index].TotalPrice = Item.TotalPrice;

                    var ItemsToJson = JsonConvert.SerializeObject(CartItems);
                    result = await PCLFileStorage.WriteTextAllAsync(FileName, ItemsToJson, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName));
                }
            }

            return await Task.FromResult(result);
        }
        async Task<bool> AddItem(CartItem Item)
        {
            var result = false;
            var CartItems = await CartItemsList();


            var item = CartItems.FirstOrDefault(i => i.Id == Item.Id);
            if (item != null)
            {
               result = await UpdateCartItem(Item, CartItems);
            }
            else
            {
                CartItems.Add(Item);
                var ItemsToJson = JsonConvert.SerializeObject(CartItems);
                result = await PCLFileStorage.WriteTextAllAsync(FileName, ItemsToJson, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName));
            }

           
            return await Task.FromResult(result);
        }

        async Task<List<CartItem>> CartItemsList()
        {
            var CartItems = new List<CartItem>();

            var CartItemsAsync = await GetCartItems();
            if (CartItemsAsync != null)
            {
                CartItems = await CheckPriceChanged(CartItemsAsync.ToList());
            }

            return CartItems;
        }

        async Task<List<CartItem>> CheckPriceChanged(List<CartItem> CartItems)
        {
            var CartItemsToReturn = new List<CartItem>();
            foreach(var item in CartItems)
            {
                var product = await ProductsService.GetProductAsync(item.ProductId);
                if(product != null)
                {
                    if(product.ID != 0)
                    {
                        if (product.Price != item.Price)
                        {
                            item.Price = product.Price;
                            item.TotalPrice = product.Price * item.CounterValue;
                            await UpdateCartItem(item, CartItems);
                        }
                    }

                    CartItemsToReturn.Add(item);
                }
            }

            return await Task.FromResult(CartItemsToReturn);
        }
    }
}
