using Todo.Model;
using Todo.Data;
using Todo.Model.ManageViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Threading.Tasks;

namespace Todo.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly string _externalCookieScheme;        

        public ManageController(
          UserManager<IdentityUser> userManager,
          SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = IdentityConstants.ExternalScheme;            
        }

        //
        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
            };
            return View(model);
        }

        ////
        //// POST: /Manage/RemoveLogin
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
        //{
        //    ManageMessageId? message = ManageMessageId.Error;
        //    var user = await GetCurrentUserAsync();
        //    if (user != null)
        //    {
        //        var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
        //        if (result.Succeeded)
        //        {
        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            message = ManageMessageId.RemoveLoginSuccess;
        //        }
        //    }
        //    return RedirectToAction(nameof(ManageLogins), new { Message = message });
        //}



        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);                    
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        ////
        //// GET: /Manage/SetPassword
        //[HttpGet]
        //public IActionResult SetPassword()
        //{
        //    return View();
        //}

        ////
        //// POST: /Manage/SetPassword
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = await GetCurrentUserAsync();
        //    if (user != null)
        //    {
        //        var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
        //        if (result.Succeeded)
        //        {
        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
        //        }
        //        AddErrors(result);
        //        return View(model);
        //    }
        //    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        //}

        ////GET: /Manage/ManageLogins
        //[HttpGet]
        //public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
        //{
        //    ViewData["StatusMessage"] =
        //        message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
        //        : message == ManageMessageId.AddLoginSuccess ? "The external login was added."
        //        : message == ManageMessageId.Error ? "An error has occurred."
        //        : "";
        //    var user = await GetCurrentUserAsync();
        //    if (user == null)
        //    {
        //        return View("Error");
        //    }
        //    var userLogins = await _userManager.GetLoginsAsync(user);            
        //    ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
        //    return View(new ManageLoginsViewModel
        //    {
        //        CurrentLogins = userLogins                
        //    });
        //}

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        private Task<IdentityUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}
