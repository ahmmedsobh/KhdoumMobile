using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.Views.OrdersViews;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels.CartViewModels
{
    class CartViewModel:BaseViewModel
    {
        public IProductsService ProductsService => DependencyService.Get<IProductsService>();
        public ICartService CartService => DependencyService.Get<ICartService>();

        public IFavoriteService FavoriteService => DependencyService.Get<IFavoriteService>();


        private CartItem _selectedItem;

        public ObservableCollection<CartItem> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command<CartItem> ItemTapped { get; }

        public CartViewModel()
        {
            Items = new ObservableCollection<CartItem>();
            ItemTapped = new Command<CartItem>(OnItemSelected);
            LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand());

            AppShell.StaticAppViewModel.CartValue = null;
        }

        void ExecuteLoadItemsCommand()
        {

            //var IsConnected = await connectionService.IsConnected();

            //if (!IsConnected)
            //    return;

            IsBusy = true;

            try
            {
                FillItems();
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
            SelectedItem = null;
            AppShell.StaticAppViewModel.CartValue = null;
        }

        public CartItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        decimal cartTotalAmount;
        public decimal CartTotalAmount
        {
            get => cartTotalAmount;
            set
            {
                SetProperty(ref cartTotalAmount, value);
            }
        }


        public ICommand IncreaseCounter
        {
            get
            {
                return new Command<CartItem>((i) =>
                {
                    i.CounterValue += i.QuantityDuration;
                    if (i.CounterValue == i.CounterValueToCompare)
                    {
                        i.AddCartItemBtnColor = "#0972ce";
                    }
                    else
                    {
                        i.AddCartItemBtnColor = "Red";
                    }
                });
            }
        }

        public ICommand DecreaseCounter
        {
            get
            {
                return new Command<CartItem>((i) =>
                {
                    if (i.CounterValue > i.QuantityDuration)
                    {
                        i.CounterValue -= i.QuantityDuration;
                    }

                    if (i.CounterValue == i.CounterValueToCompare)
                    {
                        i.AddCartItemBtnColor = "#0972ce";
                    }
                    else
                    {
                        i.AddCartItemBtnColor = "Red";
                    }

                });
            }
        }

        public ICommand AddItemToCartCommand
        {
            get
            {
                return new Command<CartItem>(async (i) =>
                {
                    if (i.CounterValue != i.CounterValueToCompare)
                    {
                        var item = new CartItem()
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Quantity = i.CounterValue,
                            Price = i.Price,
                            TotalPrice = i.Price * i.CounterValue,
                            ImgUrl = i.ImgUrl,
                            QuantityDuration = i.QuantityDuration,
                            CounterValue = i.CounterValue,
                            CounterValueToCompare = i.CounterValue,
                            AddCartItemBtnColor = i.AddCartItemBtnColor,
                            UnitName = i.UnitName,
                            MarketName = i.MarketName,
                            ProductId = i.ProductId,
                            MarketId = i.MarketId,
                            StateId = i.StateId,
                            StateName = i.StateName,
                        };

                        var r = await CartService.AddCartItem(item);
                        if (r)
                        {
                            AppShell.StaticAppViewModel.CartValue = AppShell.StaticAppViewModel.CartValue == null ? 1 : AppShell.StaticAppViewModel.CartValue += 1;

                            SoundPlayer.Play("notification1");
                            try
                            {
                                CrossToastPopUp.Current.ShowToastMessage("تمت الاضافة");

                            }
                            catch
                            {

                            }
                            FillItems();
                        }


                    }
                });
            }
        }

        public ICommand AddFavoriteCommand
        {
            get
            {
                return new Command<CartItem>(async (i) =>
                {

                    bool r;
                    if (i.AddFavoriteBtnColor == "Red")
                    {
                        r = await FavoriteService.DeleteProduct(i.Id);
                        //if(r)
                        //{
                        i.AddFavoriteBtnColor = "DarkGray";
                        //}
                    }
                    else if (i.AddFavoriteBtnColor == "DarkGray")
                    {
                        var p = new Product()
                        {
                            ID = i.Id,
                            Name = i.Name,
                            Price = i.Price,
                            ImgUrl = i.ImgUrl,
                            QuantityDuration = i.QuantityDuration,
                            UnitName = i.UnitName,
                            MarketName = i.MarketName,
                            MarketId = i.MarketId,
                            ProductId = i.ProductId,
                            StateId = i.StateId,
                            StateName = i.StateName
                        };

                        r = await FavoriteService.AddProduct(p);
                        //if (r)
                        //{
                        i.AddFavoriteBtnColor = "Red";
                        //}
                    }

                });
            }
        }

        public ICommand DeleteItemFromCartCommand
        {
            get
            {
                return new Command<CartItem>(async (i) =>
                {
                    var r = await CartService.DeleteCartItem(i.Id);
                    if (r)
                    {
                        try
                        {
                            CrossToastPopUp.Current.ShowToastMessage("تم الحذف");
                        }
                        catch (Exception ex)
                        {

                        }
                        Items.Remove(i);
                        UpdateCartTotalAmount();
                    }
                });
            }
        }

        public ICommand DeleteAllItemsFromCartCommand
        {
            get
            {
                return new Command(async () =>
                {
                    foreach(var item in Items)
                    {
                        await CartService.DeleteCartItem(item.Id);
                    }

                    try
                    {
                        CrossToastPopUp.Current.ShowToastMessage("تم الحذف");
                    }
                    catch
                    {

                    }
                    FillItems();
                    
                });
            }
        }

        public ICommand ConfirmOrderCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if(Items.Count() > 0)
                    {
                        await Shell.Current.GoToAsync(nameof(AddOrderPage));
                    }
                });
            }
        }


        async void OnItemSelected(CartItem Item)
        {
            if (Item == null)
                return;

            SelectedItem = null;
            //await Shell.Current.GoToAsync($"{nameof(SupCategoryPage)}?{nameof(SupCategoryViewModel.CategoryId)}={category.ID}");
        }

        async void FillItems()
        {

            IEnumerable<Product> favorites = new List<Product>();

            if ((await FavoriteService.GetProducts()) != null)
            {
                if ((await FavoriteService.GetProducts()).Count() > 0)
                {
                    favorites = await FavoriteService.GetProducts();
                }
            }


            Items.Clear();
            var items = await CartService.GetCartItems();

            decimal TotalAmount = 0;
            foreach (var item in items)
            {
                var favorite = favorites.FirstOrDefault(f => f.ID == item.Id);
                if (favorite == null)
                    item.AddFavoriteBtnColor = "DarkGray";
                else
                    item.AddFavoriteBtnColor = "Red";


                TotalAmount += item.TotalPrice;
                Items.Add(item);
            }

            CartTotalAmount = TotalAmount;
        }

        void UpdateCartTotalAmount()
        {
            decimal TotalAmount = 0;
            if(Items != null)
            {
                if(Items.Count > 0)
                {
                    TotalAmount = Items.Select(i => i.TotalPrice).Sum();
                }
            }
            CartTotalAmount = TotalAmount;
        }


    }
}
