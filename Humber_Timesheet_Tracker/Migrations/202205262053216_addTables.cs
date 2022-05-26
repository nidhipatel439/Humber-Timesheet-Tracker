namespace Humber_Timesheet_Tracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseName = c.String(),
                    })
                .PrimaryKey(t => t.CourseId);
            
            CreateTable(
                "dbo.CourseTasks",
                c => new
                    {
                        CourseTaskId = c.Int(nullable: false, identity: true),
                        CourseTaskName = c.String(),
                        CourseTaskTime = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CourseTaskId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseTasks", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseTasks", new[] { "CourseId" });
            DropTable("dbo.CourseTasks");
            DropTable("dbo.Courses");
        }
    }
}
