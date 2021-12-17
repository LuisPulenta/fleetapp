using System;
using System.Linq;

namespace Fleet_App.Common.Codigos
{
    public static class CodigosTlc
    {
        public static int?[] CodigosOcultar = new int?[]
        {
            25
        };

        public static string[] EstadosOcultar = { "EJB" };

        public static bool OcultarCodigo(int? cr, string estadoGaos)
        {
            if (estadoGaos == "EJB") return true;

            return CodigosOcultar.Contains(cr) && estadoGaos == "INC";
        }

        public static string GetCodigoDescription(int? cr)
        {
            return cr != null ? GetCodigoDescription(cr.Value) : String.Empty;
        }

        public static string GetCodigoDescription(int cr)
        {
            switch (cr)
            {
                case 0: return "PENDIENTE";
                case 1: return "CLIENTE SE MUDO";
                case 4: return "ABONADO CONTINUA CON SERVICIO";
                case 5: return "ABONADO YA ENTREGÓ EL EQUIPO";
                case 7: return "ABONADO SE NIEGA A ENTREGAR";
                case 9: return "ABONADO FALLECIÓ";
                case 10: return "NO TIENE EL EQUIPO";
                case 11: return "LE ROBARON EL EQUIPO";
                case 12: return "NO SE LOCALIZÓ ABONADO";
                case 13: return "DOMICILIO INEXISTENTE";
                case 18: return "OTROS MOTIVOS";
                case 19: return "ENTREGA OTRO EQUIPO DIFERENTE";
                case 25: return "ENTREGADO";
                case 26: return "INGRESADO";
                case 27: return "FINALIZADO";
                case 28: return "FUERA DE CARTERA";
                default:
                    return "Código de cierre incorrecto";
            }
        }
    }
}
