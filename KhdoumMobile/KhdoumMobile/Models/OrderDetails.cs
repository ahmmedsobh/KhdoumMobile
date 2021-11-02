using System;
using System.Collections.Generic;
using System.Text;

namespace KhdoumMobile.Models
{
    class OrderDetails
    {
        public long ID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Value { get; set; }
        public long ProductId { get; set; }
        public long OrderId { get; set; }
    }
}
