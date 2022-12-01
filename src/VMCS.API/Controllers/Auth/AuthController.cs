using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VMCS.API.Models;
using VMCS.Core.Domains.Auth;

namespace VMCS.API.Controllers.Auth
{

    [Route("auth")]
    public class AuthController : Controller
    {
        private SignInManager<AuthUser> _signInManager;
        private UserManager<AuthUser> _userManager;

        public AuthController(UserManager<AuthUser> userManager, SignInManager<AuthUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [Route("register")]
        [HttpPost]
        public async Task Register(RegisterDTO registerData)
        {
            var user = new AuthUser()
            {
                Login = registerData.Login,
                UserName = registerData.Username,
                Email = registerData.Email
            };

            var result = await _userManager.CreateAsync(user, registerData.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                await HttpContext.Response.WriteAsync("Success!");
            }
            else
            {
                var errors = string.Join(";\n", result.Errors.Select(error => error.Description));
                await HttpContext.Response.WriteAsync("Not today, buddy :(");
                await HttpContext.Response.WriteAsync($"\n{errors}");
            }
        }

        [Route("login")]
        [HttpPost]
        public async Task Login(LoginDTO loginData)
        {
            var result = await _signInManager.PasswordSignInAsync(loginData.Login, loginData.Password, false, false);
            if (result.Succeeded)
            {
                await HttpContext.Response.WriteAsync("Logged in!");
            }
            else
            {
                await HttpContext.Response.WriteAsync("Sad, but you are not logged in :(");
            }
        }

        [Route("logout")]
        [HttpGet]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        [Route("whoami")]
        [HttpGet]
        public async Task WhoAmI()
        {
            var whoami = HttpContext.User.Identity?.Name??"Anonymous";
            await HttpContext.Response.WriteAsync(whoami);
        }
    }
}
