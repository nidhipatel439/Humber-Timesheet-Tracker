using Humber_Timesheet_Tracker.Models;
using Humber_Timesheet_Tracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Humber_Timesheet_Tracker.Controllers
{
    public class TeacherController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static TeacherController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44375/api/");
        }

        // GET: Teacher/List
        public ActionResult List()
        {
            //curl https://localhost:44375/api/Teacherdata/listteachers

            string url = "TeacherData/ListTeachers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<TeacherDto> Teachers = response.Content.ReadAsAsync<IEnumerable<TeacherDto>>().Result;

            return View(Teachers);
        }

        // GET: Teacher/Details/5
        public ActionResult Details(int id)
        {
            DetailsTeacher ViewModel = new DetailsTeacher();
            //curl https://localhost:44375/api/TeacherData/FindTeacher/{id}

            string url = "TeacherData/FindTeacher/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            TeacherDto SelectedTeacher = response.Content.ReadAsAsync<TeacherDto>().Result;

            ViewModel.SelectedTeacher = SelectedTeacher;

            //show all courses of this teacher
            url = "CourseData/ListCoursesForTeacher/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CourseDto> KeptCourses = response.Content.ReadAsAsync<IEnumerable<CourseDto>>().Result;

            ViewModel.KeptCourses = KeptCourses;

            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Teacher/New
        public ActionResult New()
        {
            return View();
        }

        // GET: Teacher/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teacher/Create
        [HttpPost]

        public ActionResult Create(Teacher Teacher)
        {
            //curl -H "Content-Type:application/json" -d @Teacher.json https://localhost:44375/api/TeacherData/AddTeacher 
            string url = "TeacherData/AddTeacher";


            string jsonpayload = jss.Serialize(Teacher);
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

        // GET: Teacher/Edit/5
      

        public ActionResult Edit(int id)
        {
            string url = "TeacherData/FindTeacher/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeacherDto selectedTeacher = response.Content.ReadAsAsync<TeacherDto>().Result;
            return View(selectedTeacher);
        }

        // POST: Teacher/Update/5
        [HttpPost]
      

        public ActionResult Update(int id, Teacher Teacher, HttpPostedFileBase TeacherPic)
        {
            string url = "Teacherdata/UpdateTeacher/" + id;
            string jsonpayload = jss.Serialize(Teacher);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
           
            //update request is successful and we have image data
            if (response.IsSuccessStatusCode && TeacherPic != null)
            {
                url = "TeacherData/UploadTeacherPic/" + id;

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(TeacherPic.InputStream);
                requestcontent.Add(imagecontent, "TeacherPic", TeacherPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;    

                return RedirectToAction("List");
            }
            else if (response.IsSuccessStatusCode)
            {
                //no image upload but update still successful
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Teacher/DeleteConfirm/5
      

        public ActionResult DeleteConfirm(int id)
        {
            string url = "TeacherData/FindTeacher/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeacherDto selectedTeacher = response.Content.ReadAsAsync<TeacherDto>().Result;
            return View(selectedTeacher);
        }

        // POST: Teacher/Delete/5
        [HttpPost]
      

        public ActionResult Delete(int id)
        {
            string url = "Teacherdata/DeleteTeacher/" + id;
            HttpContent content = new StringContent("");
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
