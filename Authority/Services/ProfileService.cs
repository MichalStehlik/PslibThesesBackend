using Authority.Data;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authority.Services
{
    public class ProfileService<TUser> : IProfileService where TUser : IdentityUser
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<ApplicationRole> _roleManager;
        protected readonly IUserClaimsPrincipalFactory<TUser> _claimsFactory;

        public ProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<TUser> claimsFactory, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _roleManager = roleManager;
        }

        public virtual async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.Claims.FirstOrDefault(cl => cl.Type == "sub");
            if (sub == null)
                throw new Exception("No sub claim provided");
            var user = await _userManager.FindByIdAsync(sub.Value);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var ir in context.RequestedResources.IdentityResources)
                {
                    switch (ir.Name)
                    {
                        case "email":
                            context.IssuedClaims.Add(new Claim("email", user.Email));
                            context.IssuedClaims.Add(new Claim("email_verified", user.EmailConfirmed == true ? "1" : "0"));
                            break;
                        case "profile":
                            context.IssuedClaims.Add(new Claim("preferred_username", user.UserName));
                            context.IssuedClaims.Add(new Claim("given_name", user.FirstName));
                            context.IssuedClaims.Add(new Claim("middle_name", user.MiddleName != null ? user.MiddleName : ""));
                            context.IssuedClaims.Add(new Claim("family_name", user.LastName));
                            context.IssuedClaims.Add(new Claim("gender", user.Gender == Gender.Male ? "male" : "female"));
                            context.IssuedClaims.Add(new Claim("name", user.FullName));
                            break;
                        case "phone":
                            context.IssuedClaims.Add(new Claim("phone_number", user.PhoneNumber != null ? user.PhoneNumber : ""));
                            context.IssuedClaims.Add(new Claim("phone_number_verified", user.PhoneNumberConfirmed == true ? "1" : "0"));
                            break;
                        case "roles":
                            foreach (var role in roles)
                            {
                                context.IssuedClaims.Add(new Claim("role", role));
                            }
                            //context.IssuedClaims.Add(new Claim("roles", String.Join(",", roles)));
                            break;
                    }
                }
                
                foreach (var role in roles)
                {
                    var r = await _roleManager.FindByNameAsync(role);
                    var claimsInRole = await _roleManager.GetClaimsAsync(r);
                    foreach (var cl in claimsInRole)
                    {
                        context.IssuedClaims.Add(new Claim(cl.Type, cl.Value));
                    }
                }
                
                List<Claim> userClaims = (List<Claim>)_userManager.GetClaimsAsync(user).Result;
                foreach (var uc in userClaims)
                {
                    context.IssuedClaims.Add(new Claim(uc.Type, uc.Value));
                }
            }
            //return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;

            return Task.FromResult(0);
        }
    }
}
