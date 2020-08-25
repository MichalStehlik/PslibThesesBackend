using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Authority.Data;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using static IdentityServer4.IdentityServerConstants;

namespace Authority.Areas.Api.Controllers
{
    [Route("api/Users")]
    [Authorize(Policy = "ApiAdmin")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private int iconSize;
        private int pictureSize;

        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; protected set; }

        public UsersController(UserManager<ApplicationUser> userManager, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _userManager = userManager;
            Configuration = configuration;
            iconSize = Convert.ToInt32(Configuration["Profile:IconSize"]);
            pictureSize = Convert.ToInt32(Configuration["Profile:PictureSize"]);
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<ApplicationUser> Get()
        {
            IQueryable<ApplicationUser> users = _userManager.Users.Select(r =>
            new ApplicationUser
            {
                Id = r.Id,
                FirstName = r.FirstName,
                MiddleName = r.MiddleName,
                LastName = r.LastName,
                UserName = r.UserName,
                Email = r.Email,
                PhoneNumber = r.PhoneNumber
            });
            return users.AsNoTracking();
        }

        // GET: api/Users/xxxxx-xxxx-xxxx
        [HttpGet("{id}", Name = "UsersGet")]
        public ActionResult<ApplicationUser> Get(string id)
        {
            var user = _userManager.Users.SingleOrDefault(r => r.Id == id);
            if (user != null)
                return Ok(user);
            else
                return NotFound();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] ApplicationUser value)
        {
            var result = await _userManager.CreateAsync(value);

            if (result.Succeeded)
            {
                return Ok(value);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(string id, [FromBody] ApplicationUser value)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == id);
            if (user != null)
            {
                value.Id = id;
                await _userManager.UpdateAsync(value);
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return NoContent();
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

        // GET: api/Users/xxxxx-xxxx-xxxx/Icon
        [HttpGet("{id}/icon")]
        public IActionResult Icon(string id)
        {
            var user = _userManager.Users.SingleOrDefault(r => r.Id == id);
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

        // GET: api/Users/xxxxx-xxxx-xxxx/Picture
        [HttpGet("{id}/picture")]
        public IActionResult Picture(string id)
        {
            var user = _userManager.Users.SingleOrDefault(r => r.Id == id);
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

        // GET: api/Users/xxxxx-xxxx-xxxx/Original
        [HttpGet("{id}/original")]
        public IActionResult Original(string id)
        {
            var user = _userManager.Users.SingleOrDefault(r => r.Id == id);
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

        // POST: api/Users/xxxxx-xxxx-xxxx/Image
        [HttpPost("{id}/image")]
        public async Task<IActionResult> UploadImage(string id)
        {
            var user = _userManager.Users.SingleOrDefault(r => r.Id == id);
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

        // POST: api/Users/xxxxx-xxxx-xxxx/Icon
        [HttpPost("{id}/icon")]
        public async Task<IActionResult> UploadIcon(string id)
        {
            var user = _userManager.Users.SingleOrDefault(r => r.Id == id);
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
                        MemoryStream oms = new MemoryStream();
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
                            image.Save(oms, format);
                        }
                        user.IconImage = oms.ToArray();
                        user.IconImageType = type;
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

        // POST: api/Users/xxxxx-xxxx-xxxx/picture
        [HttpPost("{id}/picture")]
        public async Task<IActionResult> UploadPicture(string id)
        {
            var user = _userManager.Users.SingleOrDefault(r => r.Id == id);
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
                        MemoryStream oms = new MemoryStream();
                        file.CopyTo(ims);
                        IImageFormat format;
                        using (Image image = Image.Load(ims.ToArray(), out format))
                        {
                            int largestSize = Math.Max(image.Height, image.Width);
                            bool landscape = image.Width > image.Height;
                            if (landscape)
                                image.Mutate(x => x.Resize(0, pictureSize));
                            else
                                image.Mutate(x => x.Resize(pictureSize, 0));
                            image.Mutate(x => x.Crop(new Rectangle((image.Width - pictureSize) / 2, (image.Height - pictureSize) / 2, pictureSize, pictureSize)));
                            image.Save(oms, format);
                        }
                        user.PictureImage = oms.ToArray();
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

        [HttpGet("{id}/claims")]
        public async Task<ActionResult> GetClaimsAsync(string id)
        {
            var user = _userManager.Users.SingleOrDefault(r => r.Id == id);
            if (user != null)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                return Ok(claims);
            }
            else
                return NotFound();
        }

        [HttpPost("{id}/claims")]
        public async Task<ActionResult> OnPostClaimsAsync(string id, [FromBody] Claim value)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user != null)
            {
                var result = await _userManager.AddClaimAsync(user, new Claim(value.Type, value.Value));
                if (result.Succeeded)
                {
                    return Ok(value);
                }
                else
                {
                    return BadRequest(result.Errors);
                }

            }
            else
                return NotFound();
        }

        [HttpDelete("{id}/claims/{type}/{value}")]
        public async Task<ActionResult> OnDeleteClaimsAsync(string id, string type, string value)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user != null)
            {
                var result = await _userManager.RemoveClaimAsync(user, new Claim(type, value));
                if (result.Succeeded)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
                return NotFound();
        }

        [HttpGet("{id}/roles")]
        public async Task<ActionResult> GetRolesAsync(string id)
        {
            var user = _userManager.Users.SingleOrDefault(r => r.Id == id);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(roles);
            }
            else
                return NotFound();
        }

        [HttpPost("{id}/roles")]
        public async Task<ActionResult> OnPostRolesAsync(string id, [FromBody] string value)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, value);
                if (result.Succeeded)
                {
                    return Ok(value);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
                return NotFound();
        }

        [HttpDelete("{id}/roles/{value}")]
        public async Task<ActionResult> OnDeleteRolesAsync(string id, string value)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, value);
                if (result.Succeeded)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
                return NotFound();
        }
    }
}
