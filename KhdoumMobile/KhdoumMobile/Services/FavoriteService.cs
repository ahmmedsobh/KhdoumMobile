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

namespace KhdoumMobile.Services
{
    class FavoriteService : IFavoriteService
    {
        string FileName = "FavoriteFile";
        string FolderName = "FavoriteFolder";
        public async Task<bool> AddProduct(Product Item)
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
                    await PCLFileStorage.CreateFile(FileName, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName));
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

        public async Task<bool> DeleteAllProducts()
        {
            var result = false;
            if (await PCLFileStorage.IsFolderExistAsync(FolderName))
            {
                if (await PCLFileStorage.IsFileExistAsync(FileName, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName)))
                {
                    var Products = new List<Product>();
                    var ProductsToJson = JsonConvert.SerializeObject(Products);
                    result = await PCLFileStorage.WriteTextAllAsync(FileName, ProductsToJson, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName));
                }
            }

            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteProduct(long Id)
        {
            var result = false;
            if (await PCLFileStorage.IsFolderExistAsync(FolderName))
            {
                if (await PCLFileStorage.IsFileExistAsync(FileName, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName)))
                {
                    var Products = await ProductsList();
                    var product = Products.FirstOrDefault(p => p.ID == Id);
                    Products.Remove(product);
                    var ProductsToJson = JsonConvert.SerializeObject(Products);
                    result = await PCLFileStorage.WriteTextAllAsync(FileName, ProductsToJson, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName));
                }
            }

            return await Task.FromResult(result);
        }

        public Task<Product> GetProduct(long Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            IEnumerable<Product> Products = new List<Product>();

            try
            {
                if (await PCLFileStorage.IsFileExistAsync(FileName, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName)))
                {
                    string json = await PCLFileStorage.ReadAllTextAsync(FileName, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName));
                    Products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                    return Products;
                }
            }
            catch (Exception ex)
            {

            }


            return Products;
        }

        async Task<bool> AddItem(Product Product)
        {
            var result = false;
            var Products = await ProductsList();

            var product = Products.FirstOrDefault(p => p.ID == Product.ID);

            if (product == null)
            {
                Products.Add(Product);
                var ItemsToJson = JsonConvert.SerializeObject(Products);
                result = await PCLFileStorage.WriteTextAllAsync(FileName, ItemsToJson, await FileSystem.Current.LocalStorage.GetFolderAsync(FolderName));
            }

            return await Task.FromResult(result);
        }

        async Task<List<Product>> ProductsList()
        {
            var Products = new List<Product>();

            var ProductsAsync = await GetProducts();
            if(ProductsAsync != null)
            {
                Products = ProductsAsync.ToList();
            }
            

            return Products;
        }

       
    }
}
