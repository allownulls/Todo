using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

namespace Todo.Web.Controllers
{
    public class HomeController : Controller
    {        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SecretClient _secretClient;

        public HomeController(UserManager<IdentityUser> userManager, SecretClient secretClient)
        {
            _userManager = userManager;
            _secretClient = secretClient;

        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            _secretClient.GetSecret("DefaultConnection");

            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "List");

            return View();
        }
    }
}
