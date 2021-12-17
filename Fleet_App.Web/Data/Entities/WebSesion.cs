using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fleet_App.Web.Data.Entities
{
    public class WebSesion
    {
        [Key]
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
