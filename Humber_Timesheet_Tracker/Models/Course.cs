using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Humber_Timesheet_Tracker.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        public string CourseName { get; set; }

        //a course can be taken care of by many teachers
        public ICollection<Teacher> Teachers { get; set; }
    }

    public class CourseDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }

    }
}