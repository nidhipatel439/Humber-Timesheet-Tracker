namespace Humber_Timesheet_Tracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeCourse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "Teacher_TeacherId", "dbo.Teachers");
            DropIndex("dbo.Courses", new[] { "Teacher_TeacherId" });
            CreateTable(
                "dbo.TeacherCourses",
                c => new
                    {
                        Teacher_TeacherId = c.Int(nullable: false),
                        Course_CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Teacher_TeacherId, t.Course_CourseId })
                .ForeignKey("dbo.Teachers", t => t.Teacher_TeacherId, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId, cascadeDelete: true)
                .Index(t => t.Teacher_TeacherId)
                .Index(t => t.Course_CourseId);
            
            DropColumn("dbo.Courses", "Teacher_TeacherId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Teacher_TeacherId", c => c.Int());
            DropForeignKey("dbo.TeacherCourses", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.TeacherCourses", "Teacher_TeacherId", "dbo.Teachers");
            DropIndex("dbo.TeacherCourses", new[] { "Course_CourseId" });
            DropIndex("dbo.TeacherCourses", new[] { "Teacher_TeacherId" });
            DropTable("dbo.TeacherCourses");
            CreateIndex("dbo.Courses", "Teacher_TeacherId");
            AddForeignKey("dbo.Courses", "Teacher_TeacherId", "dbo.Teachers", "TeacherId");
        }
    }
}
