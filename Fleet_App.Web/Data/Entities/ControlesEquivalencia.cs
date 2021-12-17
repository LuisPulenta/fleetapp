using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fleet_App.Web.Data.Entities
{
    public class ControlesEquivalencia
    {
        public int ID { get; set; }
        [Key]
        public string DECO1 { get; set; }
        public string CODIGOEQUIVALENCIA { get; set; }
        public string DESCRIPCION { get; set; }
        public ICollection<AsignacionesOT> AsignacionesOTs { get; set; }
    }
}
