using KhdoumMobile.Interfaces;
using KhdoumMobile.Models;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels.NotificationsViewModels
{
    class NotificationsViewModel:BaseViewModel
    {
        public INotificationService NotificationService => DependencyService.Get<INotificationService>();

        public ObservableCollection<Notification> Notifications { get; }
        public NotificationsViewModel()
        {
            Notifications = new ObservableCollection<Notification>();
        }

        void ExecuteLoadNotificationsCommand()
        {

            //var IsConnected = await connectionService.IsConnected();

            //if (!IsConnected)
            //    return;

            IsBusy = true;

            try
            {
                FillNotifications();
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

        public ICommand LoadNotificationsCommand {
            get
            {
                return new Command(()=>ExecuteLoadNotificationsCommand());
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
      
        async void FillNotifications()
        {
            try
            {
                Notifications.Clear();
                var notifications = await NotificationService.GetNotificationsForUser();
                foreach (var item in notifications)
                {
                    Notifications.Add(item);
                }
            }
            catch(Exception ex)
            {

            }
            
        }
    }
}
