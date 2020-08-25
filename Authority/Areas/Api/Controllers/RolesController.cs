using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Authority.Data;
using Microsoft.AspNetCore.Authorization;
using static IdentityServer4.IdentityServerConstants;

namespace Authority.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "ApiAdmin")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RolesController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: api/Roles
        [HttpGet]
        public ActionResult Get(string order, string filter, int page = 0, int pageSize = 5)
        {
            IQueryable<ApplicationRole> roles = _roleManager.Roles.Select(r =>
            new ApplicationRole
            {
                Id = r.Id,
                Name = r.Name
            });
            int total = roles.CountAsync().Result;
            if (order == "name.desc")
                roles = roles.OrderByDescending(r => r.Name);
            else
                roles = roles.OrderBy(r => r.Name);
            if (filter != null && filter != "")
                roles = roles.Where(r => r.Name.Contains(filter));
            int filtered = roles.CountAsync().Result;
            roles = roles.Skip(page * pageSize).Take(pageSize);
            int count = roles.CountAsync().Result;
            return Ok(new { total = total, filtered = filtered, count = count, page = page, pages = (int)Math.Ceiling((double)filtered / pageSize), data = roles.AsNoTracking() });
        }

        // GET: api/Roles/xxxxx-x-x-x-x-
        [HttpGet("{id}", Name = "RolesGet")]
        public ActionResult<ApplicationRole> Get(string id)
        {
            var role = _roleManager.Roles.SingleOrDefault(r => r.Id == id);
            if (role != null)
                return Ok(new ApplicationRole
                {
                    Id = role.Id,
                    Name = role.Name
                });
            else
                return NotFound();
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] ApplicationRole value)
        {
            var role = new ApplicationRole
            {
                Name = value.Name
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                var roledata = new ApplicationRole
                {
                    Id = role.Id,
                    Name = role.Name
                };
                return Ok(roledata);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        // PUT: api/Roles/xxxx-xxxxx-xxxx
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(string id, [FromBody] ApplicationRole value)
        {
            var role = _roleManager.Roles.SingleOrDefault(r => r.Id == id);
            if (role != null)
            {
                role.Name = value.Name;
                await _roleManager.UpdateAsync(role);
                return Ok(role);
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE: api/Roles/xxxxxxx
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var role = _roleManager.Roles.SingleOrDefault(r => r.Id == id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
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

        [HttpGet("{id}/claims")]
        public async Task<ActionResult> GetClaimsAsync(string id)
        {
            var role = _roleManager.Roles.SingleOrDefault(r => r.Id == id);
            if (role != null)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                return Ok(claims);
            }
            else
                return NotFound();
        }

        [HttpPost("{id}/claims")]
        public async Task<ActionResult> OnPostClaimsAsync(string id, [FromBody] Claim value)
        {
            var role = _roleManager.FindByIdAsync(id).Result;
            if (role != null)
            {
                var result = await _roleManager.AddClaimAsync(role, new Claim(value.Type, value.Value));
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
            var role = _roleManager.FindByIdAsync(id).Result;
            if (role != null)
            {
                var result = await _roleManager.RemoveClaimAsync(role, new Claim(type, value));
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
