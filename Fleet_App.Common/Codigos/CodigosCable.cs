using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fleet_App.Prism.ViewModels.Cable
{
    public static class CodigosCable
    {
        public static int?[] CodigosOcultar = new int?[]
        {
            2, 
            3, 
            5,
            7,
            8, 
            9, 
            11, 
            13, 
            14, 
            30
        };

        public static string[] EstadosOcultar = {"EJB"};

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
                case 1:
                    return "Cliente Ausente";
                case 2:
                    return "Entregará / Entregó a Técnico";
                case 3:
                    return "No lo quiere devolver";
                case 4:
                    return "Los dejó en el domicilio anterior";
                case 5:
                    return "Robo / Extravio";
                case 6:
                    return "Pospone devolución por Coronavirus";
                case 7:
                    return "Cliente desconoce cita";
                case 8:
                    return "Nunca tuvo equipo";
                case 9:
                    return "Desconoce Baja";
                case 10:
                    return "No atiende teléfono / Corta llamada";
                case 11:
                    return "Zona Intransitable";
                case 12:
                    return "Sin Visitar";
                case 13:
                    return "Mal dirección / Datos";
                case 14:
                    return "Fuera de zona";
                case 15:
                    return "Acepta retiro";
                case 30:
                    return "Recuperado";
                case 0:
                    return "Abierto";
                default:
                    return "Código de cierre incorrecto";
            }
        }
    }
}
