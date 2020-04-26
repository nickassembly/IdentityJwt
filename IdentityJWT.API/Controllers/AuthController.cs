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
      private readonly IMailService _mailService;
      public AuthController(IUserService userService, IMailService mailService)
      {
         _userService = userService;
         _mailService = mailService;
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

      // /api/auth/login
      [HttpPost("Login")]
      public async Task<IActionResult> LoginAsync([FromBody]LoginViewModel model)
      {
         if (ModelState.IsValid)
         {
            var result = await _userService.LoginUserAsync(model);

            if (result.IsSuccess)
            {
               await _mailService.SendEmailAsync(model.Email, "New login", "<h1>Hello, New Login to your account</h1><p>New login to your account at " + DateTime.Now + "</p>");
               return Ok(result);
            }

            return BadRequest(result);
               
         }

         return BadRequest("Some properties are not valid");
      }


    }
}
