using System.Linq;
using System.Web.Mvc;

namespace PlanningPoker.Controllers
{
    using System.Net;
    using PlanningPoker.Models;

    public class HomeController : Controller
    {

        public readonly IPlanningPokerDb db;

        public HomeController(IPlanningPokerDb db)
        {
            this.db = db;
        }

        public ActionResult Index()
        {
            var result = db.Query<Team>().ToList();

            return View(result);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Team team)
        {
            if (team.Name == "")
            {
                return View();
            }

            var TeamModel = db.Query<Team>().FirstOrDefault(k => k.Name == team.Name);

            if (TeamModel != null)
            {
                return RedirectToAction("Submit", "Result", new {team = TeamModel.Name});
            }
            team.Amount = 5;
            db.Add(team);
            db.Save();
            return RedirectToAction("Submit", "Result", new { team = team.Name });
  
        }

    }
}
