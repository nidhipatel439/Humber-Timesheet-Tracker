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


        /// <summary>
        /// Returns all coursetasks in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all coursetasks in the database, including their associated course.
        /// </returns>
        /// <example>
        /// GET: api/CourseTaskData/ListCourseTasks
        /// </example>
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
                CourseName = a.Course.CourseName,
                CourseId = a.Course.CourseId
            }));
            return Ok(CourseTaskDtos);
            
        }



        // GET: api/CourseTaskData/ListCourseTasksForCourse/{id}
        [ResponseType(typeof(CourseTaskDto))]
        [HttpGet]
        public IHttpActionResult ListCourseTasksForCourse(int id)
        {
            List<CourseTask> CourseTasks = db.CourseTasks.Where(a=> a.CourseId==id).ToList();
            List<CourseTaskDto> CourseTaskDtos = new List<CourseTaskDto>();

            CourseTasks.ForEach(a => CourseTaskDtos.Add(new CourseTaskDto()
            {
                CourseTaskId = a.CourseTaskId,
                CourseTaskName = a.CourseTaskName,
                CourseTaskTime = a.CourseTaskTime,
                CourseName = a.Course.CourseName,
                CourseId = a.Course.CourseId
            }));
            return Ok(CourseTaskDtos);

        }


        /// <summary>
        /// Returns all coursetask in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A coursetask in the system matching up to the coursetask ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the coursetask</param>
        /// <example>
        /// GET: api/CourseTaskData/FindCourseTask/5
        /// </example>
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
                CourseName = CourseTasks.Course.CourseName,
                CourseId = CourseTasks.Course.CourseId
            };
            if (CourseTasks == null)
            {
                return NotFound();
            }

            return Ok(CourseTaskDtos);
        }


        /// <summary>
        /// Updates a particular coursetask in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the coursetask ID primary key</param>
        /// <param name="coursetask">JSON FORM DATA of a coursetask</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CourseTaskData/UpdateCourseTask/5
        /// FORM DATA: coursetask JSON Object
        /// </example>
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


        /// <summary>
        /// Adds a coursetask to the system
        /// </summary>
        /// <param name="coursetask">JSON FORM DATA of a coursetask</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: coursetask ID, coursetask Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/CourseTaskData/AddCourseTask
        /// FORM DATA: coursetask JSON Object
        /// </example>
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


        /// <summary>
        /// Deletes a coursetask from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the coursetask</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/CourseTaskData/DeleteCourseTask/5
        /// FORM DATA: (empty)
        /// </example>
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