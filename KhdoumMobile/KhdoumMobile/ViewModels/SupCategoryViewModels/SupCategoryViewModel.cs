using KhdoumMobile.Helpers;
using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.ViewModels.ProductsViewModels;
using KhdoumMobile.Views.ProductsViews;
using KhdoumMobile.Views.SupCategoryViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels.SupCategoryViewModels
{
    [QueryProperty(nameof(CategoryId), nameof(CategoryId))]
    class SupCategoryViewModel:BaseViewModel
    {
        public ICategoryService CategoryService => DependencyService.Get<ICategoryService>();


        private SupCategory _selectedCategory;

        public ObservableCollection<SupCategory> Categories { get; }
        public Command LoadCategoriesCommand { get; }
        public Command<SupCategory> CategoryTapped { get; }

        public SupCategoryViewModel()
        {
            Categories = new ObservableCollection<SupCategory>();

            CategoryTapped = new Command<SupCategory>(OnCategorySelected);

        }

        async void ExecuteLoadCategoriesCommand(long CategoryId)
        {

            //var IsConnected = await connectionService.IsConnected();

            //if (!IsConnected)
            //    return;

            IsBusy = true;

            try
            {
                Categories.Clear();
                var categories = await CategoryService.GetChildCategories(CategoryId);

                if(categories.Count() > 0)
                {
                    CategoryName = categories.ToList()[0].Name;
                }

                foreach (var item in categories)
                {
                    item.ImgUrl = item.ImgUrl == "false" ? $"{Constants.BaseApiAddress}Uploads/default.png" : $"{Constants.BaseApiAddress}Uploads/Categories/{item.ImgUrl}";
                    Categories.Add(item);
                }
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
            SelectedCategory = null;
        }

        public SupCategory SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                OnCategorySelected(value);
            }
        }

        long categoryId;
        public long CategoryId
        {
            get => categoryId;
            set
            {
                SetProperty(ref categoryId, value);
                ExecuteLoadCategoriesCommand(value);
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


        async void OnCategorySelected(SupCategory category)
        {
            if (category == null)
                return;

            SelectedCategory = null;
            if(category.LevelStatus)
            {
                await Shell.Current.GoToAsync($"{nameof(SupCategoryPage)}?{nameof(SupCategoryViewModel.CategoryId)}={category.ID}");
            }
            else
            {
                await Shell.Current.GoToAsync($"{nameof(ProductsPage)}?{nameof(ProductsViewModel.CategoryId)}={category.ID}");
            }
        }
    }
}
