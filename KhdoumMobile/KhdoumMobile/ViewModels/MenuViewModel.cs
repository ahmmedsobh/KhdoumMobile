using KhdoumMobile.Helpers;
using KhdoumMobile.Models;
using KhdoumMobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels
{
    class MenuViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; }
        public ObservableCollection<Item> Medias { get; }

        public MenuViewModel()
        {
            Items = new ObservableCollection<Item>();
            Medias = new ObservableCollection<Item>();

            Name = Settings.Name;
            Phone = Settings.Username;

            FillItems();
            FillMedias();

            
        }


        Item selectedItem;
        public Item SelectedItem 
        {
            get => selectedItem;
            set
            {
                SetProperty(ref selectedItem,value);
                OnItemSelected(value);
            }
        }

        Item selectedMedia;
        public Item SelectedMedia
        {
            get => selectedMedia;
            set
            {
                SetProperty(ref selectedMedia, value);
                OnMediaSelected(value);
            }
        }

        string name;
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
            }
        }

        string phone;
        public string Phone
        {
            get => phone;
            set
            {
                SetProperty(ref phone, value);
            }
        }


        List<Item> ItemsList()
        {
            var items = new List<Item>() 
            {
                new Item(){Id = "1",Text = "الاشعارات",Icon="bell" ,PageLink = "NotificationsPage"},    
                new Item(){Id = "2",Text = "عن خدوم",Icon="\uf05a" ,PageLink = "AboutKhdoumPage"},
                new Item(){Id = "3",Text = "تواصل معنا",Icon="\uf0e0",PageLink = "ContactUsPage" },
                new Item(){Id = "4",Text = "تسجيل الخروج",Icon="\uf2f5",PageLink = "" },
            };

            return items;
        }

        List<Item> MediaList()
        {
            var items = new List<Item>()
            {
                new Item(){Id = "1",Text = "موقعنا الالكترونى",Icon="internet-explorer" ,PageLink = "http://www.khdoum.com/",IconColor="#0972ce"},
                new Item(){Id = "1",Text = "فيس بوك",Icon="facebook-square" ,PageLink = "https://www.facebook.com/khdoumEG",IconColor="#0000cd"},
                new Item(){Id = "2",Text = "واتساب",Icon="\uf40c" ,PageLink = "https://wa.me/201064641608",IconColor="#18e96b"},
                new Item(){Id = "3",Text = "يوتيوب",Icon="\uf431",PageLink = "https://www.facebook.com/khdoumEG",IconColor="#ec1717" },
                new Item(){Id = "4",Text = "انستجرام",Icon="\ue055",PageLink = "https://www.facebook.com/khdoumEG",IconColor="#e4168b" },
            };

            return items;
        }

        void FillItems()
        {
            Items.Clear();
            var items = ItemsList();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        void FillMedias()
        {
            Medias.Clear();
            var medias = MediaList();
            foreach (var item in medias)
            {
                Medias.Add(item);
            }
        }

        async void OnItemSelected(Item item)
        {
            try
            {
                if(item != null)
                {
                    SelectedItem = null;

                    if (!string.IsNullOrEmpty(item.PageLink))
                        await Shell.Current.GoToAsync(item.PageLink);
                    else
                    {
                        EmptySettings();
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    }
                }
                
            }
            catch(Exception ex)
            {

            }
           


        }

        void EmptySettings()
        {
            Settings.Name = "";
            Settings.Phone = "";
            Settings.Password = "";
            Settings.AccessToken = "";
            Settings.Address = "";
            Settings.City = 0;
            Settings.State = 0;
            Settings.Username = "";
        }

        async void OnMediaSelected(Item item)
        {
            try
            {
                SelectedItem = null;
                await Browser.OpenAsync(item.PageLink);
            }
            catch(Exception ex)
            {

            }
            
        }

    }
}
