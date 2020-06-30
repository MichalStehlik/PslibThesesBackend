using System;
using System.Collections.Generic;
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
using static IdentityServer4.IdentityServerConstants;

namespace Authority.Controllers.Api
{
    [Route("api/Users")]
    [Authorize(Constants.LocalScopeName)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
