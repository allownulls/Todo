using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Model
{
    public class ListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }

        public List<TaskModel> Tasks = new List<TaskModel>();
    }
}
