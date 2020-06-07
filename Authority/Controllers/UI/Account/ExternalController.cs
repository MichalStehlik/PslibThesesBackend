using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Authority.Data;
using Authority.Models;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authority.Quickstart.UI
{
    [SecurityHeaders]
    [AllowAnonymous]
    [Route("[controller]")]
    public class ExternalController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ExternalController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _events = events;
            _signInManager = signInManager;
        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet("Challenge")]
        public async Task<IActionResult> Challenge(string provider, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            // validate returnUrl - either it is a valid OIDC URL or back to a local page
            if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }

            if (AccountOptions.WindowsAuthenticationSchemeName == provider)
            {
                // windows authentication needs special handling
                return await ProcessWindowsLoginAsync(returnUrl);
            }
            else
            {
                // start challenge and roundtrip the return URL and scheme 
                var props = new AuthenticationProperties
                {
                    RedirectUri = Url.Action(nameof(Callback)),
                    Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", provider },
                    }
                };

                return Challenge(props, provider);
            }
        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet("Callback")]
        public async Task<IActionResult> Callback()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }
            var tokens = result.Properties.GetTokens();
            var accessToken = tokens.Where(t => t.Name == "access_token").FirstOrDefault();

            // lookup our user and external provider info
            var (user, provider, providerUserId, claims) = FindUserFromExternalProvider(result);
            if (user == null)
            {
                // this might be where you might initiate a custom workflow for user registration
                // in this sample we don't show how that would be done, as our sample implementation
                // simply auto-provisions new external user
                //user = AutoProvisionUser(provider, providerUserId, claims);
                return RedirectToAction("Connect");
            }

            // this allows us to collect any additonal claims or properties
            // for the specific prtotocols used and store them in the local auth cookie.
            // this is typically used to store data needed for signout from those protocols.
            var additionalLocalClaims = new List<Claim>();
            var localSignInProps = new AuthenticationProperties();
            ProcessLoginCallbackForOidc(result, additionalLocalClaims, localSignInProps);
            ProcessLoginCallbackForWsFed(result, additionalLocalClaims, localSignInProps);
            ProcessLoginCallbackForSaml2p(result, additionalLocalClaims, localSignInProps);

            if (provider == "Microsoft" && accessToken != null)
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokens.First(t => t.Name == accessToken.Value).Value}");
                var info = await client.GetAsync($"https://graph.microsoft.com/v1.0/me/people/?$filter=id eq '{providerUserId}'");
                var content = await info.Content.ReadAsAsync<dynamic>();
                string department = content.value[0].department;
                additionalLocalClaims.Add(new Claim("department",department));
            }

            foreach (var claim in claims)
            {
                additionalLocalClaims.Add(claim);
            }

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                additionalLocalClaims.Add(new Claim("role", role));
                var r = await _roleManager.FindByNameAsync(role);
                var claimsInRole = await _roleManager.GetClaimsAsync(r);
                foreach (var cl in claimsInRole)
                {
                    additionalLocalClaims.Add(new Claim(cl.Type, cl.Value));
                }
            }

            IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);
            foreach (var uc in userClaims)
            {
                additionalLocalClaims.Add(new Claim(uc.Type, uc.Value));
            }

            // issue authentication cookie for user
            await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id, user.UserName));
            await HttpContext.SignInAsync(user.Id, user.UserName, provider, localSignInProps, additionalLocalClaims.ToArray());

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // retrieve return URL
            var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

            // check if external login is in the context of an OIDC request
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context != null)
            {
                if (await _clientStore.IsPkceClientAsync(context.ClientId))
                {
                    // if the client is PKCE then we assume it's native, so this change in how to
                    // return the response is for better UX for the end user.
                    return View("Redirect", new RedirectViewModel { RedirectUrl = returnUrl });
                }
            }

            return Redirect(returnUrl);
        }

        [HttpGet("Connect")]
        public async Task<IActionResult> Connect()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }
            var (user, provider, providerUserId, claims) = FindUserFromExternalProvider(result);
            RegisterExternalInputModel model = new RegisterExternalInputModel {
                Provider = provider,
                ProviderUserId = providerUserId,
                Claims = claims,
                UserName = claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault().Value
            };
            return View(model);
        }
        
        [HttpPost("Connect")]
        public async Task<IActionResult> Connect(RegisterExternalInputModel model)
        {
            if (ModelState.IsValid)
            {
                if ((model.UserName == null) || (model.UserName == string.Empty))
                {
                    return RedirectToAction("Create");
                }
                else
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    if (user != null)
                    {
                        var externalResult = await HttpContext.AuthenticateAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);
                        var (localUser, provider, providerUserId, claims) = FindUserFromExternalProvider(externalResult);
                        var result = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider,providerUserId,user.UserName));
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Callback");
                        }
                        return View("Connect", model);
                    }
                    else
                    {
                        // -- TODO --
                        var firstname = model.Claims.Where(c => c.Type == ClaimTypes.GivenName).FirstOrDefault();
                        var lastname = model.Claims.Where(c => c.Type == ClaimTypes.Surname).FirstOrDefault();
                        var email = model.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
                        var gender = model.Claims.Where(c => c.Type == ClaimTypes.Gender).FirstOrDefault();
                        return RedirectToAction("Create", new { 
                            Provider = model.Provider, 
                            ProviderUserId = model.ProviderUserId, 
                            Claims = model.Claims, 
                            firstname = firstname != null ? firstname.Value : null,
                            lastname = lastname != null ? lastname.Value : null,
                            email = email != null ? email.Value : null,
                            gender = gender != null ? gender.Value : null,
                        });
                    }
                }
            }
            else
            {
                return View("Connect", model);
            }
        }

        [HttpGet("Create")]
        public IActionResult Create(string provider, string providerUserId, string userName, IList<Claim> claims, string firstname = "", string lastname = "", string email = "", string gender = "")
        {
            var vm = new CreateExternalInputModel { UserName = userName, Provider = provider, ProviderUserId = providerUserId, Claims = claims };
            if (!String.IsNullOrEmpty(firstname)) vm.FirstName = firstname;
            if (!String.IsNullOrEmpty(lastname)) vm.LastName = lastname;
            if (!String.IsNullOrEmpty(email)) vm.UserName = vm.Email = email;
            if (!String.IsNullOrEmpty(gender)) vm.Gender = gender == "F" ? Gender.Female : Gender.Male;
            return View(vm);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateExternalInputModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = model.EmailConfirmed,
                };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    var externalResult = await HttpContext.AuthenticateAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);
                    var (localUser, provider, providerUserId, claims) = FindUserFromExternalProvider(externalResult);
                    result = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, user.UserName));
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Callback");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        private async Task<IActionResult> ProcessWindowsLoginAsync(string returnUrl)
        {
            // see if windows auth has already been requested and succeeded
            var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
            if (result?.Principal is WindowsPrincipal wp)
            {
                // we will issue the external cookie and then redirect the
                // user back to the external callback, in essence, treating windows
                // auth the same as any other external authentication mechanism
                var props = new AuthenticationProperties()
                {
                    RedirectUri = Url.Action("Callback"),
                    Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", AccountOptions.WindowsAuthenticationSchemeName },
                    }
                };

                var id = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
                id.AddClaim(new Claim(JwtClaimTypes.Subject, wp.Identity.Name));
                id.AddClaim(new Claim(JwtClaimTypes.Name, wp.Identity.Name));

                // add the groups as claims -- be careful if the number of groups is too large
                if (AccountOptions.IncludeWindowsGroups)
                {
                    var wi = wp.Identity as WindowsIdentity;
                    var groups = wi.Groups.Translate(typeof(NTAccount));
                    var roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));
                    id.AddClaims(roles);
                }

                await HttpContext.SignInAsync(
                    IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme,
                    new ClaimsPrincipal(id),
                    props);
                return Redirect(props.RedirectUri);
            }
            else
            {
                // trigger windows auth
                // since windows auth don't support the redirect uri,
                // this URL is re-triggered when we call challenge
                return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
            }
        }

        private (ApplicationUser user, string provider, string providerUserId, IEnumerable<Claim> claims) FindUserFromExternalProvider(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            var provider = result.Properties.Items["scheme"];
            var providerUserId = userIdClaim.Value;

            // find external user
            var user = _userManager.FindByLoginAsync(provider, providerUserId).Result;

            return (user, provider, providerUserId, claims);
        }
        /*
        private TestUser AutoProvisionUser(string provider, string providerUserId, IEnumerable<Claim> claims)
        {
            var user = _users.AutoProvisionUser(provider, providerUserId, claims.ToList());
            return user;
        }
        */
        private void ProcessLoginCallbackForOidc(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            var id_token = externalResult.Properties.GetTokenValue("id_token");
            if (id_token != null)
            {
                localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = id_token } });
            }
        }

        private void ProcessLoginCallbackForWsFed(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
        }

        private void ProcessLoginCallbackForSaml2p(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
        }
    }
}
