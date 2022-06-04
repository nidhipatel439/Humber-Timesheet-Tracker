﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Humber_Timesheet_Tracker.Models;
using Humber_Timesheet_Tracker.Models.ViewModels;

namespace Humber_Timesheet_Tracker.Controllers
{
    public class CourseController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CourseController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44375/api/");
        }



        // GET: Course/List
        public ActionResult List()
        {
            //curl https://localhost:44375/api/CourseData/ListCourses

            string url = "CourseData/ListCourses";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CourseDto> courses = response.Content.ReadAsAsync<IEnumerable<CourseDto>>().Result;

            return View(courses);
        }

        // GET: Course/Details/5
        public ActionResult Details(int id)
        {
            //curl https://localhost:44375/api/CourseData/FindCourse/{id}

            DetailsCourse ViewModel = new DetailsCourse();

            string url = "CourseData/FindCourse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CourseDto SelectedCourse = response.Content.ReadAsAsync<CourseDto>().Result;

            ViewModel.SelectedCourse = SelectedCourse; 

            //showcase information about coursetask related to this course
            url = "CourseTaskData/ListCourseTasksForCourse/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CourseTaskDto> RelatedCourseTasks = response.Content.ReadAsAsync<IEnumerable<CourseTaskDto>>().Result;

            ViewModel.RelatedCourseTasks = RelatedCourseTasks;

            //show associated teachers with this course
            url = "TeacherData/ListTeachersForCourse/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<TeacherDto> ResponsibleTeachers = response.Content.ReadAsAsync<IEnumerable<TeacherDto>>().Result;

            ViewModel.ResponsibleTeachers = ResponsibleTeachers;

            url = "TeacherData/ListTeachersNotCaringForCourse/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<TeacherDto> AvailableTeachers = response.Content.ReadAsAsync<IEnumerable<TeacherDto>>().Result;

            ViewModel.AvailableTeachers = AvailableTeachers;
            return View(ViewModel);
        }

        //POST: Course/Associate/{id}
        [HttpPost]
        public ActionResult Associate(int id, int TeacherId)
        {
            
           //call our api to associate course with teacher
            string url = "CourseData/AssociateCourseWithTeacher/" + id + "/" + TeacherId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //Get: Course/UnAssociate/{id}?TeacherId={TeacherId}
        [HttpGet]
        public ActionResult UnAssociate(int id, int TeacherId)
        {
            
            //call our api to associate course with teacher
            string url = "CourseData/UnAssociateCourseWithTeacher/" + id + "/" + TeacherId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //GET: Course/Error
        public ActionResult Error()
        {
            return View();
        }
        // GET: Course/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        public ActionResult Create(Course course)
        {
            //curl -d @course.json -H "Content-Type:application/json" https://localhost:44375/api/CourseData/AddCourse
            string url = "CourseData/AddCourse";

            string jsonpayload = jss.Serialize(course);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int id)
        {
            //curl https://localhost:44375/api/CourseData/FindCourse/{id}

            string url = "CourseData/FindCourse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CourseDto selectedcourse = response.Content.ReadAsAsync<CourseDto>().Result;
            return View(selectedcourse);
        }

        // POST: Course/Update/5
        [HttpPost]
        public ActionResult Update(int id, Course course)
        {
            //curl -d @course.json -H "Content-Type:application/json" https://localhost:44375/api/CourseData/UpdateCourse/{id}
            string url = "CourseData/UpdateCourse/" + id;

            string jsonpayload = jss.Serialize(course);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new {id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Course/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            //curl https://localhost:44375/api/CourseData/FindCourse/{id}

            string url = "CourseData/FindCourse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CourseDto selectedcourse = response.Content.ReadAsAsync<CourseDto>().Result;
            return View(selectedcourse);
        }

        // POST: Course/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Course course)
        {
            string url = "CourseData/DeleteCourse/" + id;
            string jsonpayload = jss.Serialize(course);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
