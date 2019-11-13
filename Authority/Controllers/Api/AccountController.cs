using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Authority.Data;
using Authority.ViewModels;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

namespace Authority.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize(LocalApi.PolicyName)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                return Ok(new ApplicationUserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Gender = user.Gender,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    HasPassword = ((user.PasswordHash != "") ? true : false),
                    //TwoFactorEnabled = user.TwoFactorEnabled,
                    //LockoutEnabled = user.LockoutEnabled,
                    //LockoutEnd = user.LockoutEnd,
                    AccessFailedCount = user.AccessFailedCount
                });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ApplicationUserInputModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.MiddleName = model.MiddleName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;
                user.Gender = model.Gender;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("password")]
        public async Task<ActionResult> OnPutPassword([FromBody] PasswordInputModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);    
            if (user != null)
            {
                IdentityResult changePasswordResult;
                if (await _userManager.HasPasswordAsync(user))
                {
                    changePasswordResult = await _userManager.ChangePasswordAsync(user, model.Current, model.Password);
                }
                else
                {
                    changePasswordResult = await _userManager.AddPasswordAsync(user, model.Password);
                }           
                if (changePasswordResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Password was not set.");
                }
            }
            else
            {
                return NotFound("User not recognized");
            }
        }

        [HttpGet("claims")]
        public async Task<IActionResult> GetClaims()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
                return Ok(await _userManager.GetClaimsAsync(user));
            else
                return NotFound();
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(roles);
            }
            else
                return NotFound();
        }
    }
}
