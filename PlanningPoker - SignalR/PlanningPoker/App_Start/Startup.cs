using Microsoft.Owin;
using PlanningPoker.App_Start;

[assembly: OwinStartup(typeof(Startup))]
namespace PlanningPoker.App_Start
{
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}