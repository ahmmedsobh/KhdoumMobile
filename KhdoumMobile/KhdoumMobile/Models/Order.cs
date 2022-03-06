using KhdoumMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace KhdoumMobile.Models
{
    public class Order:BaseViewModel
    {
        public long ID { get; set; }
        public int Index { get; set; }

        string customerName;
        public string CustomerName
        {
            get => customerName;
            set
            {
                SetProperty(ref customerName,value);
            } 

        }

        DateTime date;
        public DateTime Date 
        {
            get => date;
            set
            {
                SetProperty(ref date, value);
            }
        }
        decimal total;
        public decimal Total 
        {
            get => total;
            set
            {
                SetProperty(ref total , value);
            }
        }

        decimal totalAmount;
        public decimal TotalAmount
        {
            get => totalAmount;
            set
            {
                SetProperty(ref totalAmount, value);
            }
        }

        string address;
        public string Address 
        {
            get => address;
            set
            {
                SetProperty(ref address,value);
            }
        }

        string notes;
        public string Notes 
        {
            get => notes;
            set
            {
                SetProperty(ref notes, value);
            }
        }

        string phone;
        public string Phone 
        {
            get => phone;
            set
            {
                SetProperty(ref phone,value);
            }
        }

        decimal deliveryService;
        public decimal DeliveryService 
        {
            get => deliveryService;
            set
            {
                SetProperty(ref deliveryService,value);
            }
        }

        public bool IsActive { get; set; }
        public int DeliveryData { get; set; }
        public int Status { get; set; }
        public int CityId { get; set; }
        public int? StateId { get; set; } = 1;
        public int? ToStateId { get; set; } = 1;
        public string UserId { get; set; }
        string statusTitle;
        public string StatusTitle 
        {
            get => statusTitle;
            set
            {
                SetProperty(ref statusTitle, value);
            }
        }
        public string StatusIcon { get; set; }
        string statusColor;
        public string StatusColor 
        {
            get => statusColor;
            set
            {
                SetProperty(ref statusColor,value);
            }
        }

        string statusIconTitle;
        public string StatusIconTitle
        {
            get => statusIconTitle;
            set
            {
                SetProperty(ref statusIconTitle, value);
            }
        }
    }
}
