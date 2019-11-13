using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Authority.Controllers.UI.Dashboard
{
    [Authorize(Policy = "Admin")]
    public class IdentityResourcesController : Controller
    {
        protected ConfigurationDbContext _context;

        public IdentityResourcesController(ConfigurationDbContext context)
        {
            _context = context;
        }

        // GET: IdentityResources
        public ActionResult Index(string order, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = order;
            ViewBag.NameSortParm = String.IsNullOrEmpty(order) ? "name_desc" : "";
            IQueryable<IdentityResource> resources = _context.IdentityResources.Select(ir =>
            new IdentityResource
            {
                Id = ir.Id,
                Enabled = ir.Enabled,
                Name = ir.Name,
                DisplayName = ir.DisplayName,
                Description = ir.Description,
                Required = ir.Required,
                Emphasize = ir.Emphasize,
                ShowInDiscoveryDocument = ir.ShowInDiscoveryDocument,
                Created = ir.Created,
                NonEditable = ir.NonEditable,
                Updated = ir.Updated
            });
            if (!String.IsNullOrEmpty(searchString))
            {
                resources = resources.Where(
                    ir => (ir.Name.Contains(searchString)));
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            switch (order)
            {
                case "name_desc":
                    resources = resources.OrderByDescending(ir => ir.Name);
                    break;
                default:
                    resources = resources.OrderBy(ir => ir.Name);
                    break;
            }
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(resources.ToPagedList(pageNumber, pageSize));
        }

        // GET: IdentityResources/Details/5
        public ActionResult Details(int id)
        {
            var ires = _context.IdentityResources.SingleOrDefault(ir => ir.Id == id);
            return View(ires);
        }

        // GET: IdentityResources/Create
        public ActionResult Create()
        {
            IdentityResource ires = new IdentityResource();
            return View(ires);
        }

        // POST: IdentityResources/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                IdentityResource ires = new IdentityResource
                {
                    Name = collection["Name"].ToString(),
                    DisplayName = collection["DisplayName"].ToString(),
                    Description = collection["Description"].ToString(),
                    Enabled = collection["Enabled"] == "true",
                    Required = collection["Required"] == "true",
                    Emphasize = collection["Emphasize"] == "true",
                    ShowInDiscoveryDocument = collection["ShowInDiscoveryDocument"] == "true"
                };
                var result = _context.IdentityResources.Add(ires);
                TempData["SuccessMessage"] = "Zdroj byl vytvořen";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: IdentityResources/Edit/5
        public ActionResult Edit(int id)
        {
            var ires = _context.IdentityResources.SingleOrDefault(u => u.Id == id);
            return View(ires);
        }

        // POST: IdentityResources/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: IdentityResources/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: IdentityResources/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}