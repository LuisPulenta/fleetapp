using System;
using System.Linq;

namespace Fleet_App.Common.Codigos
{
    public static class CodigosRemotes
    {
        public static int?[] CodigosOcultar = new int?[]
        {
            13,
            14,
            15,
            16,
            17,
            18,
            19,
            20,
            21,
            22,
            23,
            24,
            25,
            26,
            30,

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
                    return "Sin respuesta: Llamado telefónico";
                case 2:
                    return "Teléfono erroneo: Llamado telefónico";
                case 3:
                    return "Se coordinó visita: Llamado telefónico";
                case 4:
                    return "Sin respuesta: Envío de correo";
                case 5:
                    return "Casilla inválida: Envío de correo";
                case 6:
                    return "Se coordinó visita: Envío de correo";
                case 7:
                    return "Sin respuesta: SMS";
                case 8:
                    return "Teléfono erroneo: SMS";
                case 9:
                    return "Se coordinó visita: SMS";
                case 10:
                    return "Ausente: Visita en domicilio";
                case 11:
                    return "Menor en domicilio: Visita en domicilio";
                case 12:
                    return "Sin stock de control: Visita en domicilio";
                case 13:
                    return "Entregado";
              
                case 15:
                    return "Se desestima pedido: Visita en domicilio";
                case 16:
                    return "En gestión: Visita en domicilio";
                case 17:
                    return "Entrega rechazada / No acepta costos";
                case 18:
                    return "Entrega rechazada / Dio de baja el servicio";
                case 19:
                    return "Entrega rechazada / No lo requiere";
                case 20:
                    return "Entrega rechazada / Nunca lo solicitó";
                case 21:
                    return "Entrega rechazada / Gestión duplicada";
                case 22:
                    return "Entrega rechazada / Reclamo mal cargado";
                case 23:
                    return "Entrega rechazada / Sin contacto";
                case 24:
                    return "Entrega rechazada / Dirección Incorrecta";
                case 25:
                    return "Entrega rechazada / Error barrios";
                case 26:
                    return "Entrega rechazada / Error de Geolocalización";
                case 30:
                    return "Finalizado";
                case 0:
                    return "Abierto";
                default:
                    return "Código de cierre incorrecto";
            }
        }
    }
}
