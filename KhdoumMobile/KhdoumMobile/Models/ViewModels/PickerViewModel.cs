using System;
using System.Collections.Generic;
using System.Text;

namespace KhdoumMobile.Models.ViewModels
{
    class PickerViewModel<T>
    {
        public T Value { get; set; }
        public string Name { get; set; }
        public Object Value2 { get; set; }
    }
}
