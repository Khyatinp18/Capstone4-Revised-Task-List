using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Capestone4TaskListRevisited.Models
{
    public partial class Tasks
    {
        public string TaskDescription { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
        public string TaskOwnerId { get; set; }
        public int Id { get; set; }

        public virtual AspNetUsers TaskOwner { get; set; }
    }
}
