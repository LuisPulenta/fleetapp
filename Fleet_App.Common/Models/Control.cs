using System;
using System.Collections.Generic;
using System.Text;

namespace Fleet_App.Common.Models
{
    public class Control
    {
        public int IDREGISTRO { get; set; }
        public string RECUPIDJOBCARD { get; set; }
        public string ESTADOGAOS { get; set; }
        public string PROYECTOMODULO { get; set; }
        public DateTime? FECHACUMPLIDA { get; set; }
        public DateTime? HsCumplidaTime { get; set; }
        public int? CodigoCierre { get; set; }
        public int? Autonumerico { get; set; }
        public string UrlDni { get; set; }
        public string UrlFirma { get; set; }
        public byte[] ImageArrayDni { get; set; }
        public byte[] ImageArrayFirma { get; set; }
        public string UrlDniFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(UrlDni))
                {
                    return "nouser";
                }
                return $"http://fleetsa.serveftp.net:88/FleetApiNew/{this.UrlDni.Substring(1)}";
                //return $"http://keypress.serveftp.net:88/FleetApiPrueba/{this.UrlDni.Substring(1)}";
                //return $"http://keypress.serveftp.net:88/FleetApiNew/{this.UrlDni.Substring(1)}";
                //return $"http://190.221.174.70/Fleetsa/{this.UrlDni.Substring(1)}";
            }
        }
        public string UrlFirmaFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(UrlFirma))
                {
                    return "nouser";
                }
                return $"http://fleetsa.serveftp.net:88/FleetApiNew/{this.UrlDni.Substring(1)}";
                //return $"http://keypress.serveftp.net:88/FleetApiPrueba/{this.UrlFirma.Substring(1)}";
                //return $"http://keypress.serveftp.net:88/FleetApiNew/{this.UrlFirma.Substring(1)}";
                //return $"http://190.221.174.70/Fleetsa/{this.UrlFirma.Substring(1)}";
            }
        }
        public string DECO1 { get; set; }
        public string CMODEM1 { get; set; }
        public string ESTADO { get; set; }
        public string ZONA { get; set; }
        public int? HsCumplida { get; set; }
        public string Observacion { get; set; }
        public int? ReclamoTecnicoID { get; set; }

        public DateTime? FechaCita { get; set; }
        public string MedioCita { get; set; }
        
        public DateTime? FechaEvento1 { get; set; }
        public DateTime? FechaEvento2 { get; set; }
        public DateTime? FechaEvento3 { get; set; }
        public DateTime? FechaEvento4 { get; set; }
        public string Evento1 { get; set; }
        public string Evento2 { get; set; }
        public string Evento3 { get; set; }
        public string Evento4 { get; set; }

        public ControlesEquivalencia ControlesEquivalencia { get; set; }
    }
}
