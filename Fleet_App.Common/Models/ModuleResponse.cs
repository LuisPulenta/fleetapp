using System;
using System.Collections.Generic;
using System.Text;

namespace Fleet_App.Common.Models
{
    public class ModuleResponse
    {
        public int IdModulo { get; set; }
        public string NOMBRE { get; set; }
        public string NroVersion { get; set; }
        public string Link { get; set; }
        public DateTime FechaRelease { get; set; }
        public int ActualizOblig { get; set; }
    }
}
