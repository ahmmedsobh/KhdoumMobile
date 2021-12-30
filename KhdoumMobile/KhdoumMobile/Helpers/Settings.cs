using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace KhdoumMobile.Helpers
{
    class Settings
    {
        public static string Username
        {
            get
            {
                return Preferences.Get("Username", "");
            }
            set
            {
                Preferences.Set("Username", value);
            }
        }

       
        public static string Password
        {
            get
            {
                return Preferences.Get("Password", "");
            }
            set
            {
                Preferences.Set("Password", value);
            }
        }
        public static string AccessToken
        {
            get
            {
                return Preferences.Get("AccessToken", "");
            }
            set
            {
                Preferences.Set("AccessToken", value);
            }
        }

        public static DateTime AccessTokenExpirationDate
        {
            get
            {
                return Preferences.Get("AccessTokenExpirationDate", DateTime.UtcNow);
            }
            set
            {
                Preferences.Set("AccessTokenExpirationDate", value);
            }
        }

        public static string Name
        {
            get
            {
                return Preferences.Get("Name", "");
            }
            set
            {
                Preferences.Set("Name", value);
            }
        }

        public static string Phone
        {
            get
            {
                return Preferences.Get("Phone", "");
            }
            set
            {
                Preferences.Set("Phone", value);
            }
        }

        public static int City
        {
            get
            {
                return Preferences.Get("City", 0);
            }
            set
            {
                Preferences.Set("City", value);
            }
        }

        public static int State
        {
            get
            {
                return Preferences.Get("State", 0);
            }
            set
            {
                Preferences.Set("State", value);
            }
        }

        public static string Address
        {
            get
            {
                return Preferences.Get("Address", "");
            }
            set
            {
                Preferences.Set("Address", value);
            }
        }

        public static string FirebaseAppToken
        {
            get
            {
                return Preferences.Get("FirebaseAppToken", "");
            }
            set
            {
                Preferences.Set("FirebaseAppToken", value);
            }
        }


    }
}
