﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Humber_Timesheet_Tracker.Models.ViewModels
{
    public class DetailsTeacher
    {
        public TeacherDto SelectedTeacher { get; set; }
        public IEnumerable<CourseDto> KeptCourses { get; set; }
    }
}