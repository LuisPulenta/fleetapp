using Fleet_App.Common.Models;
using Fleet_App.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fleet_App.Common.Codigos;


namespace Fleet_App.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlCablesController : ControllerBase
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

        public static bool OcultarCodigo(int? cr, string estadoGaos)
        {
            if (estadoGaos == "EJB") return true;

            return CodigosOcultar.Contains(cr) && estadoGaos == "INC";
        }

        private readonly DataContext _dataContext;

        public ControlCablesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        [HttpPost]
        [Route("GetAutonumericos")]
        public async Task<IActionResult> GetCables(ControlCableRequest controlCableRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }



            var controlcables = await _dataContext.AsignacionesOTs
                   .Where(o => 
                       (o.ReclamoTecnicoID == controlCableRequest.ReclamoTecnicoID 
                        //&& o.CodigoCierre < 13
                        //&& !OcultarCodigo(o.CodigoCierre, o.ESTADOGAOS)
                        && !CodigosOcultar.Contains(o.CodigoCierre)
                        && !OcultarCodigo(o.CodigoCierre, o.ESTADOGAOS)
                        //&& !OcultarCodigo(o.CodigoCierre, o.ESTADOGAOS)
                        && o.UserID== controlCableRequest.UserID))
                   .OrderBy(o => o.ReclamoTecnicoID)
                   .ToListAsync();


            var response = new List<ControlCable>();

            foreach (var control in controlcables)
            {
                var controlResponse = new ControlCable
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
                    IDSuscripcion=control.IDSuscripcion,
                    MarcaModeloId=control.MarcaModeloId,
                    MODELO=control.MODELO,
                    Motivos= control.Motivos,
                    ZONA = control.ZONA,
                    FechaCita = control.FechaCita,
                    MedioCita = control.MedioCita,
                    NroSeriesExtras = control.NroSeriesExtras,
                    Evento1=control.Evento1,
                    FechaEvento1=control.FechaEvento1,
                    Evento2 = control.Evento2,
                    FechaEvento2 = control.FechaEvento2,
                    Evento3 = control.Evento3,
                    FechaEvento3 = control.FechaEvento3,
                    Evento4 = control.Evento4,
                    FechaEvento4 = control.FechaEvento4,
                    TelefAlternativo1 = control.TelefAlternativo1,
                    TelefAlternativo2 = control.TelefAlternativo2,
                    TelefAlternativo3 = control.TelefAlternativo3,
                    TelefAlternativo4 = control.TelefAlternativo4
                };
                response.Add(controlResponse);
            }

            return Ok(response);
        }
    }


   









}