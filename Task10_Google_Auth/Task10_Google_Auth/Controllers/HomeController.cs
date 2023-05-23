using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Task10_Google_Auth.Models;

namespace Task10_Google_Auth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task Login()
        {
            // Start the process of login
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                // call Response after successful authentication
                RedirectUri = Url.Action("Response")
            });
        }

        public async Task<IActionResult> Response()
        {
            // Authenticates the user using the specified authentication scheme
            var res = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Retrieves the user info from the authenticated principal
            var claims = res.Principal.Identities.FirstOrDefault().Claims.Select(claims => new
            {
                // Selects claim properties
                claims.Issuer,
                claims.OriginalIssuer,
                claims.Type,
                claims.Value
            });
            // return Json(claims);
            return RedirectToAction("dashboard");
        }

        public async Task<IActionResult> Logout()
        {
            // Signout the current user
            await HttpContext.SignOutAsync();
            return RedirectToAction("index");
        }


    }
}