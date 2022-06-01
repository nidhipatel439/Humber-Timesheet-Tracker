using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Humber_Timesheet_Tracker.Models;

namespace Humber_Timesheet_Tracker.Controllers
{
    public class CourseDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CourseData/ListCourses
        [ResponseType(typeof(CourseDto))]
        [HttpGet]
        public IHttpActionResult ListCourses()
        {
            List<Course> Courses = db.Courses.ToList();
            List<CourseDto> CourseDtos =new List<CourseDto>();

            Courses.ForEach(a => CourseDtos.Add(new CourseDto()
            {
                CourseId = a.CourseId,
                CourseName = a.CourseName
            }));

            return Ok(CourseDtos);
        }

        // GET: api/CourseData/FindCourse/5
        [ResponseType(typeof(CourseDto))]
        [HttpGet]
        public IHttpActionResult FindCourse(int id)
        {
            Course Courses = db.Courses.Find(id);
            CourseDto CourseDto = new CourseDto()
            {
                CourseId = Courses.CourseId,
                CourseName = Courses.CourseName
            };
            if (Courses == null)
            {
                return NotFound();
            }

            return Ok(CourseDto);
        }

        // POST: api/CourseData/UpdateCourse/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCourse(int id, Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.CourseId)
            {
                return BadRequest();
            }

            db.Entry(course).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/CourseData/AddCourse
        [ResponseType(typeof(Course))]
        [HttpPost]
        public IHttpActionResult AddCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Courses.Add(course);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = course.CourseId }, course);
        }

        // DELETE: api/CourseData/DeleteCourse/5
        [ResponseType(typeof(Course))]
        [HttpPost]
        public IHttpActionResult DeleteCourse(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            db.Courses.Remove(course);
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

        private bool CourseExists(int id)
        {
            return db.Courses.Count(e => e.CourseId == id) > 0;
        }
    }
}