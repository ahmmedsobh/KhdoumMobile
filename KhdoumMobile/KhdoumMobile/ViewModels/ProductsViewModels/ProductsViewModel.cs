using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.ViewModels.SupCategoryViewModels;
using KhdoumMobile.Views.SupCategoryViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Plugin.Toast;

namespace KhdoumMobile.ViewModels.ProductsViewModels
{
    [QueryProperty(nameof(CategoryId), nameof(CategoryId))]
    class ProductsViewModel :BaseViewModel
    {
        public IProductsService ProductsService => DependencyService.Get<IProductsService>();
        public ICartService CartService => DependencyService.Get<ICartService>();
        public IFavoriteService FavoriteService => DependencyService.Get<IFavoriteService>();

        private Product _selectedProduct;

        public ObservableCollection<Product> Products { get; }
        public Command LoadProductsCommand { get; }
        public Command<Product> ProductTapped { get; }

        public ProductsViewModel()
        {
            Products = new ObservableCollection<Product>();

            ProductTapped = new Command<Product>(OnProductSelected);
             
            LoadProductsCommand = new Command( () => ExecuteLoadProductsCommand(CategoryId));
        }

        async void ExecuteLoadProductsCommand(long CategoryId)
        {

            //var IsConnected = await connectionService.IsConnected();

            //if (!IsConnected)
            //    return;

            //IsBusy = true;

            try
            {
                var products = await ProductsService.GetProductsAsync(CategoryId);

                if(products.Count() > 0)
                {
                    CategoryName = products.ToList()[0].CategoryName;
                }

                FillProducts();

              
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                //IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedProduct = null;
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                SetProperty(ref _selectedProduct, value);
                OnProductSelected(value);
            }
        }

        long categoryId;
        public long CategoryId
        {
            get => categoryId;
            set
            {
                SetProperty(ref categoryId, value);
                ExecuteLoadProductsCommand(value);
            }
        }

        string categoryName;
        public string CategoryName 
        {
            get => categoryName;
            set
            {
                SetProperty(ref categoryName, value);
            } 
        }

        string searchString;
        public string SearchString
        {
            get => searchString;
            set
            {
                SetProperty(ref searchString, value);
            }
        }





        public ICommand IncreaseCounter
        {
            get
            {
                return new Command<Product>((p) =>
                {
                    p.CounterValue += p.QuantityDuration;
                    p.AddCartItemBtnColor = "Red";
                });
            }
        }

        public ICommand DecreaseCounter
        {
            get
            {
                return new Command<Product>((p) =>
                {
                    if(p.CounterValue > 0)
                    {
                        p.CounterValue -= p.QuantityDuration;
                    }

                    if (p.CounterValue == 0)
                    {
                        p.AddCartItemBtnColor = "#0972ce";
                    }
                    
                });
            }
        }

        public ICommand AddItemToCartCommand
        {
            get
            {
                return new Command<Product>(async(p) =>
                {
                   if(p.CounterValue >= 0.1M)
                    {
                        var item = new CartItem()
                        {
                            Id = p.ID,
                            Name = p.Name,
                            Quantity = p.CounterValue,
                            Price = p.Price,
                            TotalPrice = p.Price * p.CounterValue,
                            ImgUrl = p.ImgUrl,
                            QuantityDuration = p.QuantityDuration,
                            CounterValue = p.CounterValue,
                            CounterValueToCompare  = p.CounterValue,
                            //AddCartItemBtnColor = p.AddCartItemBtnColor,
                            UnitName = p.UnitName,
                            MarketName = p.MarketName,
                            ProductId = p.ProductId,
                            MarketId = p.MarketId,
                            StateId = p.StateId,
                            StateName = p.StateName,
                        };

                        var r =  await CartService.AddCartItem(item);
                        if(r)
                        {

                            AppShell.StaticAppViewModel.CartValue = AppShell.StaticAppViewModel.CartValue == null?1: AppShell.StaticAppViewModel.CartValue += 1;
                            SoundPlayer.Play("notification1");
                            try
                            {
                                CrossToastPopUp.Current.ShowToastMessage("تمت الاضافة");
                                
                            }
                            catch
                            {

                            }
                           
                        }
                    }
                });
            }
        }

        public ICommand AddFavoriteCommand
        {
            get
            {
                return new Command<Product>(async (p) =>
                {
                    
                    bool r;
                    if(p.AddFavoriteBtnColor == "Red")
                    {
                        r = await FavoriteService.DeleteProduct(p.ID);
                        //if(r)
                        //{
                            p.AddFavoriteBtnColor = "DarkGray";
                        //}
                    }
                    else if(p.AddFavoriteBtnColor == "DarkGray")
                    {
                            r = await FavoriteService.AddProduct(p);
                            //if (r)
                            //{
                                p.AddFavoriteBtnColor = "Red";
                            //}
                    }
                    
                });
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new Command(() =>
                {
                    var SearchString = this.SearchString;
                    if(SearchString == null || SearchString == "")
                    {
                        FillProducts();
                        return;
                    }

                    var SearchResult = Products.Where(p => p.Name.Contains(SearchString) || p.Price.ToString().Contains(SearchString) || p.UnitName.Contains(SearchString) || p.CategoryName.Contains(SearchString)).ToList();

                    if (SearchResult.Count() > 0)
                    {
                        Products.Clear();
                        foreach (var item in SearchResult)
                        {
                            Products.Add(item);
                        }
                    }
                    else
                    {
                        FillProducts();
                    }
                });
            }
        }
        async void OnProductSelected(Product Product)
        {
            if (Product == null)
                return;

            SelectedProduct = null;
            //await Shell.Current.GoToAsync($"{nameof(SupCategoryPage)}?{nameof(SupCategoryViewModel.CategoryId)}={category.ID}");
        }

        async void FillProducts()
        {
            //IsBusy = true;
            Products.Clear();

            IEnumerable<Product> favorites = new List<Product>();

            if((await FavoriteService.GetProducts()) != null)
            {
                if((await FavoriteService.GetProducts()).Count() > 0)
                {
                    favorites = await FavoriteService.GetProducts();
                }
            }

            var products = await ProductsService.GetProductsAsync(CategoryId);
            foreach (var item in products)
            {
                var favorite = favorites.FirstOrDefault(f => f.ID == item.ID);
                if (favorite == null)
                    item.AddFavoriteBtnColor = "DarkGray";
                else
                    item.AddFavoriteBtnColor = "Red";

                Products.Add(item);
            }

            IsBusy = false;

        }

    }
}
