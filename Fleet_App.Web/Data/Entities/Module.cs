using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fleet_App.Web.Data.Entities
{
    public class Module
    {
        [Key]
        public int IdModulo { get; set; }
        public string NOMBRE { get; set; }
        public string NroVersion { get; set; }
        public string Link { get; set; }
        public DateTime FechaRelease { get; set; }
        public int ActualizOblig { get; set; }
    }
}
