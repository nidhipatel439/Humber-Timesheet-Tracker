namespace Humber_Timesheet_Tracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeacherPic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teachers", "TeacherHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Teachers", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teachers", "PicExtension");
            DropColumn("dbo.Teachers", "TeacherHasPic");
        }
    }
}
