using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Humber_Timesheet_Tracker.Models
{
    public class CourseTask
    {
        [Key]
        public int CourseTaskId { get; set; }
        public string CourseTaskName { get; set; }
        //coursetasktime in seconds
        public int CourseTaskTime { get; set; }

        [ForeignKey ("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

    }

    public class CourseTaskDto
    {
        public int CourseTaskId { get; set; }
        public string CourseTaskName { get; set; }
        public int CourseTaskTime { get; set; }

        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
}