using System;
using System.ComponentModel.DataAnnotations;

namespace Fleet_App.Web.Data.Entities
{
    public class AsignacionesOtsEquiposExtra
    {
        [Key]
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
