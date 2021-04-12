using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Core;
using Todo.Model;
using Todo.Services;
using Microsoft.AspNetCore.Authentication;
using Todo.Data;
using Microsoft.AspNetCore.Authorization;

namespace Todo.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        ITaskService _taskService;
        IListService _listService;

        public TaskController(ITaskService taskService,
                              IListService listService)
        {
            _taskService = taskService;
            _listService = listService;
        }


        public async Task<IActionResult> Index(Guid id)
        {
            var model = await _listService.GetList(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] TaskModel taskModel)
        {
            var model = await _taskService.AddTask(taskModel);

            return Ok(model);
        }

        public async Task<IActionResult> DeleteTask(Guid id)
        {
            await _taskService.DeleteTask(id);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTask([FromBody] TaskModel taskModel)
        {
            var model = await _taskService.UpdateTask(taskModel);
            
            return Ok(model);
        }
    }
}

