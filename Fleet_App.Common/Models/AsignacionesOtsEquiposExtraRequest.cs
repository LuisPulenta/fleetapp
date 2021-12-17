using System;

namespace Fleet_App.Common.Models
{
    public class AsignacionesOtsEquiposExtraRequest
    {
        public int IDASIGNACIONEXTRA { get; set; }
        public int? IDGAOS { get; set; }
        public DateTime? FECHACARGA { get; set; }
        public string NROCLIENTE { get; set; }
        public int? IDTECNICO { get; set; }
        public string CODDECO1 { get; set; }
        public string NROSERIEEXTRA { get; set; }
        public string ProyectoModulo { get; set; }
    }
}
