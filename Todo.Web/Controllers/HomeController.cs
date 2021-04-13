using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Todo.Web.Controllers
{
    public class HomeController : Controller
    {        
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(UserManager<IdentityUser> userManager) => _userManager = userManager;


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "List");

            return View();
        }
    }
}
