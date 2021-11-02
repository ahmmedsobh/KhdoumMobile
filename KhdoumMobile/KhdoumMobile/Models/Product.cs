using KhdoumMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace KhdoumMobile.Models
{
    public class Product:BaseViewModel
    {
        public Product()
        {
            AddCartItemBtnColor = "#0972ce";
        }
        public long ID { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public long CategoryId { get; set; }
        public int UnitId { get; set; }
        public string CategoryName { get; set; }
        public string UnitName { get; set; }
        public decimal QuantityDuration { get; set; }
        public string PriceLable { get; set; }
        public string MarketId { get; set; }
        public string MarketName { get; set; }
        public long ProductId { get; set; }


        decimal counterValue;
        public decimal CounterValue
        {
            get => counterValue;
            set
            {
                SetProperty(ref counterValue, value);
            }
        }

        string addCartItemBtnColor;
        public string AddCartItemBtnColor
        {
            get => addCartItemBtnColor;
            set
            {
                SetProperty(ref addCartItemBtnColor, value);
            }
        }
    }
}
