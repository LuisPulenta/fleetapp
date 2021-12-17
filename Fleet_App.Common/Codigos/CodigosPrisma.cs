using System;
using System.Linq;

namespace Fleet_App.Common.Codigos
{
    public static class CodigosPrisma
    {
        public static int?[] CodigosOcultar = new int?[]
        {
            1, 2, 3, 4, 5, 7, 8, 9, 10
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
                case 1: return "A PEDIDO DE PRISMA";
                case 2: return "COMERCIO CERRADO";
                case 3: return "DATOS INCORRECTOS";
                case 4: return "DESEA MANTENER LA TERMINAL";
                case 6: return "GESTION DE OPERACIONES";
                case 7: return "PERDIDA";
                case 8: return "ROBADA";
                case 9: return "MAL ASIGNADA";
                case 10: return "RECUPERADO";
                default:
                    return "Código de cierre incorrecto";
            }
        }
    }
}
