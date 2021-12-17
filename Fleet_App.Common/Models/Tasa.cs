using System;
using System.Collections.Generic;
using System.Text;

namespace Fleet_App.Common.Models
{
    public class Tasa
    {
        public int IDREGISTRO { get; set; }
        public string RECUPIDJOBCARD { get; set; }
        public string CLIENTE { get; set; }
        public string NOMBRE { get; set; }
        public string DOMICILIO { get; set; }
        public string ENTRECALLE1 { get; set; }
        public string ENTRECALLE2 { get; set; }
        public string CP { get; set; }
        public string LOCALIDAD { get; set; }
        public string GRXX { get; set; }
        public string GRYY { get; set; }
        public string ESTADO { get; set; }
        public string ZONA { get; set; }
        public string ESTADOGAOS { get; set; }
        public DateTime? FECHACUMPLIDA { get; set; }
        public string SUBCON { get; set; }
        public string CAUSANTEC { get; set; }
        public DateTime? FechaAsignada { get; set; }
        public string EntreCalles { get; set; }
        public string PROYECTOMODULO { get; set; }
        public string DECO1 { get; set; }
        public string CMODEM1 { get; set; }
        public string Observacion { get; set; }
        public DateTime? HsCumplidaTime { get; set; }
        public int? UserID { get; set; }
        public int? CodigoCierre { get; set; }
        public string UrlDni { get; set; }
        public string UrlFirma { get; set; }


        public List<Picture> Pictures { get; set; }
    }
}
