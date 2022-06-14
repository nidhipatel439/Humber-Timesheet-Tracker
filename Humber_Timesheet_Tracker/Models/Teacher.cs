using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Humber_Timesheet_Tracker.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }

        public bool TeacherHasPic { get; set; }
        public string PicExtension { get; set; }

        //A teacher can teach many courses
        public ICollection<Course> Courses { get; set; }
    }

    public class TeacherDto
    {
        public int TeacherId { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }

        public bool TeacherHasPic { get; set; }
        public string PicExtension { get; set; }

    }


}