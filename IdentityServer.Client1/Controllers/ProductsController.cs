using IdentityModel.Client;
using IdentityServer.Client1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace IdentityServer.Client1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration configuration;
        List<Product> products = new List<Product>();

        public ProductsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<ActionResult> Index()
        {
            HttpClient httpClient = new HttpClient();
            var discovery = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");

            if (discovery.IsError)
                throw new System.Exception(discovery.Error);

            ClientCredentialsTokenRequest clientCredentialsTokenRequest = new ClientCredentialsTokenRequest()
            {
                ClientId = configuration["Client:ClientId"],
                ClientSecret = configuration["Client:ClientSecret"],
                Address = discovery.TokenEndpoint
            };

            var token = await httpClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);

            if (token.IsError)
                throw new System.Exception(token.Error);

            httpClient.SetBearerToken(token.AccessToken);

            var response = await httpClient.GetAsync("https://localhost:5016/api/products");

            if (!response.IsSuccessStatusCode)
                throw new System.Exception(response.RequestMessage.ToString());



            var content = await response.Content.ReadAsStringAsync();
            products = JsonConvert.DeserializeObject<List<Product>>(content);
            return View(products);
        }
    }
}
