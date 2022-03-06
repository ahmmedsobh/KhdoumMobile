using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using KhdoumMobile.Models.ViewModels;
using KhdoumMobile.Services;
using KhdoumMobile.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            
        }

        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();


        public List<PickerViewModel<int>> StatusList { get; set; } = new List<PickerViewModel<int>>()
        {
            new PickerViewModel<int>{Value = 0,Name="الكل"},
            new PickerViewModel<int>{Value = 1,Name="انتظار"},
            new PickerViewModel<int>{Value = 2,Name="يحهز"},
            new PickerViewModel<int>{Value = 3,Name="مجهز"},
            new PickerViewModel<int>{Value = 4,Name="يسلم"},
            new PickerViewModel<int>{Value = 5,Name="مكتمل"},
            new PickerViewModel<int>{Value = 6,Name="ملغى"},
        };


        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public string Logo
        {
            get => "KhdoumMobile.Resources.Images.KhdoumLogo.png";
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
