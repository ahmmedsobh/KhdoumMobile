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

namespace KhdoumMobile.ViewModels.CartViewModels
{
    class CartViewModel:BaseViewModel
    {
        public IProductsService ProductsService => DependencyService.Get<IProductsService>();
        public ICartService CartService => DependencyService.Get<ICartService>();

        private CartItem _selectedItem;

        public ObservableCollection<CartItem> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command<CartItem> ItemTapped { get; }

        public CartViewModel()
        {
            Items = new ObservableCollection<CartItem>();
            ItemTapped = new Command<CartItem>(OnItemSelected);
            LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand());
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
                    i.AddCartItemBtnColor = "Red";
                });
            }
        }

        public ICommand DecreaseCounter
        {
            get
            {
                return new Command<CartItem>((i) =>
                {
                    if (i.CounterValue > 0)
                    {
                        i.CounterValue -= i.QuantityDuration;
                    }

                    if (i.CounterValue == 0)
                    {
                        i.AddCartItemBtnColor = "#0972ce";
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
                    if (i.CounterValue > 0.1M)
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
                            AddCartItemBtnColor = i.AddCartItemBtnColor,
                            UnitName = i.UnitName,
                            MarketName = i.MarketName

                        };

                        var r = await CartService.AddCartItem(item);
                        if (r)
                        {
                            CrossToastPopUp.Current.ShowToastMessage("تمت الاضافة");
                            FillItems();
                        }


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
                    if(r)
                    {
                        CrossToastPopUp.Current.ShowToastMessage("تم الحذف");
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
                    CrossToastPopUp.Current.ShowToastMessage("تم الحذف");
                    FillItems();
                    
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
            Items.Clear();
            var items = await CartService.GetCartItems();

            decimal TotalAmount = 0;
            foreach (var item in items)
            {
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
