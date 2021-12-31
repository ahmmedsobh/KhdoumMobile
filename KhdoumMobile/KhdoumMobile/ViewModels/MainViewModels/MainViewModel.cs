using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.ViewModels.ProductsViewModels;
using KhdoumMobile.ViewModels.SupCategoryViewModels;
using KhdoumMobile.Views.ProductsViews;
using KhdoumMobile.Views.SupCategoryViews;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels.MainViewModels
{
    class MainViewModel:BaseViewModel
    {
        public ICategoryService CategoryService => DependencyService.Get<ICategoryService>();
        public INotificationService NotificationService => DependencyService.Get<INotificationService>();
        public ObservableCollection<Category> Categories { get; }

        public Command LoadCategoriesCommand { get; }
        public Command<Item> ItemTapped { get; }

        public MainViewModel()
        {
            Categories = new ObservableCollection<Category>();
            LoadCategoriesCommand = new Command(async () => await ExecuteLoadCategoriesCommand());
            ItemTapped = new Command<Item>(OnItemSelected);

            SaveToken();

        }

        async void  SaveToken()
        {
            string Token = Settings.FirebaseAppToken;
            if(!string.IsNullOrEmpty(Token))
            {
                var Result = await NotificationService.SaveFirebaseAppToken(Token);
                if(Result)
                {
                    Settings.FirebaseAppToken = "";
                }
            }
        }

        async Task ExecuteLoadCategoriesCommand()
        {
            IsBusy = true;
            Categories.Clear();
            var categories = await CategoryService.GetItemsAsync();
            foreach (var c in categories)
            {
                Categories.Add(c);
            }
            IsBusy = false;
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            if(item.PageLink != null && item.PageLink != "")
            {
                await Shell.Current.GoToAsync(item.PageLink);
                return;
            }

            if (item.LevelStatus)
            {
                await Shell.Current.GoToAsync($"{nameof(SupCategoryPage)}?{nameof(SupCategoryViewModel.CategoryId)}={item.Id}&{nameof(SupCategoryViewModel.CategoryName)}={item.CategoryName}");
            }
            else
            {
                await Shell.Current.GoToAsync($"{nameof(ProductsPage)}?{nameof(ProductsViewModel.CategoryId)}={item.Id}");
            }

            //await Shell.Current.GoToAsync($"{item.Url}");
        }
    }
}
