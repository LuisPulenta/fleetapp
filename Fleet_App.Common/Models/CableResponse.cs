using System;
using System.Collections.Generic;
using System.Text;

namespace Fleet_App.Common.Models
{
    public class CableResponse
    {
        public string RECUPIDJOBCARD { get; set; }
        public string CLIENTE { get; set; }
        public string NOMBRE { get; set; }
        public string DOMICILIO { get; set; }
        public string CP { get; set; }
        public string ENTRECALLE1 { get; set; }
        public string ENTRECALLE2 { get; set; }
        public string LOCALIDAD { get; set; }
        public string TELEFONO { get; set; }
        public string GRXX { get; set; }
        public string GRYY { get; set; }
        public string ESTADOGAOS { get; set; }
        public string PROYECTOMODULO { get; set; }
        public int? UserID { get; set; }
        public string CAUSANTEC { get; set; }
        public string SUBCON { get; set; }
        public DateTime? FechaAsignada { get; set; }
        public int? CodigoCierre { get; set; }
        public string ObservacionCaptura { get; set; }
        public string Novedades { get; set; }
        public string DESCRIPCION { get; set; }
        public int CantRem { get; set; }
        public string ClienteCompleto => $"{CLIENTE}-{NOMBRE}";
        public string EntreCalles => $"{ENTRECALLE1} y {ENTRECALLE2}";
        public string TelefAlternativo1 { get; set; }

        public string TelefAlternativo2 { get; set; }
        public string TelefAlternativo3 { get; set; }
        public string TelefAlternativo4 { get; set; }


    }
}
