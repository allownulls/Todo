using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Todo.Services;
using Todo.Model;
using Todo.Core;
using Todo.Data;
using System.Linq;
using System.Collections.Generic;
using System;
using Moq;
using System.Threading;
using System.Threading.Tasks;

namespace Todo.Test
{
    [TestClass]
    public class TaskServiceTest
    {
        [TestMethod]
        public async Task General()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "TodoDB")
           .Options;

            using (var db = new ApplicationDbContext(options))
            {
                var listService = new ListService(db);
                var taskService = new TaskService(db);

                var userId = Guid.NewGuid().ToString();
                string listName = "Testname";
                string listName2 = "Testname";

                string taskDesc1 = "Test task 1";
                string taskDesc2 = "Test task 2";

                var tasklist = await listService.AddList(new ListModel(){ OwnerId = userId, Name = listName });
                var tasklist2 = await listService.AddList(new ListModel() { OwnerId = userId, Name = listName2 });

                var tasks = await taskService.GetTasks(tasklist.Id);
                Assert.AreEqual(tasks.Count(), 0);

                var taskModel = new TaskModel()
                {
                    HeaderId = tasklist.Id,
                    Description = taskDesc1
                };

                taskModel = await taskService.AddTask(taskModel);
                var taskUpdated = await taskService.GetTask(taskModel.Id);

                Assert.AreEqual(taskModel.Description, taskUpdated.Description);                
                Assert.AreEqual(taskModel.Checked, taskUpdated.Checked);

                tasks = await taskService.GetTasks(tasklist2.Id);
                Assert.AreEqual(tasks.Count(), 0);
                tasks = await taskService.GetTasks(tasklist.Id);
                Assert.AreEqual(tasks.Count(), 1);

                taskModel.Description = taskDesc2;
                taskModel = await taskService.AddTask(taskModel);

                tasks = await taskService.GetTasks(tasklist2.Id);
                Assert.AreEqual(tasks.Count(), 0);
                tasks = await taskService.GetTasks(tasklist.Id);
                Assert.AreEqual(tasks.Count(), 2);

                taskModel.Checked = true;

                var taskOld = await taskService.GetTask(taskModel.Id);
                taskModel = await taskService.UpdateTask(taskModel);
                taskUpdated = await taskService.GetTask(taskModel.Id);
                
                Assert.AreEqual(taskModel.Description, taskUpdated.Description);
                Assert.AreNotEqual(taskOld.Checked, taskUpdated.Checked);
                Assert.IsTrue(taskUpdated.Checked);
                Assert.IsTrue(DateTime.UtcNow - taskUpdated.LastUpdated < new TimeSpan(0,0,1));

                taskModel.Checked = false;
                taskOld = await taskService.GetTask(taskModel.Id);
                taskModel = await taskService.UpdateTask(taskModel);
                taskUpdated = await taskService.GetTask(taskModel.Id);

                Assert.AreEqual(taskModel.Description, taskUpdated.Description);
                Assert.AreNotEqual(taskOld.Checked, taskUpdated.Checked);
                Assert.IsFalse(taskUpdated.Checked);
                Assert.IsTrue(DateTime.UtcNow - taskUpdated.LastUpdated < new TimeSpan(0, 0, 1));

                tasks = await taskService.GetTasks(tasklist.Id);

                await taskService.DeleteTask(tasks.First().Id);
                tasks = await taskService.GetTasks(tasklist2.Id);
                Assert.AreEqual(tasks.Count(), 0);
                tasks = await taskService.GetTasks(tasklist.Id);
                Assert.AreEqual(tasks.Count(), 1);

                await taskService.DeleteTask(tasks.First().Id);
                tasks = await taskService.GetTasks(tasklist2.Id);
                Assert.AreEqual(tasks.Count(), 0);
                tasks = await taskService.GetTasks(tasklist.Id);
                Assert.AreEqual(tasks.Count(), 0);

            }
        }
    }
}

