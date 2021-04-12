using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Todo.Core;
using Todo.Services;
using Todo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Todo.Data;
using Microsoft.AspNetCore.Authorization;

namespace Todo.Controllers
{
    [Authorize]
    public class ListController : Controller
    {
        protected IListService _listService;
        private readonly UserManager<IdentityUser> _userManager;

        public ListController(IListService listService, UserManager<IdentityUser> userManager)
        {            
            _listService = listService;
            _userManager = userManager;
        }

        
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = await _listService.GetLists(user.Id);
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddList(string name)
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new ListModel
            {
                Name = name,
                OwnerId = user.Id
            };            
            
            model = await _listService.AddList(model);

            return Ok(model);
        }

        public async Task<IActionResult> DeleteList(Guid id)
        {
            await _listService.DeleteList(id);

            return Ok();
        }
    }
}
