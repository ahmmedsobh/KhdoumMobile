using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels.FavoriteViewModels
{
    class FavoriteViewModel:BaseViewModel
    {
        public IProductsService ProductsService => DependencyService.Get<IProductsService>();
        public ICartService CartService => DependencyService.Get<ICartService>();
        public IFavoriteService FavoriteService => DependencyService.Get<IFavoriteService>();
        private Product _selectedProduct;
        public ObservableCollection<Product> Products { get; }
        public Command LoadProductsCommand { get; }
        public Command<Product> ProductTapped { get; }
        public FavoriteViewModel()
        {
            Products = new ObservableCollection<Product>();

            ProductTapped = new Command<Product>(OnProductSelected);
            LoadProductsCommand = new Command(() => ExecuteLoadProductsCommand());
        }
        void ExecuteLoadProductsCommand()
        {

            //var IsConnected = await connectionService.IsConnected();

            //if (!IsConnected)
            //    return;

            IsBusy = true;

            try
            {
                FillProducts();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
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
                    if (p.CounterValue > 0)
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
                return new Command<Product>(async (p) =>
                {
                    if (p.CounterValue > 0.1M)
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
                            AddCartItemBtnColor = p.AddCartItemBtnColor,
                            UnitName = p.UnitName,
                            MarketName = p.MarketName,
                            ProductId = p.ProductId,
                            MarketId = p.MarketId
                        };

                        var r = await CartService.AddCartItem(item);
                        if (r)
                        {
                            CrossToastPopUp.Current.ShowToastMessage("تمت الاضافة");
                        }
                    }
                });
            }
        }
        public ICommand DeleteProductCommand
        {
            get
            {
                return new Command<Product>(async (p) =>
                {
                    var r = await FavoriteService.DeleteProduct(p.ID);
                    if(r)
                    {
                        Products.Remove(p);
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
            Products.Clear();
            var products = await FavoriteService.GetProducts();
            foreach (var item in products)
            {
                Products.Add(item);
            }
        }
    }
}
