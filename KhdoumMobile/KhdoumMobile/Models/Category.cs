using System;
using System.Collections.Generic;
using System.Text;

namespace KhdoumMobile.Models
{
    class Category:List<Item>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string IconColor { get; set; }
        public Category(int id, string name, string icon, string iconColor, List<Item> items) : base(items)
        {
            Id = id;
            Name = name;
            Icon = icon;
            IconColor = iconColor;
        }
    }
}
