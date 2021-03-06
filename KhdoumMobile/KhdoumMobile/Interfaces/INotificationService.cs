using KhdoumMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Interfaces
{
    interface INotificationService
    {
        Task<bool> SaveFirebaseAppToken(string Token);
        Task<IEnumerable<Notification>> GetNotificationsForUser();
    }
}
