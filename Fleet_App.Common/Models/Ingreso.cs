using System;
using System.Collections.Generic;
using System.Text;

namespace Fleet_App.Common.Models
{
    public class Ingreso
    {
        public int ID_WS { get; set; }
        public int NROCONEXION { get; set; }
        public string USUARIO { get; set; }
        public string IP { get; set; }
        public DateTime LOGINDATE { get; set; }
        public int? LOGINTIME { get; set; }
        public string MODULO { get; set; }
        public DateTime? LOGOUTDATE { get; set; }
        public int? LOGOUTTIME { get; set; }
        public int? CONECTAVERAGE { get; set; }
    }
}