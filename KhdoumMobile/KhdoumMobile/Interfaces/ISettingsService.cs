using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KhdoumMobile.Interfaces
{
    interface ISettingsService
    {
        Task<bool> ShowDeliveryDatesState();
    }
}
