using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using C971proj.Models;
using Xamarin.Essentials;

namespace C971proj.Services
{
    public static class DatabaseServices
    {
        private static SQLiteAsyncConnection _db;
        /// <summary>
        /// Initialize Database and Load data if its first time
        /// </summary>
        /// <returns></returns>
        static async Task Init()
        {
            if (_db != null)
            {
                return;
            }

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "Term.db");
            
            _db = new SQLiteAsyncConnection(databasePath);

            await _db.CreateTableAsync<Student>();
            await _db.CreateTableAsync<Term>();
            await _db.CreateTableAsync<Course>();
        }
        #region Student Methods
        private static int? loggedinId;
        private static string LoggedInStudentName;
        /// <summary>
        /// accesses database for student data
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Student>> GetStudent()
        {
            await Init();

            var student = await _db.Table<Student>().ToListAsync();
            return student;
        }
        /// <summary>
        /// Saves the logged in information in variables
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="studentName"></param>
        public static void LoggedIn (int studentId, string studentName)
        {
            loggedinId = studentId;
            LoggedInStudentName = studentName;
        }
        /// <summary>
        /// Removes the logged in information from variables
        /// </summary>
        public static void LoggedOut()
        {
            loggedinId = null;
            LoggedInStudentName = null;
        }
        /// <summary>
        /// Returns the logged in studentsID
        /// </summary>
        /// <returns></returns>
        public static int? GetLogInId()
        {
            return loggedinId;
        }
        /// <summary>
        /// Returns the logged in students name
        /// </summary>
        /// <returns></returns>
        public static string GetStudentName()
        {

            return LoggedInStudentName;
        }
        #endregion
        #region Term Methods
        /// <summary>
        /// Returns Term Information
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Term>> GetTerm()
        {
            await Init();

            var term = await _db.Table<Term>().ToListAsync();
            return term;
        }
        /// <summary>
        /// Adds Term Information to Database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static async Task AddTerm (string name, int? studentId, DateTime start, DateTime end)
        {
            await Init();
            var term = new Term()
            {
                TermName = name,
                StudentId = studentId,
                TermStart = start,
                TermEnd = end
            };

            await _db.InsertAsync(term);
        }
        /// <summary>
        /// Updates Term Information in Database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static async Task UpdateTerm (int id, string name, DateTime start, DateTime end)
        {
            await Init();

            var termQuery = await _db.Table<Term>()
                .Where(i => i.TermId == id)
                .FirstOrDefaultAsync();

            if (termQuery != null)
            {
                termQuery.TermName = name;
                termQuery.TermStart = start;
                termQuery.TermEnd = end;
            }

            await _db.UpdateAsync(termQuery);
        }
        /// <summary>
        /// Removes Term From Database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task RemoveTerm (int id)
        {
            await Init();

            await _db.DeleteAsync<Term>(id);
        }
        #endregion
        #region Course Methods
        /// <summary>
        /// Gets Course Information from Database 
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Course>> GetCourse()
        {
            await Init();

            var course = await _db.Table<Course>().ToListAsync();

            return course;
        }
        /// <summary>
        /// Gets Course(s) Information Based on TermId
        /// </summary>
        /// <param name="termid"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Course>> GetCourse(int termid)
        {
            await Init();

            var course = await _db.Table<Course>().Where(i => i.TermId == termid).ToListAsync();

            return course;
        }
        /// <summary>
        /// Add Course to Database
        /// </summary>
        /// <param name="termid"></param>
        /// <param name="title"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="obj1"></param>
        /// <param name="obj1_duedate"></param>
        /// <param name="obj2"></param>
        /// <param name="obj2_duedate"></param>
        /// <param name="status"></param>
        /// <param name="alertse"></param>
        /// <param name="alertobj"></param>
        /// <param name="sharenotes"></param>
        /// <param name="optionalnotes"></param>
        /// <param name="ci_name"></param>
        /// <param name="ci_phone"></param>
        /// <param name="ci_email"></param>
        /// <returns></returns>
        public static async Task AddCourse(int termid, string title, DateTime startdate, DateTime enddate,
        string obj1, DateTime obj1_duedate,
        string obj2, DateTime obj2_duedate,
        string status, bool alertse, bool alertobj, string optionalnotes,
        string ci_name, string ci_phone, string ci_email)
        {
            await Init();
            var course = new Course()
            {
                TermId = termid,

                Title = title,
                StartDate = startdate,
                EndDate = enddate,

                Obj1 = obj1,
                Obj1_DueDate = obj1_duedate,

                Obj2 = obj2,
                Obj2_DueDate = obj2_duedate,

                Status = status,

                AlertSE = alertse,
                AlertObj = alertobj,
                OptionalNotes = optionalnotes,

                CI_Name = ci_name,
                CI_Phone = ci_phone,
                CI_Email = ci_email
            };

            await _db.InsertAsync(course);
            var id = course.CourseId;
        }
        /// <summary>
        /// Updates a Course's Information in Database
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="termid"></param>
        /// <param name="title"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="obj1"></param>
        /// <param name="obj1_duedate"></param>
        /// <param name="obj2"></param>
        /// <param name="obj2_duedate"></param>
        /// <param name="status"></param>
        /// <param name="alertse"></param>
        /// <param name="alertobj"></param>
        /// <param name="sharenotes"></param>
        /// <param name="optionalnotes"></param>
        /// <param name="ci_name"></param>
        /// <param name="ci_phone"></param>
        /// <param name="ci_email"></param>
        /// <returns></returns>
        public static async Task UpdateCourse(int courseid, int termid, string title, DateTime startdate, DateTime enddate,
            string obj1, DateTime obj1_duedate, 
            string obj2, DateTime obj2_duedate,
            string status, bool alertse, bool alertobj, string optionalnotes,
            string ci_name, string ci_phone, string ci_email)
        {
            await Init();

            var courseQuery = await _db.Table<Course>()
                .Where(i => i.CourseId == courseid)
                .FirstOrDefaultAsync();

            if (courseQuery != null)
            {
                courseQuery.TermId = termid;

                courseQuery.Title = title;
                courseQuery.StartDate = startdate;
                courseQuery.EndDate = enddate;

                courseQuery.Obj1 = obj1;
                courseQuery.Obj1_DueDate = obj1_duedate;
                courseQuery.Obj2 = obj2;
                courseQuery.Obj2_DueDate = obj2_duedate;

                courseQuery.Status = status;

                courseQuery.AlertSE = alertse;
                courseQuery.AlertObj = alertobj;
                courseQuery.OptionalNotes = optionalnotes;

                courseQuery.CI_Name = ci_name;
                courseQuery.CI_Phone = ci_phone;
                courseQuery.CI_Email = ci_email;

                await _db.UpdateAsync(courseQuery);
            }
        }
        /// <summary>
        /// No Longer in Use
        /// Used to Update the Course Dates if User Changes Term Date
        /// </summary>
        /// <param name="termid"></param>
        /// <param name="difference"></param>
        /// <returns></returns>
        //public static async Task UpdateCourseDates(int termid, double difference)
        //{
        //    await Init();

