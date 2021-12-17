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
    public class AsignacionesPrismaController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AsignacionesPrismaController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        [Route("GetPrismas/{UserID}")]
        public async Task<IActionResult> GetPrismas(int UserID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var codigosMostrar = new List<int?>() { 1, 2, 3, 4, 6, 7, 8, 9, 10 };

            var orders = await _dataContext.AsignacionesOTs
                .Include(m => m.ControlesEquivalencia)
           .Where(o => 
                    (o.UserID == UserID) 
                    && (o.PROYECTOMODULO == "Prisma") 
                    && ((o.ESTADOGAOS == "PEN") || (o.ESTADOGAOS == "INC") && !CodigosPrisma.OcultarCodigo(o.CodigoCierre, o.ESTADOGAOS)))
           .OrderBy(o => o.RECUPIDJOBCARD)
           .GroupBy(r => new
           {
               r.RECUPIDJOBCARD,
               r.CLIENTE,
               r.NOMBRE,
               r.DOMICILIO,
               r.CP,
               r.ENTRECALLE1,
               r.ENTRECALLE2,
               r.LOCALIDAD,
               r.TELEFONO,
               r.GRXX,
               r.GRYY,
               r.ESTADOGAOS,
               r.PROYECTOMODULO,
               r.UserID,
               r.CAUSANTEC,
               r.SUBCON,
               r.FechaAsignada,
               r.CodigoCierre,
               r.ObservacionCaptura,
               r.Novedades,
               r.PROVINCIA,
               r.ReclamoTecnicoID,
               r.Motivos,
               r.FechaCita,
               r.MedioCita,
               r.NroSeriesExtras,
               r.Evento1,
               r.FechaEvento1,
               r.Evento2,
               r.FechaEvento2,
               r.Evento3,
               r.FechaEvento3,
               r.Evento4,
               r.FechaEvento4,
               r.TelefAlternativo1,
               r.TelefAlternativo2,
               r.TelefAlternativo3,
               r.TelefAlternativo4

           })
           .Select(g => new
           {
               RECUPIDJOBCARD = g.Key.RECUPIDJOBCARD,
               CLIENTE = g.Key.CLIENTE,
               NOMBRE = g.Key.NOMBRE,
               DOMICILIO = g.Key.DOMICILIO,
               CP = g.Key.CP,
               ENTRECALLE1 = g.Key.ENTRECALLE1,
               ENTRECALLE2 = g.Key.ENTRECALLE2,
               LOCALIDAD = g.Key.LOCALIDAD,
               TELEFONO = g.Key.TELEFONO,
               GRXX = g.Key.GRXX,
               GRYY = g.Key.GRYY,
               ESTADOGAOS = g.Key.ESTADOGAOS,
               PROYECTOMODULO = g.Key.PROYECTOMODULO,
               UserID = g.Key.UserID,
               CAUSANTEC = g.Key.CAUSANTEC,
               SUBCON = g.Key.SUBCON,
               FechaAsignada = g.Key.FechaAsignada,
               CodigoCierre = g.Key.CodigoCierre,
               ObservacionCaptura = g.Key.ObservacionCaptura,
               Novedades = g.Key.Novedades,
               PROVINCIA = g.Key.PROVINCIA,
               ReclamoTecnicoID = g.Key.ReclamoTecnicoID,
               Motivos = g.Key.Motivos,
               FechaCita = g.Key.FechaCita,
               MedioCita = g.Key.MedioCita,
               NroSeriesExtras = g.Key.NroSeriesExtras,
               Evento1 = g.Key.Evento1,
               FechaEvento1 = g.Key.FechaEvento1,
               Evento2 = g.Key.Evento2,
               FechaEvento2 = g.Key.FechaEvento2,
               Evento3 = g.Key.Evento3,
               FechaEvento3 = g.Key.FechaEvento3,
               Evento4 = g.Key.Evento4,
               FechaEvento4 = g.Key.FechaEvento4,
               TelefAlternativo1 = g.Key.TelefAlternativo1,
               TelefAlternativo2 = g.Key.TelefAlternativo2,
               TelefAlternativo3 = g.Key.TelefAlternativo3,
               TelefAlternativo4 = g.Key.TelefAlternativo4,

               CantRem = g.Count(),
           }).ToListAsync();


            if (orders == null)
            {
                return BadRequest("No hay Recuperos de Prisma para este Usuario.");
            }

            return Ok(orders);
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
                   .Where(o =>
                       (o.ReclamoTecnicoID == controlPrismaRequest.ReclamoTecnicoID
                        && !CodigosPrisma.OcultarCodigo(o.CodigoCierre, o.ESTADOGAOS)
                        && o.UserID == controlPrismaRequest.UserID))
                   .OrderBy(o => o.ReclamoTecnicoID)
                   .ToListAsync();


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
                    FechaCita = control.FechaCita,
                    MedioCita = control.MedioCita,
                    //NroSeriesExtras = control.NroSeriesExtras,
                    Evento1 = control.Evento1,
                    FechaEvento1 = control.FechaEvento1,
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
