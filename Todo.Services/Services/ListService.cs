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
    public class ListService : IListService
    {
        protected readonly ApplicationDbContext _db;
        public ListService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<ListModel>> GetLists(string userId)
        {
            return _db.TodoHeaders.Any() ?
                             await _db.TodoHeaders.Where(e => e.OwnerId == userId)
                                                  .Select(e => new ListModel() {
                                                             Id = e.Id,
                                                             Name = e.Name,
                                                             OwnerId = e.OwnerId,
                                                          })
                             .ToListAsync() : new List<ListModel>();
        }

        public async Task<ListModel> GetList(Guid id)
        {
            var th = await _db.TodoHeaders
                              .Include(e => e.Lines)
                              .SingleAsync(e => e.Id == id);

            return new ListModel()
            {
                Id = th.Id,
                Name = th.Name,
                OwnerId = th.OwnerId,
                Tasks = th.Lines.Select(e => new TaskModel()
                {
                    Id = e.Id,
                    HeaderId = e.Header.Id,
                    Description = e.Description,
                    LastUpdated = e.LastUpdated,
                    Name = e.Name,
                    Checked = e.Checked
                })                                      
                .ToList()
            };
        }

        public async Task<ListModel> AddList(ListModel listModel)
        {
            var th = new TodoHeader()
            {
                OwnerId = listModel.OwnerId,
                Name = listModel.Name                
            };

            var lines = listModel.Tasks.Select(e => new TodoLine()
            {
                Description = e.Description,
                LastUpdated = e.LastUpdated,
                Name = e.Name
            }).ToList();

            _db.TodoHeaders.Add(th);
            _db.TodoLines.AddRange(lines);
            await _db.SaveChangesAsync();

            return new ListModel()
            {
                Id = th.Id,
                OwnerId = th.OwnerId,
                Name = th.Name,
                Tasks = lines.Select(e => new TaskModel()
                {
                    Id = e.Id,
                    HeaderId = e.Header.Id,
                    Description = e.Description,
                    LastUpdated = e.LastUpdated
                }).ToList()
            };
        }

        public async Task DeleteList(Guid id)
        {
            var t = await _db.TodoHeaders.Include("Lines").SingleAsync( e => e.Id == id);

            _db.TodoLines.RemoveRange(t.Lines.ToList());            
            _db.TodoHeaders.Remove(t);

            await _db.SaveChangesAsync();
        }
    }
}
