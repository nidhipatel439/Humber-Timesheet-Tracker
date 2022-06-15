# Humber-Timesheet-Tracker (Passion project)

The humber timesheet tracker which can help students to manages the various tasks with how long it takes to finish them for various courses and as well as teachers to manage their information. This project is made with C# MVC.

## Features
- Feature to add a new, edit and delete course
- Feature to add a new, edit and delete task
- Feature to add a new, edit and delete teacher information

## Database
This application has 4 tables.
- Course table contains the information about the course name.
- Task table contains the information about the task name, time and their course.
- Teacher table contains the information about the teacher and their related courses.
- TeacherCourse table is a bridging table between a teacher and course.


## Running this project
- Clone the repository in Visual Studio
- Open the project folder in your computer
- Create an <App_Data> folder in the main project folder
- Go back to visual studio and open Package Manager Console and run the query to build the database on your local server
- Update-database
- The project should set up
- Run API commands through CURL to create new courses

Get a List of Courses curl https://localhost:44375/api/CourseData/ListCourses

Get a Single Course curl https://localhost:44375/api/CourseData/FindCourse/5

Add a new Course (new course info is in course.json) curl -H "Content-Type:application/json" -d @course.json https://localhost:44375/api/CourseData/AddCourse

Delete a Course curl -d "" https://localhost:44375/api/CourseData/DeleteCourse/{id}

Update a Course (existing course info including id must be included in course.json) curl -H "Content-Type:application/json" -d @course.json https://localhost:44375/api/CourseData/UpdateCourse/{id}

## Future features
- Each task has a start/stop button. So, at a time user can click on start button to 
start timer for a particular task
- Record the total time to complete all tasks for each course

