using System;
using System.Collections.Generic;
using System.Linq;
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
    [Authorize(LocalApi.PolicyName)]
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
        public void Post([FromBody] ApplicationUser value)
        {
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ApplicationUser value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
        }
    }
}
