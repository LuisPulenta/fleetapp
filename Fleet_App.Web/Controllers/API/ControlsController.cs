using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fleet_App.Common.Codigos;
using Fleet_App.Common.Models;
using Fleet_App.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleet_App.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlsController : ControllerBase
    {
        public static int?[] CodigosOcultar = new int?[]
     {
            2,
            3,
            5,
            7,
            8,
            9,
            10,
            11,
            13,
            14,
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
            30
     };

        public static bool OcultarCodigo(int? cr, string estadoGaos)
        {
            if (estadoGaos == "EJB") return true;

            return CodigosOcultar.Contains(cr) && estadoGaos == "INC";
        }


        private readonly DataContext _dataContext;

        public ControlsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        [HttpPost]
        [Route("GetAutonumericos")]



        public async Task<IActionResult> GetRemotes(ControlRequest controlRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var controls = await _dataContext.AsignacionesOTs
               .Include(m => m.ControlesEquivalencia)
               //.Where(o => (o.RECUPIDJOBCARD == controlRequest.RECUPIDJOBCARD && o.CodigoCierre < 13 && o.UserID == controlRequest.UserID))
               .Where(o =>
                       (o.RECUPIDJOBCARD == controlRequest.RECUPIDJOBCARD
                        //&& o.CodigoCierre < 13
                        //&& !OcultarCodigo(o.CodigoCierre, o.ESTADOGAOS)
                        //&& !CodigosOcultar.Contains(o.CodigoCierre)
                        //&& !OcultarCodigo(o.CodigoCierre, o.ESTADOGAOS)
                        //&& !OcultarCodigo(o.CodigoCierre, o.ESTADOGAOS)
                        //&& o.UserID == controlRequest.UserID))
                        && !CodigosRemotes.OcultarCodigo(o.CodigoCierre, o.ESTADOGAOS)
                        && o.UserID == controlRequest.UserID))
               .OrderBy(o => o.RECUPIDJOBCARD)
               .ToListAsync();

            var response = new List<Control>();

            foreach (var control in controls)
            {
                var controlResponse = new Control
                {
                    Autonumerico = control.Autonumerico,
                    CMODEM1 = control.CMODEM1,
                    CodigoCierre = control.CodigoCierre,
                    DECO1 = control.DECO1,
                    ESTADO = control.ESTADO,
                    ESTADOGAOS = control.ESTADOGAOS,
                    FECHACUMPLIDA = control.FECHACUMPLIDA,
                    HsCumplida = control.HsCumplida,
                    HsCumplidaTime = control.HsCumplidaTime,
                    IDREGISTRO = control.IDREGISTRO,
                    Observacion = control.Observacion,
                    PROYECTOMODULO = control.PROYECTOMODULO,
                    ReclamoTecnicoID = control.ReclamoTecnicoID,
                    RECUPIDJOBCARD = control.RECUPIDJOBCARD,
                    UrlDni = control.UrlDni,
                    UrlFirma = control.UrlFirma,
                    ZONA = control.ZONA,
                    ControlesEquivalencia = new ControlesEquivalencia() { ID = 0 }
                };

                if (control.ControlesEquivalencia != null)
                {
                    controlResponse.ControlesEquivalencia = new ControlesEquivalencia
                    {
                        CODIGOEQUIVALENCIA = control.ControlesEquivalencia.CODIGOEQUIVALENCIA,
                        DECO1 = control.ControlesEquivalencia.DECO1,
                        DESCRIPCION = control.ControlesEquivalencia.DESCRIPCION,
                        ID = control.ControlesEquivalencia.ID
                    };
                }

                response.Add(controlResponse);
            }

            return Ok(response);
        }
    }












}