using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ToDoMVC.Models
{
    public partial class ToDos
    {
        public int Id { get; set; }
        public string Details { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Completed { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
