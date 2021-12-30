using KhdoumMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace KhdoumMobile.Models
{
    class CartItem:BaseViewModel
    {
        public CartItem()
        {
            AddCartItemBtnColor = "#0972ce";
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string ImgUrl { get; set; }
        public decimal QuantityDuration { get; set; }
        public string UnitName { get; set; }
        public string MarketName { get; set; }
        public string MarketId { get; set; }
        public long ProductId { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }


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
