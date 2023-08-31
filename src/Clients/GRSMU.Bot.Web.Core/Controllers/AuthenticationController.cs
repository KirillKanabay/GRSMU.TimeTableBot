using GRSMU.Bot.Domain.Identity.Models;
using GRSMU.Bot.Web.Core.Immutable;
using GRSMU.Bot.Web.Core.ViewModels.Authetication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GRSMU.Bot.Web.Core.Controllers
{
    [Controller]
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationController(SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet("register")]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewBag[ViewBagKeys.ReturnUrl] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register()
        {

        }

        [HttpGet("login")]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag[ViewBagKeys.ReturnUrl] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ViewBag[ViewBagKeys.ReturnUrl] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync
                (
                    loginViewModel.Login,
                    loginViewModel.Password,
                    false,
                    true
                );

                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
            }

            return View();
        }
    }
}
