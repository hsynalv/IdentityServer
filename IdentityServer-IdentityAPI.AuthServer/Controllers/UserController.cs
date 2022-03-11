using IdentityServer_IdentityAPI.AuthServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer_IdentityAPI.AuthServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(LocalApi.PolicyName)]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserSaveViewModel userSaveViewModel)
        {
            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = userSaveViewModel.UserName,
                Email = userSaveViewModel.Email,
                City = userSaveViewModel.City,
            };

            var result = await _userManager.CreateAsync(applicationUser,userSaveViewModel.Password);

            if(!result.Succeeded)
                return BadRequest(result.Errors.Select(x=>x.Description));


            return Ok("Kayıt İşlemi Başarıyla Tamamlandı");
        }
    }
}
