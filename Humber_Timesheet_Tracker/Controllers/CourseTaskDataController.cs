using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Humber_Timesheet_Tracker.Models;

namespace Humber_Timesheet_Tracker.Controllers
{
    public class CourseTaskDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CourseTaskData/ListCourseTasks
        [ResponseType(typeof(CourseTaskDto))]
        [HttpGet]
        public IHttpActionResult ListCourseTasks()
        {
            List<CourseTask> CourseTasks = db.CourseTasks.ToList();
            List<CourseTaskDto> CourseTaskDtos = new List<CourseTaskDto>();

            CourseTasks.ForEach(a => CourseTaskDtos.Add(new CourseTaskDto()
            {
                CourseTaskId = a.CourseTaskId,
                CourseTaskName = a.CourseTaskName,
                CourseTaskTime = a.CourseTaskTime,
                CourseName = a.Course.CourseName
            }));
            return Ok(CourseTaskDtos);
            
        }

        // GET: api/CourseTaskData/FindCourseTask/5
        [ResponseType(typeof(CourseTask))]
        [HttpGet]
        public IHttpActionResult FindCourseTask(int id)
        {
            CourseTask CourseTasks = db.CourseTasks.Find(id);
            CourseTaskDto CourseTaskDtos = new CourseTaskDto()
            {
                CourseTaskId = CourseTasks.CourseTaskId,
                CourseTaskName = CourseTasks.CourseTaskName,
                CourseTaskTime = CourseTasks.CourseTaskTime,
                CourseName = CourseTasks.Course.CourseName
            };
            if (CourseTasks == null)
            {
                return NotFound();
            }

            return Ok(CourseTaskDtos);
        }

        // POST: api/CourseTaskData/UpdateCourseTask/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCourseTask(int id, CourseTask courseTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != courseTask.CourseTaskId)
            {
                return BadRequest();
            }

            db.Entry(courseTask).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CourseTaskData/AddCourseTask
        [ResponseType(typeof(CourseTask))]
        [HttpPost]
        public IHttpActionResult AddCourseTask(CourseTask courseTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CourseTasks.Add(courseTask);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = courseTask.CourseTaskId }, courseTask);
        }

        // DELETE: api/CourseTaskData/DeleteCourseTask/5
        [ResponseType(typeof(CourseTask))]
        [HttpPost]
        public IHttpActionResult DeleteCourseTask(int id)
        {
            CourseTask courseTask = db.CourseTasks.Find(id);
            if (courseTask == null)
            {
                return NotFound();
            }

            db.CourseTasks.Remove(courseTask);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseTaskExists(int id)
        {
            return db.CourseTasks.Count(e => e.CourseTaskId == id) > 0;
        }
    }
}