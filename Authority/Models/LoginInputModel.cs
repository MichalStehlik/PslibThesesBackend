// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;

namespace Authority.Models
{
    public class LoginInputModel
    {
        [Required]
        [Display(Name ="Uživatelské jméno")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Heslo")]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}