using System;

namespace Fleet_App.Common.Models
{
    public class TrabajosRequest
    {
        public int? UserID { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public String Proyecto { get; set; }
    }
}