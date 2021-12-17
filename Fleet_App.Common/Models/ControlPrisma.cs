using System;
using System.Collections.Generic;
using System.Text;

namespace Fleet_App.Common.Models
{
    public class ControlPrisma
    {
        public int IDREGISTRO { get; set; }
        public string RECUPIDJOBCARD { get; set; }
        public int? ReclamoTecnicoID { get; set; }
        public string IDSuscripcion { get; set; }
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
        public virtual Reclamo Reclamo { get; set; }
        public string DECO1 { get; set; }
        public string CMODEM1 { get; set; }
        public string ESTADO { get; set; }
        public string ESTADO2 { get; set; }
        public string ESTADO3 { get; set; }
        public string ZONA { get; set; }
        public int? HsCumplida { get; set; }
        public string Observacion { get; set; }
        public string MODELO { get; set; }
        public string MarcaModeloId { get; set; }
        public string Motivos { get; set; }
        public DateTime? FechaCita { get; set; }
        public string MedioCita { get; set; }
        public int? Elegir { get; set; }
        public bool Activo { get; set; }
        public bool Habilita { get; set; }
        public DateTime? FechaEvento1 { get; set; }
        public DateTime? FechaEvento2 { get; set; }
        public DateTime? FechaEvento3 { get; set; }
        public DateTime? FechaEvento4 { get; set; }
        public string Evento1 { get; set; }
        public string Evento2 { get; set; }
        public string Evento3 { get; set; }
        public string Evento4 { get; set; }
        public string TelefAlternativo1 { get; set; }
        public string TelefAlternativo2 { get; set; }
        public string TelefAlternativo3 { get; set; }
        public string TelefAlternativo4 { get; set; }


        public ControlesEquivalencia ControlesEquivalencia { get; set; }
        //public PrismasEquivalencia TasasEquivalencias { get; set; }
        //public int NoMostrarAPP { get; set; }
    }
}
