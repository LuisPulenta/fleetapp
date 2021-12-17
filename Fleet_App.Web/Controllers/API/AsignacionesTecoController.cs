using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fleet_App.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleet_App.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsignacionesTecoController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AsignacionesTecoController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        [HttpPost]
        [Route("GetTecos/{UserID}")]
        public async Task<IActionResult> GetTecos(int UserID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var codigosMostrar = new List<int?> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15 };

            var orders = await _dataContext.AsignacionesOTs
                .Include(m => m.ControlesEquivalencia)
                .Where(o => o.UserID == UserID && o.PROYECTOMODULO == "Teco")
            .Where(o => (o.ESTADOGAOS == "PEN") || (o.ESTADOGAOS == "INC") && codigosMostrar.Contains(o.CodigoCierre))
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
               r.PROVINCIA,
               r.ReclamoTecnicoID,
               r.TELEFONO,
               r.GRXX,
               r.GRYY,
               r.ESTADOGAOS,
               r.PROYECTOMODULO,
               r.UserID,
               r.CAUSANTEC,
               r.SUBCON,
               r.FechaAsignada,
               r.FechaInicio,
               r.CodigoCierre,
               r.ObservacionCaptura,
               r.Novedades,
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
               r.FechaEvento4
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
               PROVINCIA = g.Key.PROVINCIA,
               ReclamoTecnicoID = g.Key.ReclamoTecnicoID,
               TELEFONO = g.Key.TELEFONO,
               GRXX = g.Key.GRXX,
               GRYY = g.Key.GRYY,
               ESTADOGAOS = g.Key.ESTADOGAOS,
               PROYECTOMODULO = g.Key.PROYECTOMODULO,
               UserID = g.Key.UserID,
               CAUSANTEC = g.Key.CAUSANTEC,
               SUBCON = g.Key.SUBCON,
               FechaAsignada = g.Key.FechaAsignada,
               FechaInicio = g.Key.FechaInicio,
               CodigoCierre = g.Key.CodigoCierre,
               ObservacionCaptura = g.Key.ObservacionCaptura,
               Novedades = g.Key.Novedades,
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
               CantRem = g.Count(),
           }).ToListAsync();

            if (orders == null)
            {
                return BadRequest("No hay recuperos para este Usuario.");
            }

            return Ok(orders);
        }

    }
}
