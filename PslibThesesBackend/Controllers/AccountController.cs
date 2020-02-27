using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PslibThesesBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet("Claims")]
        public List<ClaimViewModel> GetClaims()
        {
            var claims = User.Claims;
            return claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value }).ToList();
        }

        [HttpGet("Name")]
        public string GetUserName()
        {
            var user = User.Identity.Name;
            if (user == null)
                return "";
            return user;
        }

        [HttpGet("Name2")]
        public string GetNameClaim(int id)
        {
            var c = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault();
            if (c != null)
            {
                return c.Value;
            }
            return "--";
        }

        [HttpGet("Id")]
        public string GetNameIdentidierClaim(int id)
        {
            var c = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            if (c != null)
            {
                return c.Value;
            }
            return "--";
        }
    }

    public class ClaimViewModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}