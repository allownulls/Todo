using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Data
{
    public class TodoLine
    {
        public Guid Id { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Checked { get; set; }

        public TodoHeader Header { get; set; }
    }
}
