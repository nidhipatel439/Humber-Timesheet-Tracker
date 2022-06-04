# Humber-Timesheet-Tracker

This application is Course,Task and Teacher management system with CRUD functions built using C# MVC.


## Running this project
- Run API commands through CURL to create new courses

- Get a List of Courses curl https://localhost:44375/api/CourseData/ListCourses

- Get a Single Course curl https://localhost:44375/api/CourseData/FindCourse/5

- Add a new Course (new course info is in course.json) curl -H "Content-Type:application/json" -d @course.json https://localhost:44375/api/CourseData/AddCourse

- Delete a Course curl -d "" https://localhost:44375/api/CourseData/DeleteCourse/{id}

- Update a Course (existing course info including id must be included in course.json) curl -H "Content-Type:application/json" -d @course.json https://localhost:44375/api/CourseData/UpdateCourse/{id}

