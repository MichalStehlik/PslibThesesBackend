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
    public class ClientsController : Controller
    {
        protected ConfigurationDbContext _context;
        public ClientsController(ConfigurationDbContext context)
        {
            _context = context;
        }

        // GET: Clients
        public ActionResult Index(string order, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = order;
            ViewBag.NameSortParm = String.IsNullOrEmpty(order) ? "name_desc" : "";
            IQueryable<ClientListViewModel> resources = _context.Clients.Select(c =>
            new ClientListViewModel
            {
                Id = c.Id,
                ClientId = c.ClientId,
                ClientName = c.ClientName,
            });
            if (!String.IsNullOrEmpty(searchString))
            {
                resources = resources.Where(
                    ir => (ir.ClientName.Contains(searchString)));
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            switch (order)
            {
                case "name_desc":
                    resources = resources.OrderByDescending(ir => ir.ClientName);
                    break;
                default:
                    resources = resources.OrderBy(ir => ir.ClientName);
                    break;
            }
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(resources.ToPagedList(pageNumber, pageSize));
        }

        // GET: Clients/Details/5
        public ActionResult Details(int id)
        {
            var c = _context.Clients.SingleOrDefault(c => c.Id == id);
            return View(new ClientViewModel { Client = c });
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            Client c = new Client();
            return View(c);
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client model)
        {
            try
            {
                if (model.ClientName == "") throw new ArgumentException();
                if (model.ClientId == "") throw new ArgumentException();
                var result = _context.Clients.Add(model);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Klient byl vytvořen";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Při vytváření klienta došlo k chybě";
                return View(model);
            }
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int id)
        {
            var c = _context.Clients.SingleOrDefault(c => c.Id == id);
            return View(c);
        }

        // POST: Clients/Edit/5
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

        // GET: Clients/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Clients/Delete/5
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

    public class ClientListViewModel
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public bool Enabled { get; set; }
    }

    public class ClientViewModel
    {
        public Client Client { get; set; }
    }
}