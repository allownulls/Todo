using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Data
{
    public class TodoHeader
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public ICollection<TodoLine> Lines { get; set; }
    }
}
