namespace Fleet_App.Common.Models
{
    public class Devolucion
    {
        public string PROYECTOMODULO { get; set; }

        public int? Cantidad { get; set; }

        public string Image => (Cantidad < 100)
        ? "alertaverde"
        : ((Cantidad >= 100) && (Cantidad < 200))
        ? "alertaamarillo"
        :"alertarojo";

    }
}