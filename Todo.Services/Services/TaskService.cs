using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Todo.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Core;
using Todo.Model;


namespace Todo.Services
{
    public class TaskService: ITaskService
    {
        protected readonly ApplicationDbContext _db;

        public TaskService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TaskModel>> GetTasks(Guid id)
        {
            return await _db.TodoLines.Where(e => e.Header.Id == id)
                .Select(e => new TaskModel() { 
                    Id = e.Id,
                    HeaderId = e.Header.Id,
                    Name = e.Name,
                    Description = e.Description,
                    LastUpdated = e.LastUpdated,
                    Checked = e.Checked
                }).ToListAsync();
        }

        public async Task<TaskModel> GetTask(Guid id)
        {
            var tl = await _db.TodoLines.FindAsync(id);

            return new TaskModel()
            {
                Id = tl.Id,
                Description = tl.Description,
                LastUpdated = tl.LastUpdated,
                Name = tl.Name,
                Checked = tl.Checked                
            };
        }

        public async Task<TaskModel> AddTask(TaskModel taskModel)
        {
            var th = await _db.TodoHeaders                              
                              .FindAsync(taskModel.HeaderId);

            var task = new TodoLine()
            {
                Header = th,
                Description = taskModel.Description,
                Name = taskModel.Name,
                LastUpdated = DateTime.UtcNow
            };

            _db.TodoLines.Add(task);            

            await _db.SaveChangesAsync();

            taskModel.Id = task.Id;
            taskModel.LastUpdated = task.LastUpdated;

            return taskModel;
        }

        public async Task DeleteTask(Guid id)
        {
            var tl = await _db.TodoLines.FindAsync(id);

            _db.TodoLines.Remove(tl);
            await _db.SaveChangesAsync();
        }
        
        public async Task<TaskModel> UpdateTask(TaskModel taskModel)
        {
            var tl = await _db.TodoLines.FindAsync(taskModel.Id);
            
            tl.LastUpdated = DateTime.UtcNow;            
            tl.Checked = taskModel.Checked;
                        
            await _db.SaveChangesAsync();

            taskModel.LastUpdated = tl.LastUpdated;

            return taskModel;
        }
    }
}
