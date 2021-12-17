using System;

namespace Fleet_App.Common.Models
{
    public class TrabajosResponse2
    {
        public string ProyectoModulo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaAsignada { get; set; }
        public DateTime FECHACUMPLIDA { get; set; }
        public double Cant { get; set; }
    }
}