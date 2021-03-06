﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Authority.Data;
using Authority.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using X.PagedList;

namespace Authority.Areas.Dashboard.Controllers
{
    [Authorize(Policy = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private int iconSize;
        private int pictureSize;

        private List<SelectListItem> Genders;

        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; protected set; }

        public UsersController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            Genders = new List<SelectListItem> {
                new SelectListItem { Value = "0", Text = "Neznámé" },
                new SelectListItem { Value = "1", Text = "Muž" },
                new SelectListItem { Value = "2", Text = "Žena" },
                new SelectListItem { Value = "3", Text = "Jiné" }
            };
            _roleManager = roleManager;
            _userManager = userManager;
            Configuration = configuration;
            iconSize = Convert.ToInt32(Configuration["Profile:IconSize"]);
            pictureSize = Convert.ToInt32(Configuration["Profile:PictureSize"]);
        }
        // GET: Users
        [HttpGet("Users")]
        public ActionResult Index(string order, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = order;
            ViewBag.LastNameSortParm = String.IsNullOrEmpty(order) ? "lastname_desc" : "";
            ViewBag.FirstNameSortParm = order == "firstname" ? "firstname_desc" : "firstname";
            ViewBag.UserNameSortParm = order == "username" ? "username_desc" : "username";
            ViewBag.EmailSortParm = order == "email" ? "email_desc" : "email";
            IQueryable<ApplicationUser> users = _userManager.Users.Select(r =>
            new ApplicationUser
            {
                Id = r.Id,
                FirstName = r.FirstName,
                MiddleName = r.MiddleName,
                LastName = r.LastName,
                UserName = r.UserName,
                Email = r.Email
            });

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(
                    s => (s.FirstName.Contains(searchString) ||
                    s.MiddleName.Contains(searchString) ||
                    s.LastName.Contains(searchString) || 
                    s.Email.Contains(searchString) || 
                    s.UserName.Contains(searchString)));
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            switch (order)
            {
                case "lastname_desc":
                    users = users.OrderByDescending(s => s.LastName);
                    break;
                case "firstname":
                    users = users.OrderBy(s => s.FirstName);
                    break;
                case "firstname_desc":
                    users = users.OrderByDescending(s => s.FirstName);
                    break;
                case "username":
                    users = users.OrderBy(s => s.UserName);
                    break;
                case "username_desc":
                    users = users.OrderByDescending(s => s.UserName);
                    break;
                case "email":
                    users = users.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(s => s.Email);
                    break;
                default:
                    users = users.OrderBy(s => s.LastName);
                    break;
            }
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: Users/Details/5
        [HttpGet("Users/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            var user = _userManager.Users.SingleOrDefault(r => r.Id == id);
            var role = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.OrderBy(r => r.Name).ToList();
            var claims = await _userManager.GetClaimsAsync(user);
            var external = await _userManager.GetLoginsAsync(user);
            return View(new ExtendedUserViewModel { User = user, Roles = role, AllRoles = allRoles, Claims = claims, External = external });
        }

        // GET: Users/Icon/5
        [HttpGet("Users/Icon/{id}")]
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

        // GET: Users/Picture/5
        [HttpGet("Users/Picture/{id}")]
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

        [HttpPost("Users/UploadImage/{id}")]
        public async Task<IActionResult> UploadNewImage(string id)
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
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return RedirectToAction("Details",new { id = id});
        }

        // GET: Users/Create
        [HttpGet("Users/Create")]
        public ActionResult Create()
        {
            ViewData["Genders"] = Genders;
            ApplicationUser user = new ApplicationUser();
            return View(user);
        }

        // POST: Users/Create
        [HttpPost("Users/Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = collection["FirstName"].ToString(),
                    MiddleName = collection["MiddleName"].ToString(),
                    LastName = collection["LastName"].ToString(),
                    Gender = collection["Gender"] == "0" ? Gender.Male : (collection["Gender"] == "1" ? Gender.Female : Gender.Other),
                    UserName = collection["UserName"].ToString(),
                    Email = collection["Email"].ToString(),
                    EmailConfirmed = collection["EmailConfirmed"] == "true",
                    PhoneNumber = collection["PhoneNumber"].ToString(),
                    PhoneNumberConfirmed = collection["PhoneNumberConfirmed"] == "true",
                };
                var result = await _userManager.CreateAsync(user);
                TempData["SuccessMessage"] = "Role byla vytvořena";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        [HttpGet("Users/Edit/{id}")]
        public ActionResult Edit(string id)
        {
            ViewData["Genders"] = Genders;
            var user = _userManager.Users.SingleOrDefault(u => u.Id == id);
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost("Users/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, /*IFormCollection collection*/ApplicationUser model)
        {
            try
            {
                var user = _userManager.Users.SingleOrDefault(u => u.Id == id);
                user.FirstName = model.FirstName;
                user.MiddleName = model.MiddleName;
                user.LastName = model.LastName;
                user.Gender = model.Gender;
                user.Email = model.Email;
                user.EmailConfirmed = model.EmailConfirmed;
                user.UserName = model.UserName;
                user.PhoneNumber = model.PhoneNumber;
                user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                user.AccessFailedCount = model.AccessFailedCount;
                var result = await _userManager.UpdateAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        [HttpGet("Users/Delete/{id}")]
        public ActionResult Delete(string id)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == id);
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost("Users/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                var user = _userManager.Users.SingleOrDefault(u => u.Id == id);
                _userManager.DeleteAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet("Users/RemoveClaim")]
        public async Task<ActionResult> RemoveClaim(string user, string type, string value)
        {
            if (user == null) return RedirectToAction(nameof(Index));
            if (user != null && type != null && value != null)
            {
                var us = _userManager.Users.SingleOrDefault(u => u.Id == user);
                var claims = await _userManager.GetClaimsAsync(us);
                List<System.Security.Claims.Claim> userRemoveClaims = claims.Where(c => (c.Type == type && c.Value == value)).ToList();
                foreach (System.Security.Claims.Claim claim in userRemoveClaims)
                {
                    await _userManager.RemoveClaimAsync(us, claim);
                }
            }
            return RedirectToAction(nameof(Details), new { id = user });
        }

        [HttpPost("Users/CreateClaim")]
        public async Task<ActionResult> CreateClaim(string user, string type, string value)
        {
            if (user == null) return RedirectToAction(nameof(Index));
            if (user != null && type != null && value != null)
            {
                var us = _userManager.FindByIdAsync(user).Result;
                if (us == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                var result = await _userManager.AddClaimAsync(us, new System.Security.Claims.Claim(type, value));
            }
            return RedirectToAction(nameof(Details), new { id = user });
        }

        [HttpGet("Users/AddRole")]
        public async Task<ActionResult> AddRole(string user, string role)
        {
            if (user == null) return RedirectToAction(nameof(Index));
            if (user != null && role != null)
            {
                var us = _userManager.FindByIdAsync(user).Result;
                if (us == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                var result = await _userManager.AddToRoleAsync(us,role);
            }
            return RedirectToAction(nameof(Details), new { id = user });
        }

        [HttpGet("Users/RemoveRole")]
        public async Task<ActionResult> RemoveRole(string user, string role)
        {
            if (user == null) return RedirectToAction(nameof(Index));
            if (user != null && role != null)
            {
                var us = _userManager.FindByIdAsync(user).Result;
                if (us == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                var result = await _userManager.RemoveFromRoleAsync(us, role);
            }
            return RedirectToAction(nameof(Details), new { id = user });
        }

        [HttpGet("Users/RemoveExternal")]
        public async Task<ActionResult> RemoveExternal(string user, string provider, string key)
        {
            if (user == null) return RedirectToAction(nameof(Index));
            if (user != null && provider != null)
            {
                var us = _userManager.FindByIdAsync(user).Result;
                if (us == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                var result = await _userManager.RemoveLoginAsync(us,provider,key);
            }
            return RedirectToAction(nameof(Details), new { id = user });
        }

        [HttpPost("Users/AddExternal")]
        public async Task<ActionResult> AddExternal(string user, string provider, string name, string key)
        {
            if (user == null) return RedirectToAction(nameof(Index));
            if (user != null && provider != null && key != null)
            {
                var us = _userManager.FindByIdAsync(user).Result;
                if (us == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                var result = await _userManager.AddLoginAsync(us, new UserLoginInfo(provider, key, name));
            }
            return RedirectToAction(nameof(Details), new { id = user });
        }
    }
}