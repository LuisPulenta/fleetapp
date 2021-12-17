using Fleet_App.Common.Models;
using Fleet_App.Web.Data;
using Fleet_App.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fleet_App.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebSesionsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public WebSesionsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        public async Task<IActionResult> PostWebSesion([FromBody] WebSesion request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dataContext.WebSesions.Add(request);
            await _dataContext.SaveChangesAsync();

            var newWebSesion = await _dataContext.WebSesions
                .Where(a => a.NROCONEXION == request.NROCONEXION)
                .ToListAsync();

            var response = new List<WebSesionRequest>(newWebSesion.Select(o => new WebSesionRequest
            {
                CONECTAVERAGE=o.CONECTAVERAGE,
                ID_WS=o.ID_WS,
                IP=o.IP,
                LOGINDATE=o.LOGINDATE,
                LOGINTIME=o.LOGINTIME,
                LOGOUTDATE=o.LOGOUTDATE,
                LOGOUTTIME=o.LOGOUTTIME,
                MODULO =o.MODULO,
                NROCONEXION=o.NROCONEXION,
                USUARIO=o.USUARIO,

            }).ToList());


            return Ok(response);
        }

        [HttpGet("GetLastWebSesion/{id}")]
        public async Task<IActionResult> GetLastWebSesion([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var webSesion = await _dataContext.WebSesions
                .FirstOrDefaultAsync(o => o.NROCONEXION == id);

            if (webSesion == null)
            {
                return NotFound();
            }

            var response = new WebSesionRequest
            {
                CONECTAVERAGE=webSesion.CONECTAVERAGE,
                ID_WS = webSesion.ID_WS,
                IP = webSesion.IP,
                LOGINDATE = webSesion.LOGINDATE,
                LOGINTIME = webSesion.LOGINTIME,
                LOGOUTDATE = webSesion.LOGOUTDATE,
                LOGOUTTIME = webSesion.LOGOUTTIME,
                MODULO = webSesion.MODULO,
                NROCONEXION = webSesion.NROCONEXION,
                USUARIO = webSesion.USUARIO
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutWebSesion([FromRoute] int id, [FromBody] WebSesionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.NROCONEXION)
            {
                return BadRequest();
            }

            //var oldWebSesion = await _dataContext.WebSesions.FindAsync(request.NROCONEXION);
                

            //if (oldWebSesion == null)
            //{
                //return BadRequest("WebSesion no existe.");
            //}

            var newWebSesion = new WebSesion
            {
                ID_WS=request.ID_WS,
                NROCONEXION=request.NROCONEXION,
                CONECTAVERAGE = request.CONECTAVERAGE,
                IP = request.IP,
                LOGINDATE = request.LOGINDATE,
                LOGINTIME = request.LOGINTIME,
                LOGOUTDATE = DateTime.Now,
                LOGOUTTIME = Convert.ToInt32(DateTime.Now.ToString("hhmmss")),
                MODULO = request.MODULO,
                USUARIO = request.USUARIO,
            };
            _dataContext.WebSesions.Update(newWebSesion);
            await _dataContext.SaveChangesAsync();
            return Ok(true);
        }


    }
}