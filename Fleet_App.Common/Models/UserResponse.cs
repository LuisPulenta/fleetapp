using System;
using System.Collections.Generic;
using System.Text;

namespace Fleet_App.Common.Models
{
    public class UserResponse
    {
        public int IDUser { get; set; }
        public string CODIGO { get; set; }
        public string APELLIDONOMBRE { get; set; }
        public string USRLOGIN { get; set; }
        public string USRCONTRASENA { get; set; }
        public int HabilitadoWeb { get; set; }
        public string CausanteC { get; set; }
    }
}
