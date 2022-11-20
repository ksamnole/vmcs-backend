using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VMCS.API.Models;

namespace VMCS.API.Controllers
{
    public class UserController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        //public IActionResult Register(RegisterDTO registrationData)
        //{
        //}
    }
}
