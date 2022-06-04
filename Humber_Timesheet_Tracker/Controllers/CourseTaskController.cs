using Humber_Timesheet_Tracker.Models;
using Humber_Timesheet_Tracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Humber_Timesheet_Tracker.Controllers
{
    public class CourseTaskController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();


        static CourseTaskController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44375/api/");
        }
        // GET: CourseTask/List
        public ActionResult List()
        {
            //curl https://localhost:44375/api/CourseTaskData/ListCourseTasks
            string url = "CourseTaskData/ListCourseTasks";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CourseTaskDto> coursetasks = response.Content.ReadAsAsync<IEnumerable<CourseTaskDto>>().Result;
            
            return View(coursetasks);
        }

        // GET: CourseTask/Details/5
        public ActionResult Details(int id)
        {
            //curl https://localhost:44375/api/CourseTaskData/FindCourseTask/{id}
            string url = "CourseTaskData/FindCourseTask/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CourseTaskDto selectedcoursetasks = response.Content.ReadAsAsync<CourseTaskDto>().Result;

            return View(selectedcoursetasks);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: CourseTask/New
        public ActionResult New()
        {
            //information about all courses in the system
            //GET: api/CourseData/ListCourses

            string url = "CourseData/ListCourses";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CourseDto> CourseOptions = response.Content.ReadAsAsync<IEnumerable<CourseDto>>().Result;

            return View(CourseOptions);
        }

        // POST: CourseTask/Create
        [HttpPost]
        public ActionResult Create(CourseTask coursetask)
        {
            //curl -H "Content-Type:application/json" -d @coursetask.json https://localhost:44375/api/CourseTaskData/AddCourseTask
            string url = "CourseTaskData/AddCourseTask";

            string jsonpayload = jss.Serialize(coursetask);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response= client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details","Course",new { id = coursetask.CourseId });
            }else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: CourseTask/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateCourseTask ViewModel = new UpdateCourseTask(); 

            //the existing coursetask information
            string url = "CourseTaskData/FindCourseTask/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CourseTaskDto SelectedCourseTask = response.Content.ReadAsAsync<CourseTaskDto>().Result;
            ViewModel.SelectedCourseTask = SelectedCourseTask;

            // all courses to choose from when updating this coursetask
            url = "CourseData/ListCourses";
            response = client.GetAsync(url).Result;
            IEnumerable<CourseDto> CourseOptions = response.Content.ReadAsAsync<IEnumerable<CourseDto>>().Result;
            ViewModel.CourseOptions = CourseOptions;
            return View(ViewModel); 
        }

        // POST: CourseTask/Update/5
        [HttpPost]
        public ActionResult Update(int id, CourseTask coursetask)
        {
            //curl -d @course.json -H "Content-Type:application/json" https://localhost:44375/api/CourseData/UpdateCourse/{id}

            string url = "CourseTaskData/UpdateCourseTask/" + id;

            string jsonpayload = jss.Serialize(coursetask);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", "Course", new { id = coursetask.CourseId });
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: CourseTask/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "CourseTaskData/FindCourseTask/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CourseTaskDto selectedcoursetask = response.Content.ReadAsAsync<CourseTaskDto>().Result;
            return View(selectedcoursetask);
        }

        // POST: CourseTask/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, CourseTask coursetask)
        {
            string url = "CourseTaskData/DeleteCourseTask/" + id;
            string jsonpayload = jss.Serialize(coursetask);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                // return RedirectToAction("Error");
                return RedirectToAction("Details", "Course" , new {id = coursetask.CourseId});
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
