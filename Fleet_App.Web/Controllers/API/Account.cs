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
    public class AccountController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AccountController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        [Route("GetUserByEmail")]
        public async Task<IActionResult> GetUser(UserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _dataContext.SubContratistasUsrWebs

                 .FirstOrDefaultAsync(o => o.USRLOGIN.ToLower() == userRequest.Email.ToLower());

            if (user == null)
            {
                return BadRequest("El Usuario no existe.");
            }

            var response = new UserResponse
            {
                IDUser = user.IDUser,
                APELLIDONOMBRE = user.APELLIDONOMBRE,
                CausanteC = user.CausanteC,
                CODIGO = user.CODIGO,
                HabilitadoWeb = user.HabilitadoWeb,
                USRCONTRASENA = user.USRCONTRASENA,
                USRLOGIN = user.USRLOGIN,
            };

            return Ok(response);
        }
    }
}