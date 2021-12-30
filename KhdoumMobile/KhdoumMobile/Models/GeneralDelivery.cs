using System;
using System.Collections.Generic;
using System.Text;

namespace KhdoumMobile.Models
{
    public class GeneralDelivery
    {
        public int Id { get; set; }
        public int? FromStateId { get; set; }
        public int? ToStateId { get; set; }
        public decimal DeliveryService { get; set; }
    }
}
