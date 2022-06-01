using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Humber_Timesheet_Tracker.Models.ViewModels
{
    public class UpdateCourseTask
    {
        //the existing coursetask information
        public CourseTaskDto SelectedCourseTask { get; set; }

        //all courses to choose from when updating this coursetask
        public IEnumerable<CourseDto> CourseOptions { get; set; }
    }
}