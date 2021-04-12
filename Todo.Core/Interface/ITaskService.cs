using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Model;

namespace Todo.Core
{
    public interface ITaskService
    {
        public Task<IEnumerable<TaskModel>> GetTasks(Guid id);

        public Task<TaskModel> GetTask(Guid id);

        public Task<TaskModel> AddTask(TaskModel taskModel);

        public Task DeleteTask(Guid id);

        public Task<TaskModel> UpdateTask(TaskModel taskModel);
    }
}
