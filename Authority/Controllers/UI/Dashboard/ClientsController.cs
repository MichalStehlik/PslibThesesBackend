using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var c = _context.Clients
                .Include(x => x.ClientSecrets)
                .Include(x => x.AllowedScopes)
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedCorsOrigins)
                .SingleOrDefault(c => c.Id == id);
            if (c == null) return NotFound();
            return View(new ClientViewModel 
            { 
                Client = c, 
                ClientSecrets = c.ClientSecrets,
                AllowedGrantTypes = c.AllowedGrantTypes,
                AllowedScopes = c.AllowedScopes,
                AllowedCorsOrigins = c.AllowedCorsOrigins,
                RedirectUris = c.RedirectUris,
                PostLogoutRedirectUris = c.PostLogoutRedirectUris
            });
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            IdentityServer4.EntityFramework.Entities.Client c = new IdentityServer4.EntityFramework.Entities.Client();
            return View(c);
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityServer4.EntityFramework.Entities.Client model)
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
            if (c == null) return NotFound();
            return View(c);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IdentityServer4.EntityFramework.Entities.Client model)
        {
            var c = _context.Clients.SingleOrDefault(u => u.Id == id);
            if (c == null) return NotFound();
            try
            {
                if (id != model.Id) throw new ArgumentException();
                if (model.ClientName == "") throw new ArgumentException();
                if (model.ClientId == "") throw new ArgumentException();
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Klient byl uložen";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Při ukládání klienta došlo k chybě";
                return View(c);
            }
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int id)
        {
            var c = _context.Clients.SingleOrDefault(c => c.Id == id);
            if (c == null) return NotFound();
            return View(c);
        }

        // POST: Clients/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var c = _context.Clients.SingleOrDefault(u => u.Id == id);
                if (c == null) return NotFound();
                _context.Clients.Remove(c);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(new ClientSecretInputModel { });
            }
        }

        // GET: Clients/CreateSecret/5
        public ActionResult CreateSecret(int id)
        {
            var c = _context.Clients.SingleOrDefault(c => c.Id == id);
            if (c != null)
            {
                var generatedSecret = Guid.NewGuid().ToString().Replace("-", string.Empty);
                return View(new ClientSecretInputModel { Value = generatedSecret, ClientId = id });
            }
            else
            {
                TempData["ErrorMessage"] = "Neznámý klient.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Clients/CreateSecret/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSecret(int id, ClientSecretInputModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityServer4.EntityFramework.Entities.Client client = _context.Clients.Include(x => x.ClientSecrets).FirstOrDefaultAsync(n => n.Id == model.ClientId).Result;
                if (client == null)
                {
                    TempData["ErrorMessage"] = "Neznámý klient.";
                    return RedirectToAction(nameof(Index));
                }
                client.ClientSecrets.Add(new ClientSecret
                {
                    Value = model.Value.Sha256(),
                    Description = model.Description
                });
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = model.ClientId });
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Aktualizace klienta se nepodařila.";
                    return RedirectToAction("Details", new { id = model.ClientId });
                }
            }
            return View(model);
        }

        // GET: Clients/RemoveSecret/5?Client=1
        public async Task<ActionResult> RemoveSecret(int id, int client)
        {
            var c = _context.Clients.Include(x => x.ClientSecrets).FirstOrDefaultAsync(cl => cl.Id == client).Result;
            if (c == null)
            {
                return NotFound();
            }

            var secret = c.ClientSecrets.Where(x => x.Id == id).FirstOrDefault();
            if (secret == null)
            {
                return NotFound();
            }
            c.ClientSecrets.Remove(secret);
            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Klientovi byl odebrán secret.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Odstranění secretu se nepodařilo.";
            }

            return RedirectToAction("Details", new { id = client });
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
        public IdentityServer4.EntityFramework.Entities.Client Client { get; set; }
        public List<ClientSecret> ClientSecrets { get; set; }
        public List<ClientScope> AllowedScopes { get; set; }
        public List<ClientGrantType> AllowedGrantTypes { get; set; }
        public List<ClientRedirectUri> RedirectUris { get; set; }
        public List<ClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; set; }
        public List<ClientCorsOrigin> AllowedCorsOrigins { get; set; }
    }

    public class ClientSecretInputModel
    {
        public int ClientId { get; set; }
        [Required]
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Description { get; set; }
    }
}