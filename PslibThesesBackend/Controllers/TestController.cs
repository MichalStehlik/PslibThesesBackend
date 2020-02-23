using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PslibThesesBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("Claims")]
        public List<ClaimViewModel> GetClaims()
        {
            var claims = User.Claims;
            return claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value}).ToList();
        }

        [HttpGet("Name")]
        public string GetName()
        {
            return User.Identity.Name;
        }
    }
}