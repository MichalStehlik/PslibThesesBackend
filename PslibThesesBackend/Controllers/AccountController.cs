using System;
using System.Collections.Generic;
using System.Linq;
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
    }

    public class ClaimViewModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}