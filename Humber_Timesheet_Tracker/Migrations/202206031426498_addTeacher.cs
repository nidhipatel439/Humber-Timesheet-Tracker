namespace Humber_Timesheet_Tracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTeacher : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        TeacherId = c.Int(nullable: false, identity: true),
                        TeacherFirstName = c.String(),
                        TeacherLastName = c.String(),
                    })
                .PrimaryKey(t => t.TeacherId);
            
            AddColumn("dbo.Courses", "Teacher_TeacherId", c => c.Int());
            CreateIndex("dbo.Courses", "Teacher_TeacherId");
            AddForeignKey("dbo.Courses", "Teacher_TeacherId", "dbo.Teachers", "TeacherId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "Teacher_TeacherId", "dbo.Teachers");
            DropIndex("dbo.Courses", new[] { "Teacher_TeacherId" });
            DropColumn("dbo.Courses", "Teacher_TeacherId");
            DropTable("dbo.Teachers");
        }
    }
}
