using Fleet_App.Common.Models;
using Fleet_App.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Fleet_App.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlPrismasController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ControlPrismasController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public static int?[] CodigosOcultar = new int?[]
     {
            1,
            2,
            3,
            4,
            5,
            7,
            8,
            9,
            10
     };

        public static bool OcultarCodigo(int? cr, string estadoGaos)
        {
            if (estadoGaos == "EJB") return true;

            return CodigosOcultar.Contains(cr) && estadoGaos == "INC";
        }



        [HttpPost]
        [Route("GetAutonumericos")]
        public async Task<IActionResult> GetPrismas(ControlPrismaRequest controlPrismaRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var controlprismas = await _dataContext.AsignacionesOTs
                   .Where(o => (o.ReclamoTecnicoID == controlPrismaRequest.ReclamoTecnicoID 
                   //&& ((o.CodigoCierre <= 50 && o.CodigoCierre > 40) 
                   //|| o.CodigoCierre < 20) 
                   && o.UserID == controlPrismaRequest.UserID
                   //&& o.CodigoCierre != 26
                   //&& o.CodigoCierre != 27
                   && !CodigosOcultar.Contains(o.CodigoCierre)
                   && !OcultarCodigo(o.CodigoCierre, o.ESTADOGAOS)
                   ))

                   //&& o.NoMostrarAPP == 0))
                   .OrderBy(o => o.ReclamoTecnicoID).ToListAsync();


            var response = new List<ControlPrisma>();
            foreach (var control in controlprismas)
            {
                var controlResponse = new ControlPrisma
                {
                    Autonumerico = control.Autonumerico,
                    CMODEM1 = control.CMODEM1,
                    CodigoCierre = control.CodigoCierre,
                    DECO1 = control.DECO1,
                    ESTADO = control.ESTADO,
                    ESTADO2 = control.ESTADO2,
                    ESTADO3 = control.ESTADO3,
                    ESTADOGAOS = control.ESTADOGAOS,
                    FECHACUMPLIDA = control.FECHACUMPLIDA,
                    HsCumplida = control.HsCumplida,
                    HsCumplidaTime = control.HsCumplidaTime,
                    IDREGISTRO = control.IDREGISTRO,
                    Observacion = control.Observacion,
                    PROYECTOMODULO = control.PROYECTOMODULO,
                    ReclamoTecnicoID = control.ReclamoTecnicoID,
                    RECUPIDJOBCARD = control.RECUPIDJOBCARD,
                    IDSuscripcion = control.IDSuscripcion,
                    MarcaModeloId = control.MarcaModeloId,
                    MODELO = control.MODELO,
                    Motivos = control.Motivos,
                    ZONA = control.ZONA,
                    TelefAlternativo1 = control.TelefAlternativo1,
                    TelefAlternativo2 = control.TelefAlternativo2,
                    TelefAlternativo3 = control.TelefAlternativo3,
                    TelefAlternativo4 = control.TelefAlternativo4,
                  
                };
                response.Add(controlResponse);
            }

            return Ok(response);
        }
    }












}