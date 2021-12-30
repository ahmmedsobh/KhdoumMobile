using System;
using System.Collections.Generic;
using System.Text;

namespace KhdoumMobile.Models
{
    class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAndTime { get; set; }
        //public string UserId { get; set; }
        //public int NotificationId { get; set; }
    }
}
