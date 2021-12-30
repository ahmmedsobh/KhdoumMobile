using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KhdoumMobile.ViewModels
{
    class ContactUsViewModel:BaseViewModel
    {
        string message;
        public string Message {
            get => message;

            set
            {
                SetProperty(ref message,value);
            }
        }

        public ICommand SendCommand {
            get
            {
                return new Command(() => { 
                    if(Message != null)
                    {
                        Browser.OpenAsync($"https://wa.me/201064641608?text={Message}");
                    }
                });
            }
        }

    }
}
