using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971proj.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int CourseId { get; set; }
        public int TermId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Obj1 { get; set; }
        public DateTime Obj1_DueDate { get; set; }
        public string Obj2 { get; set; }
        public DateTime Obj2_DueDate { get; set; }
        public string Status { get; set; }
        public bool AlertSE { get; set; }
        public bool AlertObj { get; set; }
        public string OptionalNotes { get; set; }
        public string CI_Name { get; set; }
        public string CI_Phone { get; set; }
        public string CI_Email { get; set; }
    }
}
