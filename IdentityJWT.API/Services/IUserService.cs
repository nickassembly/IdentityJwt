using IdentityJWT.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityJWT.API.Services
{
    public interface IUserService
    {
      Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
      Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
    }

   public class UserService : IUserService
   {

      private readonly UserManager<IdentityUser> _userManager;
      private readonly IConfiguration _configuration;

      public UserService(UserManager<IdentityUser> usermanager, IConfiguration configuration)
      {
         _userManager = usermanager;
         _configuration = configuration;
      }

      public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
      {
         var user = await _userManager.FindByEmailAsync(model.Email);

         if(user == null)
         {
            return new UserManagerResponse
            {
               Message = "There is no user with that email address",
               IsSuccess = false
            };
         }

         var result = await _userManager.CheckPasswordAsync(user, model.Password);

         if (!result)
         {
            return new UserManagerResponse
            {
               Message = "Incorrect Password",
               IsSuccess = false
            };
         }

         var claims = new[]
         {
            new Claim("Email", model.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
         };

      }

      public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
      {
         if (model == null)
            throw new NullReferenceException("Register Model is null");

         if (model.Password != model.ConfirmPassword)
            return new UserManagerResponse
            {
               Message = "Confirm password doesn't match.",
               IsSuccess = false,
            };

         var identityUser = new IdentityUser
         {
            Email = model.Email,
            UserName = model.Email
         };

         var result = await _userManager.CreateAsync(identityUser, model.Password);

         if (result.Succeeded)
         {
            // TODO: send confirmation email
            return new UserManagerResponse
            {
               Message = "User created Successfully",
               IsSuccess = true
            };
         }

         return new UserManagerResponse
         {
            Message = "User did not create",
            IsSuccess = false,
            Errors = result.Errors.Select(e => e.Description)
         };

      }
   }


}
