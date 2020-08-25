using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Authority.Data;
using Authority.ViewModels;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using static IdentityServer4.IdentityServerConstants;
using Microsoft.Extensions.Configuration;

namespace Authority.Areas.Api.Controllers
{
    [Route("api/account")]
    [Authorize(LocalApi.PolicyName)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private int iconSize;
        private int pictureSize;
        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; protected set; }
        public AccountController(UserManager<ApplicationUser> userManager, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _userManager = userManager;
            Configuration = configuration;
            iconSize = Convert.ToInt32(Configuration["Profile:IconSize"]);
            pictureSize = Convert.ToInt32(Configuration["Profile:PictureSize"]);
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
                    AccessFailedCount = user.AccessFailedCount,
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

        [HttpGet("icon")]
        public async Task<IActionResult> Icon()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                if (user.IconImage != null)
                {
                    return File(user.IconImage, user.IconImageType);
                }
                else
                {
                    return NoContent();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("picture")]
        public async Task<IActionResult> Picture()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                if (user.PictureImage != null)
                {
                    return File(user.PictureImage, user.PictureImageType);
                }
                else
                {
                    return NoContent();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("original")]
        public async Task<IActionResult> Original()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                if (user.OriginalImage != null)
                {
                    return File(user.OriginalImage, user.OriginalImageType);
                }
                else
                {
                    return NoContent();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadImage()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null && Request.Form.Files.Count == 1)
            {
                var file = Request.Form.Files[0];
                if (file != null && file.Length > 0)
                {
                    try
                    {
                        var size = file.Length;
                        var type = file.ContentType;
                        var filename = file.FileName;
                        MemoryStream ims = new MemoryStream();
                        MemoryStream oms1 = new MemoryStream();
                        MemoryStream oms2 = new MemoryStream();
                        file.CopyTo(ims);
                        IImageFormat format;
                        using (Image image = Image.Load(ims.ToArray(), out format))
                        {
                            int largestSize = Math.Max(image.Height, image.Width);
                            bool landscape = image.Width > image.Height;
                            if (landscape)
                                image.Mutate(x => x.Resize(0, iconSize));
                            else
                                image.Mutate(x => x.Resize(iconSize, 0));
                            image.Mutate(x => x.Crop(new Rectangle((image.Width - iconSize) / 2, (image.Height - iconSize) / 2, iconSize, iconSize)));
                            image.Save(oms1, format);
                        }
                        using (Image image = Image.Load(ims.ToArray(), out format))
                        {
                            int largestSize = Math.Max(image.Height, image.Width);
                            bool landscape = image.Width > image.Height;
                            if (landscape)
                                image.Mutate(x => x.Resize(0, pictureSize));
                            else
                                image.Mutate(x => x.Resize(pictureSize, 0));
                            image.Mutate(x => x.Crop(new Rectangle((image.Width - pictureSize) / 2, (image.Height - pictureSize) / 2, pictureSize, pictureSize)));
                            image.Save(oms2, format);
                        }
                        user.OriginalImage = ims.ToArray();
                        user.OriginalImageType = type;
                        user.IconImage = oms1.ToArray();
                        user.IconImageType = type;
                        user.PictureImage = oms2.ToArray();
                        user.PictureImageType = type;
                        var result = await _userManager.UpdateAsync(user);
                        return Ok();
                    }
                    catch (Exception)
                    {
                        return BadRequest();
                    }
                }
                return BadRequest();
            }
            return BadRequest();
        }
    }
}
