using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Humber_Timesheet_Tracker.Startup))]
namespace Humber_Timesheet_Tracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
