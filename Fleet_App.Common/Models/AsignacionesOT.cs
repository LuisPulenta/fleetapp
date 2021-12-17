using System;
using System.Collections.Generic;
using System.Text;

namespace Fleet_App.Common.Models
{
    public class AsignacionesOT
    {
        public int IDREGISTRO { get; set; }
        public string SUBAGENTEMERCADO { get; set; }
        public string RECUPIDJOBCARD { get; set; }
        public string CLIENTE { get; set; }
        public string NOMBRE { get; set; }
        public string DOMICILIO { get; set; }
        public string ENTRECALLE1 { get; set; }
        public string ENTRECALLE2 { get; set; }

        public int? CantRem { get; set; }

        public byte[] ImageArrayDni { get; set; }
        public byte[] ImageArrayFirma { get; set; }
        public string CP { get; set; }
        public string ZTECNICO { get; set; }
        public string PROVINCIA { get; set; }
        public string LOCALIDAD { get; set; }
        public string TELEFONO { get; set; }
        public string GRXX { get; set; }
        public string GRYY { get; set; }
        public string DECO1 { get; set; }
        public string CMODEM1 { get; set; }
        public DateTime? FECHACARGA { get; set; }
        public string ESTADO { get; set; }
        public DateTime? FECHAENT { get; set; }
        public string TECASIG { get; set; }
        public string ZONA { get; set; }
        public string IDR { get; set; }
        public string MODELO { get; set; }
        public string SMARTCARD { get; set; }
        public string RUTA { get; set; }
        public string ESTADO2 { get; set; }
        public string ESTADO3 { get; set; }
        public string TARIFA { get; set; }
        public string PROYECTOMODULO { get; set; }
        public DateTime? FECHACAPTURA { get; set; }
        public string ESTADOGAOS { get; set; }
        public DateTime? FECHACUMPLIDA { get; set; }
        public int? BAJASISTEMA { get; set; }
        public int? IDCABECERACERTIF { get; set; }
        public string SUBCON { get; set; }
        public string CAUSANTEC { get; set; }
        public int? PasaDefinitiva { get; set; }
        public DateTime? FechaAsignada { get; set; }
        public DateTime? FechaInicio { get; set; }
        public int? HsCaptura { get; set; }
        public int? HsAsignada { get; set; }
        public int? HsCumplida { get; set; }
        public string Observacion { get; set; }
        public string LinkFoto { get; set; }
        public int? UserID { get; set; }
        public DateTime? HsCumplidaTime { get; set; }
        public string TerminalAsigna { get; set; }
        public string UrlDni { get; set; }
        public string UrlFirma { get; set; }
        public string UrlDni2 { get; set; }
        public string UrlFirma2 { get; set; }
        public int? EsCR { get; set; }
        public int? Autonumerico { get; set; }
        public int? ReclamoTecnicoID { get; set; }
        public int? ClienteTipoId { get; set; }
        public string Documento { get; set; }
        public string Partido { get; set; }
        public string EmailCliente { get; set; }
        public string ObservacionCaptura { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string MarcaModeloId { get; set; }
        public string Enviado { get; set; }
        public int? Cancelado { get; set; }
        public int? Recupero { get; set; }
        public int? CodigoCierre { get; set; }
        public int? VisitaTecnica { get; set; }
        public string Novedades { get; set; }
        public int? PDFGenerado { get; set; }
        public DateTime? FechaCumplidaTecnico { get; set; }
        public int? ArchivoOutGenerado { get; set; }
        public string IDSuscripcion { get; set; }
        public string ItemsID { get; set; }
        public string SectorOperativo { get; set; }
        public string IdTipoTrabajoRel { get; set; }
        public string Motivos { get; set; }
        public ControlesEquivalencia ControlesEquivalencia { get; set; }
        
        public DateTime? FechaCita { get; set; }
        public string MedioCita { get; set; }
        public string NroSeriesExtras { get; set; }
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

        //public int? NoMostrarAPP { get; set; }

        public string ClienteCompleto => $"{CLIENTE}-{NOMBRE}";
        public string EntreCalles => $"{ENTRECALLE1} y {ENTRECALLE2}";
    }
}
