using System;
using System.Linq;

namespace Fleet_App.Common.Codigos
{
    public static class CodigosDtv
    {
        public static int?[] CodigosOcultar = new int?[]
        {
            42,
            46,
            48,
            49,
            50,
            51,
            70
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
                case 0: return "No Contactado";
                case 41: return "CLIENTE AUSENTE";
                case 42: return "CLIENTE SE MUDO";
                case 43: return "NO ATIENDE EL TELEFONO";
                case 44: return "REFERENCIA INCORRECTA";
                case 45: return "VISITA COORDINADA";
                case 46: return "CLIENTE FALLECIO";
                case 47: return "CLIENTE NO ACEPTA RETIRO";
                case 48: return "CLIENTE NO POSEE LOS EQUIPOS";
                case 49: return "CLIENTE YA ENTREGO LOS EQUIPOS";
                case 50: return "CERRADA POR EL CLIENTE";
                case 51: return "CLIENTE CONTINUA CON EL SERVICIO";
                case 70: return "RECUPERADO";
                default:
                    return "Código de cierre incorrecto";
            }
        }
    }
}
