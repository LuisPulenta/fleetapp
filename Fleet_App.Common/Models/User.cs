using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fleet_App.Common.Models
{
    public class User
    {
        public int ID { get; set; }
        public string APELLIDO { get; set; }
        public string NOMBRE { get; set; }
    }
}
