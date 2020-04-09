using IdentityJWT.API.Services;
using IdentityJWT.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityJWT.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
    public class AuthController : ControllerBase
    {

      private readonly IUserService _userService;

      public AuthController(IUserService userService)
      {
         _userService = userService;
      }

      // /api/auth/register
      [HttpPost("Register")]
      public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
      {
         if (ModelState.IsValid)
         {
            var result = await _userService.RegisterUserAsync(model);

            if (result.IsSuccess)
               return Ok(result); // Status Code: 200

            return BadRequest(result);
         }

         return BadRequest("Some properties are not valid"); // Status Code: 400
      }


    }
}
