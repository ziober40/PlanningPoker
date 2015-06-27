using System.Web.Mvc;

namespace PlanningPoker.Controllers
{
    public class RulesController : Controller
    {
        public ActionResult Index(string team)
        {
            ViewBag.Message = team;
            return View();
        }

    }
}
