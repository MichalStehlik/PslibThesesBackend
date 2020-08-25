using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authority.Data;
using Authority.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Authority.Areas.Dashboard.Controllers
{
    [Authorize(Policy = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RolesController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: Roles
        [HttpGet("Roles")]
        public ActionResult Index(string order, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = order;
            ViewBag.NameSortParm = String.IsNullOrEmpty(order) ? "name_desc" : "";
            IQueryable<ApplicationRole> roles = _roleManager.Roles.Select(r =>
            new ApplicationRole
            {
                Id = r.Id,
                Name = r.Name
            });

            if (!String.IsNullOrEmpty(searchString))
            {
                roles = roles.Where(s => (s.Name.Contains(searchString)));
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            switch (order)
            {
                case "name":
                    roles = roles.OrderBy(r => r.Name);
                    break;
                case "name_desc":
                    roles = roles.OrderByDescending(r => r.Name);
                    break;
                default:
                    roles = roles.OrderBy(r => r.Name);
                    break;
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(roles.ToPagedList(pageNumber, pageSize));
        }

        // GET: Roles/Details/5
        [HttpGet("Roles/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            var role = _roleManager.Roles.SingleOrDefault(r => r.Id == id);
            var claims = await _roleManager.GetClaimsAsync(role);
            return View(new ExtendedRoleViewModel{ Role = role, Claims = claims});
        }

        // GET: Roles/Create
        [HttpGet("Roles/Create")]
        public ActionResult Create()
        {
            ApplicationRole role = new ApplicationRole();
            return View(role);
        }

        // POST: Roles/Create
        [HttpPost("Roles/Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                ApplicationRole role = new ApplicationRole { Name = collection["Name"].ToString()};
                var result = await _roleManager.CreateAsync(role);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Roles/Edit/5
        [HttpGet("Roles/Edit/{id}")]
        public ActionResult Edit(string id)
        {
            var role = _roleManager.Roles.SingleOrDefault(r => r.Id == id);
            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost("Roles/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, IFormCollection collection)
        {
            try
            {
                var role = _roleManager.Roles.SingleOrDefault(r => r.Id == id);
                role.Name = collection["Name"].ToString();
                role.NormalizedName = collection["NormalizedName"].ToString();
                var result = await _roleManager.UpdateAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Roles/Delete/5
        [HttpGet("Roles/Delete/{id}")]
        public ActionResult Delete(string id)
        {
            var role = _roleManager.Roles.SingleOrDefault(r => r.Id == id);
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost("Roles/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                var role = _roleManager.Roles.SingleOrDefault(r => r.Id == id);
                _roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet("Roles/RemoveClaim")]
        public async Task<ActionResult> RemoveClaim(string role, string type, string value)
        {
            if (role == null) return RedirectToAction(nameof(Index));
            if (role != null && type != null && value != null)
            {
                var rl = _roleManager.Roles.SingleOrDefault(r => r.Id == role);
                var claims = await _roleManager.GetClaimsAsync(rl);
                List<System.Security.Claims.Claim> roleRemoveClaims = claims.Where(c => (c.Type == type && c.Value == value)).ToList();
                foreach (System.Security.Claims.Claim claim in roleRemoveClaims)
                {
                    await _roleManager.RemoveClaimAsync(rl, claim);
                }
            }
            return RedirectToAction(nameof(Details), new { id = role });
        }

        [HttpPost("Roles/CreateClaim")]
        public async Task<ActionResult> CreateClaim(string role, string type, string value)
        {
            if (role == null) return RedirectToAction(nameof(Index));
            if (role != null && type != null && value != null)
            {
                var rl = _roleManager.FindByIdAsync(role).Result;
                if (rl == null)
                {
                    return NotFound();
                }
                var result = await _roleManager.AddClaimAsync(rl, new System.Security.Claims.Claim(type, value));
            }
            return RedirectToAction(nameof(Details), new { id = role });
        }
    }
}