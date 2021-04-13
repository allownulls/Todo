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
    public class ListServiceTest
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

                var userId = Guid.NewGuid().ToString();
                var anotherUserId = Guid.NewGuid().ToString();

                string Name1 = "Testname";
                string Name2 = "Testname2";

                var model = new ListModel()
                {
                    OwnerId = userId,
                    Name = Name1
                };

                await listService.AddList(model);

                var lists = await listService.GetLists(anotherUserId);
                Assert.AreEqual(lists.Count(), 0);
                
                lists = await listService.GetLists(userId);
                Assert.AreEqual(lists.Count(), 1);

                model.Name = Name2;
                await listService.AddList(model);

                lists = await listService.GetLists(anotherUserId);
                Assert.AreEqual(lists.Count(), 0);

                lists = await listService.GetLists(userId);
                Assert.AreEqual(lists.Count(), 2);

                var list = await listService.GetList(lists.First().Id);                
                Assert.IsTrue(list != null);                                

                await listService.DeleteList(lists.First().Id);
                lists = await listService.GetLists(anotherUserId);
                Assert.AreEqual(lists.Count(), 0);

                lists = await listService.GetLists(userId);
                Assert.AreEqual(lists.Count(), 1);

                await listService.DeleteList(lists.First().Id);
                lists = await listService.GetLists(anotherUserId);
                Assert.AreEqual(lists.Count(), 0);

                lists = await listService.GetLists(userId);
                Assert.AreEqual(lists.Count(), 0);
            }
        }
    }
}
