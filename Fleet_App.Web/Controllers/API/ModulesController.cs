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
    public class ModulesController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ModulesController(DataContext dataContext)


        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetModules()
        {
            var modules = await _dataContext.Modules
                .ToListAsync();

            var response = new List<ModuleResponse>(modules.Select(o => new ModuleResponse
            {
                ActualizOblig=o.ActualizOblig,
                FechaRelease=o.FechaRelease,
                IdModulo=o.IdModulo,
                Link=o.Link,
                NOMBRE=o.NOMBRE,
                NroVersion=o.NroVersion
            }).ToList());

            return Ok(response);
        }

        //*********************************************************
        // GET: api/Modules/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetModule([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var @module = await _dataContext.Modules.FindAsync(id);

            if (@module == null)
            {
                return NotFound();
            }

            return Ok(@module);
        }

        // PUT: api/Modules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule([FromRoute] int id, [FromBody] Module @module)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @module.IdModulo)
            {
                return BadRequest();
            }

            _dataContext.Entry(@module).State = EntityState.Modified;

            try
            {
                await _dataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Modules
        [HttpPost]
        public async Task<IActionResult> PostModule([FromBody] Module @module)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dataContext.Modules.Add(@module);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction("GetModule", new { id = @module.IdModulo }, @module);
        }

        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var @module = await _dataContext.Modules.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }

            _dataContext.Modules.Remove(@module);
            await _dataContext.SaveChangesAsync();

            return Ok(@module);
        }

        private bool ModuleExists(int id)
        {
            return _dataContext.Modules.Any(e => e.IdModulo == id);
        }



        //*********************************************************

    }
}