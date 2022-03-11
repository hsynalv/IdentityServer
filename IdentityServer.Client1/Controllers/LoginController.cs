using IdentityModel.Client;
using IdentityServer.Client1.Models;
using IdentityServer.Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Client1.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IApiResourceHttpClient _apiResourceHttpClient;

        public LoginController(IConfiguration configuration, IApiResourceHttpClient apiResourceHttpClient)
        {
            Configuration = configuration;
            _apiResourceHttpClient = apiResourceHttpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginView)
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(Configuration["AuthServerUrl"]);

            if (disco.IsError)
                throw disco.Exception;

            var password = new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                UserName = loginView.Email,
                Password = loginView.Passowrd,
                ClientId = Configuration["Client-ResourceOwner:ClientId"],
                ClientSecret = Configuration["Client-ResourceOwner:ClientSecret"]
            };

            var token = await client.RequestPasswordTokenAsync(password);

            if (token.IsError)
            {
                ModelState.AddModelError("", "Email veya şifreniz hatalıdır.");
                return View();
            }
                

            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = disco.UserInfoEndpoint
            };

            var userInfo = await client.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
                throw userInfo.Exception;

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme,"name","role");

            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.IdToken, Value = token.IdentityToken },
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken },
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken },
                new AuthenticationToken{ Name = OpenIdConnectParameterNames.ExpiresIn,Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            });

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return RedirectToAction("Index","User");
            
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(UserSaveViewModel userSaveViewModel)
        {
            if (!ModelState.IsValid) return View();

            var result = await _apiResourceHttpClient.SaveUserViewModel(userSaveViewModel);

            if (result != null)
            {
                result.ForEach(error =>
                {
                    ModelState.AddModelError("", error);
                });
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}

