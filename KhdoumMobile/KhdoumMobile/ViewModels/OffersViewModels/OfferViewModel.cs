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

namespace KhdoumMobile.ViewModels.OffersViewModels
{
    class OfferViewModel:BaseViewModel
    {
        public IOfferService OfferService => DependencyService.Get<IOfferService>();
        public ICartService CartService => DependencyService.Get<ICartService>();
        public IFavoriteService FavoriteService => DependencyService.Get<IFavoriteService>();

        private Product _selectedOffer;

        public ObservableCollection<Product> Offers { get; }
        public Command LoadOffersCommand { get; }
        public Command<Product> OfferTapped { get; }

        public OfferViewModel()
        {
            Offers = new ObservableCollection<Product>();

            OfferTapped = new Command<Product>(OnOfferSelected);

            LoadOffersCommand = new Command(ExecuteLoadOffersCommand);
            ExecuteLoadOffersCommand();
        }

        async void ExecuteLoadOffersCommand()
        {

            //var IsConnected = await connectionService.IsConnected();

            //if (!IsConnected)
            //    return;

            IsBusy = true;

            try
            {
                var offers = await OfferService.GetOffersAsync();

                if (offers.Count() > 0)
                {
                    CategoryName = offers.ToList()[0].CategoryName;
                }

                FillOffers();


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
            SelectedOffer = null;
        }

        public Product SelectedOffer
        {
            get => _selectedOffer;
            set
            {
                SetProperty(ref _selectedOffer, value);
                OnOfferSelected(value);
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
                    if (p.CounterValue >= 0.1M)
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
                            CounterValueToCompare = p.CounterValue,
                            //AddCartItemBtnColor = p.AddCartItemBtnColor,
                            UnitName = p.UnitName,
                            MarketName = p.MarketName,
                            ProductId = p.ProductId,
                            MarketId = p.MarketId,
                            StateId = p.StateId,
                            StateName = p.StateName,
                        };

                        var r = await CartService.AddCartItem(item);
                        if (r)
                        {
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
                    if (p.AddFavoriteBtnColor == "Red")
                    {
                        r = await FavoriteService.DeleteProduct(p.ID);
                        //if(r)
                        //{
                        p.AddFavoriteBtnColor = "DarkGray";
                        //}
                    }
                    else if (p.AddFavoriteBtnColor == "DarkGray")
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
                    if (SearchString == null || SearchString == "")
                    {
                        FillOffers();
                        return;
                    }

                    var SearchResult = Offers.Where(p => p.Name.Contains(SearchString) || p.Price.ToString().Contains(SearchString) || p.UnitName.Contains(SearchString) || p.CategoryName.Contains(SearchString)).ToList();

                    if (SearchResult.Count() > 0)
                    {
                        Offers.Clear();
                        foreach (var item in SearchResult)
                        {
                            Offers.Add(item);
                        }
                    }
                    else
                    {
                        FillOffers();
                    }
                });
            }
        }
        async void OnOfferSelected(Product Offer)
        {
            if (Offer == null)
                return;

            SelectedOffer = null;
            //await Shell.Current.GoToAsync($"{nameof(SupCategoryPage)}?{nameof(SupCategoryViewModel.CategoryId)}={category.ID}");
        }

        async void FillOffers()
        {
            Offers.Clear();

            IEnumerable<Product> favorites = new List<Product>();

            if ((await FavoriteService.GetProducts()) != null)
            {
                if ((await FavoriteService.GetProducts()).Count() > 0)
                {
                    favorites = await FavoriteService.GetProducts();
                }
            }

            var offers = await OfferService.GetOffersAsync();
            foreach (var item in offers)
            {
                var favorite = favorites.FirstOrDefault(f => f.ID == item.ID);
                if (favorite == null)
                    item.AddFavoriteBtnColor = "DarkGray";
                else
                    item.AddFavoriteBtnColor = "Red";

                Offers.Add(item);
            }
        }
    }
}
