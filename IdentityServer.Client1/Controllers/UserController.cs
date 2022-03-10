using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer.Client1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IConfiguration configuration;

        public UserController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index","Home");
            //await HttpContext.SignOutAsync("oidc");
        }


        public async Task<IActionResult> GetRefreshToken()
        {
            HttpClient _httpClient = new HttpClient();

            var discovery = await _httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (discovery is null)
                return NotFound();


            var resfreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);



            RefreshTokenRequest request = new RefreshTokenRequest
            {
                ClientId = configuration["Client-ResourceOwner:ClientId"],
                ClientSecret = configuration["Client-ResourceOwner:ClientSecret"],
                RefreshToken = resfreshToken,
                Address = discovery.TokenEndpoint,
            };

            var token = await _httpClient.RequestRefreshTokenAsync(request);
            if (token is null)
                return NotFound();

            var tokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken(){ Name = OpenIdConnectParameterNames.IdToken, Value = token.IdentityToken },
                new AuthenticationToken(){ Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken },
                new AuthenticationToken(){ Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken },
                new AuthenticationToken(){ Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture) }

            };


            var authenticationResult = await HttpContext.AuthenticateAsync();

            var properties = authenticationResult.Properties;

            properties.StoreTokens(tokens);

            await HttpContext.SignInAsync("Cookies", authenticationResult.Principal, properties);

            return RedirectToAction("Index");
        }
    }
}
