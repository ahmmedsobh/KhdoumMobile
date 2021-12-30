using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace KhdoumMobile.CustomControls
{
    class MyFlyoutItem:FlyoutItem
    {
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == "CurrentItem")
            {
                int index = this.Items.IndexOf(this.CurrentItem);

                if (index == 0)
                {

                }
                if (index == 1)
                {

                }

            }
        }
    }
}
