﻿using Authority.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authority.Models
{
    public class RegisterExternalInputModel
    {
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
        [Display(Name="Uživatelské jméno")]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        //public string ReturnUrl { get; set; }
    }
}
