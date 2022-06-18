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


        /// <summary>
        /// Returns all courses in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all courses in the database.
        /// </returns>
        /// <example>
        /// GET: api/CourseData/ListCourses
        /// </example>
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


        /// <summary>
        /// Returns all courses in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A course in the system matching up to the course ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the course</param>
        /// <example>
        /// GET: api/CourseData/FindCourse/5
        /// </example>
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




        /// <summary>
        /// Gathers information about courses related to a particular teacher
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all courses in the database, including their associated details that match to a particular teacher id
        /// </returns>
        /// <param name="id">Teacher ID.</param>
        /// <example>
        /// GET: api/CourseData/ListCoursesForTeacher/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CourseDto))]
        public IHttpActionResult ListCoursesForTeacher(int id)
        {
            //all courses that have teachers which match with our ID
            List<Course> Courses = db.Courses.Where(
                a => a.Teachers.Any(
                    t => t.TeacherId == id
                )).ToList();
            List<CourseDto> CourseDtos = new List<CourseDto>();

            Courses.ForEach(a => CourseDtos.Add(new CourseDto()
            {
                CourseId = a.CourseId,
                CourseName = a.CourseName
            }));

            return Ok(CourseDtos);
        }



        /// <summary>
        /// Associates a particular teacher with a particular course
        /// </summary>
        /// <param name="courseid">The course ID primary key</param>
        /// <param name="teacherid">The teacher ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/CourseData/AssociateCourseWithTeacher/9/1
        /// </example>
        [HttpPost]
        [Route("api/CourseData/AssociateCourseWithTeacher/{courseid}/{teacherid}")]
        [Authorize]
        public IHttpActionResult AssociateCourseWithTeacher(int courseid, int teacherid)
        {

            Course SelectedCourse = db.Courses.Include(a => a.Teachers).Where(a => a.CourseId == courseid).FirstOrDefault();
            Teacher SelectedTeacher = db.Teachers.Find(teacherid);

            if (SelectedCourse == null || SelectedTeacher == null)
            {
                return NotFound();
            }


            SelectedCourse.Teachers.Add(SelectedTeacher);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular teacher and a particular course
        /// </summary>
        /// <param name="courseid">The course ID primary key</param>
        /// <param name="teacherid">The teacher ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/CourseData/UnAssociateCourseWithTeacher/9/1
        /// </example>
        [HttpPost]
        [Route("api/CourseData/UnAssociateCourseWithTeacher/{courseid}/{teacherid}")]
        [Authorize]
        public IHttpActionResult UnAssociateCourseWithTeacher(int courseid, int teacherid)
        {

            Course SelectedCourse = db.Courses.Include(a => a.Teachers).Where(a => a.CourseId == courseid).FirstOrDefault();
            Teacher SelectedTeacher = db.Teachers.Find(teacherid);

            if (SelectedCourse == null || SelectedTeacher == null)
            {
                return NotFound();
            }


            SelectedCourse.Teachers.Remove(SelectedTeacher);
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Updates a particular course in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the course ID primary key</param>
        /// <param name="course">JSON FORM DATA of a course</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CourseData/UpdateCourse/5
        /// FORM DATA: course JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
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


        /// <summary>
        /// Adds a course to the system
        /// </summary>
        /// <param name="course">JSON FORM DATA of a course</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: course ID, course Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/CourseData/AddCourse
        /// FORM DATA: course JSON Object
        /// </example>
        [ResponseType(typeof(Course))]
        [HttpPost]
        [Authorize]
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



        /// <summary>
        /// Deletes a course from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the course</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/CourseData/DeleteCourse/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Course))]
        [HttpPost]
        [Authorize]
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