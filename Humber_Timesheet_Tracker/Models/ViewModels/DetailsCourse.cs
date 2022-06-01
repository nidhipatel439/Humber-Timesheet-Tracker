using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Humber_Timesheet_Tracker.Models.ViewModels
{
    public class DetailsCourse
    {
        public CourseDto SelectedCourse { get; set; }
        public IEnumerable<CourseTaskDto> RelatedCourseTasks { get; set; }
    }
}