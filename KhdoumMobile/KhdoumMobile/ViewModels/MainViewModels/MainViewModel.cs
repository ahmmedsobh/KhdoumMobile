using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.ViewModels.SupCategoryViewModels;
using KhdoumMobile.Views.SupCategoryViews;
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
        public ObservableCollection<Category> Categories { get; }

        public Command LoadCategoriesCommand { get; }
        public Command<Item> ItemTapped { get; }

        public MainViewModel()
        {
            Categories = new ObservableCollection<Category>();
            LoadCategoriesCommand = new Command(async () => await ExecuteLoadCategoriesCommand());
            ItemTapped = new Command<Item>(OnItemSelected);
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
            if(item.LevelStatus)
            {
                await Shell.Current.GoToAsync($"{nameof(SupCategoryPage)}?{nameof(SupCategoryViewModel.CategoryId)}={item.Id}");
            }
            //await Shell.Current.GoToAsync($"{item.Url}");
        }
    }
}
