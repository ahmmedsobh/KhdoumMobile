using System;
using System.Collections.Generic;
using System.Text;

namespace KhdoumMobile.Models
{
    public class SupCategory
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public long ParentId { get; set; }
        public bool LevelStatus { get; set; }
        public string ImgUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
