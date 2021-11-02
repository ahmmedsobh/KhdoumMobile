using System;
using System.Collections.Generic;
using System.Text;

namespace KhdoumMobile.Models
{
    class Order
    {
        public long ID { get; set; }
        public string CustomerName { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string Phone { get; set; }
        public decimal DeliveryService { get; set; }
        public bool IsActive { get; set; }
        public int DeliveryData { get; set; }
        public int Status { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string UserId { get; set; }
    }
}
