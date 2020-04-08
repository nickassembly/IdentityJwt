using IdentityJWT.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityJWT.API.Services
{
    public interface IUserService
    {
      Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
    }

   public class UserService : IUserService
   {

      private UserManager<IdentityUser> _userManager;

      public UserService(UserManager<IdentityUser> usermanager)
      {
         _userManager = usermanager;
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
