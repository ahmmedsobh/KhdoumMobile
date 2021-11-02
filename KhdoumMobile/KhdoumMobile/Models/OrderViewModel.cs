using System;
using System.Collections.Generic;
using System.Text;

namespace KhdoumMobile.Models
{
    class OrderViewModel
    {
        public Order Order { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
    }
}
