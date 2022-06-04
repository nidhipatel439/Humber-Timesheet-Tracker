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
    public class TeacherDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        /// <summary>
        /// Returns all teachers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all teachers in the database.
        /// </returns>
        /// <example>
        /// GET: api/TeacherData/ListTeachers
        /// </example>
        [HttpGet]
        [ResponseType(typeof(TeacherDto))]
        public IHttpActionResult ListTeachers()
        {
            List<Teacher> Teachers = db.Teachers.ToList();
            List<TeacherDto> TeacherDtos = new List<TeacherDto>();

            Teachers.ForEach(t => TeacherDtos.Add(new TeacherDto()
            {
                TeacherId = t.TeacherId,
                TeacherFirstName = t.TeacherFirstName,
                TeacherLastName = t.TeacherLastName
            }));

            return Ok(TeacherDtos);
        }


        /// <summary>
        /// Returns all teachers in the system associated with a particular course.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all teachers in the database taking care of a particular course
        /// </returns>
        /// <param name="id">course Primary Key</param>
        /// <example>
        /// GET: api/TeacherData/ListTeachersForCourse/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(TeacherDto))]
        public IHttpActionResult ListTeachersForCourse(int id)
        {
            List<Teacher> Teachers = db.Teachers.Where(
                t => t.Courses.Any(
                    a => a.CourseId == id)
                ).ToList();
            List<TeacherDto> TeacherDtos = new List<TeacherDto>();

            Teachers.ForEach(t => TeacherDtos.Add(new TeacherDto()
            {
                TeacherId = t.TeacherId,
                TeacherFirstName = t.TeacherFirstName,
                TeacherLastName = t.TeacherLastName
            }));

            return Ok(TeacherDtos);
        }



        /// <summary>
        /// Returns teachers in the system not taking for a particular course.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all teachers in the database not taking care of a particular course
        /// </returns>
        /// <param name="id">course Primary Key</param>
        /// <example>
        /// GET: api/TeacherData/ListTeachersNotCaringForCourse/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(TeacherDto))]
        public IHttpActionResult ListTeachersNotCaringForCourse(int id)
        {
            List<Teacher> Teachers = db.Teachers.Where(
                t => !t.Courses.Any(
                    a => a.CourseId == id)
                ).ToList();
            List<TeacherDto> TeacherDtos = new List<TeacherDto>();

            Teachers.ForEach(t => TeacherDtos.Add(new TeacherDto()
            {
                TeacherId = t.TeacherId,
                TeacherFirstName = t.TeacherFirstName,
                TeacherLastName = t.TeacherLastName
            }));

            return Ok(TeacherDtos);
        }


        /// <summary>
        /// Returns all teachers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A teacher in the system matching up to the techer ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the teacher</param>
        /// <example>
        /// GET: api/TeacherData/FindTeacher/5
        /// </example>
        [ResponseType(typeof(TeacherDto))]
        [HttpGet]
        public IHttpActionResult FindTeacher(int id)
        {
            Teacher Teacher = db.Teachers.Find(id);
            TeacherDto TeacherDto = new TeacherDto()
            {
                TeacherId = Teacher.TeacherId,
                TeacherFirstName = Teacher.TeacherFirstName,
                TeacherLastName = Teacher.TeacherLastName
            };
            if (Teacher == null)
            {
                return NotFound();
            }

            return Ok(TeacherDto);
        }




        /// <summary>
        /// Updates a particular teacher in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the teacher ID primary key</param>
        /// <param name="Teacher">JSON FORM DATA of an teacher</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/TeacherData/UpdateTeacher/5
        /// FORM DATA: teacher JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTeacher(int id, Teacher Teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Teacher.TeacherId)
            {
                return BadRequest();
            }

            db.Entry(Teacher).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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
        /// Adds a teacher to the system
        /// </summary>
        /// <param name="teacher">JSON FORM DATA of a teacher</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: teacher ID, teacher Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/TeacherData/AddTeacher
        /// FORM DATA: teacher JSON Object
        /// </example>

        [ResponseType(typeof(Teacher))]
        [HttpPost]
        public IHttpActionResult AddTeacher(Teacher Teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teachers.Add(Teacher);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Teacher.TeacherId }, Teacher);
        }



        /// <summary>
        /// Deletes a teacher from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the teacher</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/TeacherData/DeleteTeacher/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Teacher))]
        [HttpPost]
        public IHttpActionResult DeleteTeacher(int id)
        {
            Teacher Teacher = db.Teachers.Find(id);
            if (Teacher == null)
            {
                return NotFound();
            }

            db.Teachers.Remove(Teacher);
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

        private bool TeacherExists(int id)
        {
            return db.Teachers.Count(e => e.TeacherId == id) > 0;
        }
    }
}