using Fleet_App.Common.Models;
using Fleet_App.Web.Data;
using Fleet_App.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fleet_App.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsignacionesOtsEquiposExtrasController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AsignacionesOtsEquiposExtrasController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsignacionesOtsEquiposExtra([FromBody] AsignacionesOtsEquiposExtra request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dataContext.AsignacionesOtsEquiposExtras.Add(request);
            await _dataContext.SaveChangesAsync();

            var newAsignacionesOtsEquiposExtra = await _dataContext
                .AsignacionesOtsEquiposExtras
                .Where(a => 
                    a.NROCLIENTE == request.NROCLIENTE 
                    && a.NROSERIEEXTRA == request.NROSERIEEXTRA)
                .ToListAsync();

            var response = new List<AsignacionesOtsEquiposExtraRequest>(newAsignacionesOtsEquiposExtra.Select(o => new AsignacionesOtsEquiposExtraRequest
            {
                CODDECO1=o.CODDECO1,
                FECHACARGA=o.FECHACARGA,
                IDASIGNACIONEXTRA = o.IDASIGNACIONEXTRA,
                IDGAOS=o.IDGAOS,
                IDTECNICO = o.IDTECNICO,
                NROCLIENTE=o.NROCLIENTE,
                NROSERIEEXTRA = o.NROSERIEEXTRA,
                ProyectoModulo = o.ProyectoModulo
            }).ToList());


            return Ok(response);
        }
    }
}