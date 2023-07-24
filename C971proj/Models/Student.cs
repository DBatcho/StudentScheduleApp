using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971proj.Models
{
    public class Student
    {
        [PrimaryKey, AutoIncrement]
        public int StudentId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string StudentName { get; set; }
    }
}