        //    foreach (var course in await DatabaseServices.GetCourse(termid))
        //    {
        //        course.StartDate = course.StartDate.AddDays(difference);
        //        course.EndDate = course.EndDate.AddDays(difference);

        //        course.Obj1_DueDate = course.Obj1_DueDate.AddDays(difference);
        //        course.Obj2_DueDate = course.Obj2_DueDate.AddDays(difference);

        //        await _db.UpdateAsync(course);
        //    }
        //}
        /// <summary>
        /// Removes a Course based on TermId. Used if Term is deleted from Database
        /// </summary>
        /// <param name="termid"></param>
        /// <returns></returns>
        public static async Task RemoveCourseUsingTermId(int termid)
        {
            await Init();

            foreach (var course in await DatabaseServices.GetCourse())
            {
                if (termid == course.TermId)
                {
                    await _db.DeleteAsync<Course>(course.CourseId);
                }
            }            
        }
        /// <summary>
        /// Removes a Course from database
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public static async Task RemoveCourseUsingCourseId(int courseid)
        {
            await Init();
            
            await _db.DeleteAsync<Course>(courseid);               
        }
        #endregion
        #region Sample Data
        /// <summary>
        /// Sample Data to Load
        /// </summary>
        public static async void LoadSampleData()
        {
            await Init();

            Student student1 = new Student
            {
                UserName = "Test",
                Password = "Test",
                StudentName = "Test Student"
            };
            await _db.InsertAsync(student1);

            Term term1 = new Term
            {
                TermName = "Term 1",
                StudentId = student1.StudentId,
                TermStart = DateTime.Now.Date,
                TermEnd = DateTime.Now.Date.AddMonths(6)
            }; 
            await _db.InsertAsync(term1);

            Course course1 = new Course
            {
                TermId = term1.TermId,
                Title = "C100",
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date.AddMonths(3),
                Obj1 = "Test",
                Obj1_DueDate = DateTime.Now.Date.AddMonths(1),
                Obj2 = "Project",
                Obj2_DueDate = DateTime.Now.Date.AddMonths(1),
                Status = "in progress",
                AlertSE = true,
                AlertObj = false,
                OptionalNotes = null,
                CI_Name = "Sing",
                CI_Phone = "1010519861",
                CI_Email = "sing@email.com"
            };
            await _db.InsertAsync(course1);

            Course course2 = new Course
            {
                TermId = term1.TermId,
                Title = "C200",
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date.AddMonths(4),
                Obj1 = "Test",
                Obj1_DueDate = DateTime.Now.Date,
                Obj2 = "Project",
                Obj2_DueDate = DateTime.Now.Date.AddMonths(2),
                Status = "plan to take",
                AlertSE = false,
                AlertObj = true,
                OptionalNotes = "These are shareable notes",
                CI_Name = "Dan",
                CI_Phone = "1082319851",
                CI_Email = "dan@email.com"
            };
            await _db.InsertAsync(course2);

            Course course3 = new Course
            {
                TermId = term1.TermId,
                Title = "C300",
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date.AddMonths(6),
                Obj1 = "Project",
                Obj1_DueDate = DateTime.Now.Date.AddMonths(3),
                Obj2 = "Test",
                Obj2_DueDate = DateTime.Now.Date,
                Status = "dropped",
                AlertSE = false,
                AlertObj = true,
                OptionalNotes = null,
                CI_Name = "Term",
                CI_Phone = "1081619728",
                CI_Email = "tim@email.com"
            };
            await _db.InsertAsync(course3);

            Course course4 = new Course
            {
                TermId = term1.TermId,
                Title = "C400",
                StartDate = DateTime.Now.Date.AddMonths(4),
                EndDate = DateTime.Now.Date.AddMonths(6),
                Obj1 = null,
                Obj1_DueDate = DateTime.Now.Date.AddMonths(4),
                Obj2 = null,
                Obj2_DueDate = DateTime.Now.Date.AddMonths(5),
                Status = "dropped",
                AlertSE = false,
                AlertObj = false,
                OptionalNotes = null,
                CI_Name = "Chelsea",
                CI_Phone = "1092319386",
                CI_Email = "chelsea@email.com"
            };
            await _db.InsertAsync(course4);

            Course course5 = new Course
            {
                TermId = term1.TermId,
                Title = "C500",
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date.AddMonths(6),
                Obj1 = "Project",
                Obj1_DueDate = DateTime.Now.Date.AddMonths(5),
                Obj2 = "Presentation",
                Obj2_DueDate = DateTime.Now.Date.AddMonths(5),
                Status = "plan to take",
                AlertSE = false,
                AlertObj = false,
                OptionalNotes = null,
                CI_Name = "MaryAnn",
                CI_Phone = "1070131953",
                CI_Email = "maryann@email.com"
            };
            await _db.InsertAsync(course5);

            Course course6 = new Course
            {
                TermId = term1.TermId,
                Title = "C600",
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date.AddMonths(6),
                Obj1 = "Quiz",
                Obj1_DueDate = DateTime.Now.Date.AddMonths(4),
                Obj2 = "Test",
                Obj2_DueDate = DateTime.Now.Date.AddMonths(4),
                Status = "in progress",
                AlertSE = false,
                AlertObj = false,
                OptionalNotes = null,
                CI_Name = "Ed",
                CI_Phone = "1112321956",
                CI_Email = "ed@email.com"
            };
            await _db.InsertAsync(course6);

            Term term2 = new Term
            {
                TermName = "Term 2",
                StudentId = student1.StudentId,
                TermStart = DateTime.Now.AddMonths(7),
                TermEnd = DateTime.Now.AddMonths(13)
            };
            await _db.InsertAsync(term2);

            Course course7 = new Course
            {
                TermId = term2.TermId,
                Title = "C101",
                StartDate = DateTime.Now.Date.AddMonths(7),
                EndDate = DateTime.Now.Date.AddMonths(13),
                Obj1 = "Quiz",
                Obj1_DueDate = DateTime.Now.Date.AddMonths(8),
                Obj2 = "Test",
                Obj2_DueDate = DateTime.Now.Date.AddMonths(8),
                Status = "plan to take",
                AlertSE = false,
                AlertObj = false,
                OptionalNotes = null,
                CI_Name = "Eddie",
                CI_Phone = "1052519487",
                CI_Email = "eddie@email.com"
            };
            await _db.InsertAsync(course7);
        }
        /// <summary>
        /// Clears Database of All Data
        /// </summary>
        /// <returns></returns>
        public static async Task ClearAllData()
        {
            await Init();

            await _db.DropTableAsync<Student>();
            await _db.DropTableAsync<Term>();
            await _db.DropTableAsync<Course>();
            LoggedOut();
            _db = null;
        }
        #endregion
    }
}
