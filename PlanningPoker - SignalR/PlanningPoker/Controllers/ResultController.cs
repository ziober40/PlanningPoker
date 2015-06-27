namespace PlanningPoker.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using PlanningPoker.Models;

    public class ResultController : Controller
    {
        public readonly IPlanningPokerDb db;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ResultController(IPlanningPokerDb db)
        {
            this.db = db;
        }

        public ActionResult Index(string team="Default", string name = "")
        {
            Response.Expires = -1;
            var Team = db.Query<Team>().FirstOrDefault(k => k.Name == team);

            Team.Results = db.Query<Result>().Where(k => k.TeamId == Team.Id).ToList();

            var count = Team.Amount;
            var Results = Team.Results;

            if (Results.Count() >= count)
            {
                bool isThereIsConsensus = true;
                Result model = Results.FirstOrDefault();
                foreach (var result in Results.ToList())
                {
                    if (model != null && model.Estimate != result.Estimate)
                    {
                        isThereIsConsensus = false;
                    }

                    model = result;
                }

                Team.Results = Results;

                if (isThereIsConsensus)
                {
                    var rm = new ReturnModel
                    {
                        Team = Team,
                        Name = name
                    };

                    return View("ThereIsConsensus", rm);
                }
            }
            else
            {
                var model = new VotingNotFinishedModel
                {
                    PeopleVoted = Team.Results.Count(),
                    PeopleConnected = 0,
                    Maximum = Team.Amount,
                    Team = Team,
                    Name = name
                };
                return View("VotingNotFinished", model);
            }

            Team.Results = SetTheLowestAndHighest(Results.ToList());

            var rmIndex = new ReturnModel
            {
                Team = Team,
                Name = name
            };

            return View(rmIndex);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetAmount(SetAmountModel model)
        {
            if (model.Name == null)
            {
                model.Name = "Default";
            }

            var result = db.Query<Team>().FirstOrDefault(m=>m.Name == model.TeamName);

            if (result != null)
            {
                if (model.SetAmount < 1)
                {
                    ViewBag.Message = "You can`t set below one!";
                    return View(model);
                }
                result.Amount = model.SetAmount;
                db.Update(result);
            }

            db.Save();

            return RedirectToAction("Submit", new { team = result.Name, name = model.Name });
        }

        public ActionResult SetAmount(string team = "Default", string name = "")
        {
            var SetPeopleInTeam = db.Query<Team>().FirstOrDefault(m=>m.Name == team);

            var rmIndex = new SetAmountModel
            {
                TeamName = SetPeopleInTeam.Name,
                Name = name,
                SetAmount = SetPeopleInTeam.Amount
            };

            return View(rmIndex);
        }

        public ActionResult Submit(int NewEstimate = 0, string team = "Default", string name = "", int estimation=0)
        {

            if (TeamsThreadPool.Instance.tasks == null 
                || TeamsThreadPool.Instance.tasks.Any(k => k.IsFaulted) 
                || TeamsThreadPool.Instance.tasks.Any(k => k.IsCompleted) 
                || TeamsThreadPool.Instance.tasks.Any(k => k.IsCanceled))
            {
                log.Info("Some of tasks was removed or canceled or faulted, creating threads one more time ");
                TeamsThreadPool.Instance.AddTeamsToThreadPool();
            }

            Response.Expires = -1;
            var teamModel = db.Query<Team>().FirstOrDefault(k => k.Name == team);
            teamModel.Results = db.Query<Result>().Where(k => k.TeamId == teamModel.Id).ToList();
            
            if (NewEstimate == 1)
            {
                foreach (var table in teamModel.Results.ToList())
                {
                    db.Remove(table);
                }

                db.Save();
            }

            bool cookieIsSaved = false;

            if (name != "")
            {
                saveNameInCookie(name);
                cookieIsSaved = true;
            }

            if (Request.Cookies["name"] != null && estimation == 0)
            {
                var cookieName = Request.Cookies["name"];
                var nameValue = cookieIsSaved ? name : cookieName.Value;
                var model = new SubmitModel
                {
                    Name = nameValue,
                    Team = team
                };

                return View(model);
            }

            var count = teamModel.Amount;
            if (teamModel.Results.Count() >= count && teamModel.Results.All(b => b.Nickname != name))
            {
                var rmIndex = new ReturnModel
                {
                    Team = teamModel,
                    Name = name
                };

                return View("Toomany", rmIndex);
            }

            if (estimation != 0 && name != "" && team != "")
            {
                var returnModel = new SubmitModel
                {
                    estimate = (Estimation)estimation,
                    Name = name,
                    Team = team
                };

                var returnTeam = db.Query<Team>().FirstOrDefault(k => k.Name == returnModel.Team);
                returnTeam.Results = db.Query<Result>().Where(k => k.TeamId == returnTeam.Id).ToList();

                var result = returnTeam.Results.FirstOrDefault(k => k.Nickname == returnModel.Name);
                if (result != null)
                {
                    result.Estimate = returnModel.estimate;
                    db.Update(result);
                }
                else
                {
                    var returnResult = new Result
                    {
                        Estimate = returnModel.estimate,
                        Nickname = returnModel.Name
                    };

                    returnTeam.Results.Add(returnResult);


                    db.Update(returnTeam);
                }

                if (ModelState.IsValid)
                {
                    db.Save();

                    return RedirectToAction("Index", "Result", new { team = returnTeam.Name, name });
                }

                return View("Submit", returnModel);
            }

            var m = new SubmitModel
            {
                Name = name,
                Team = team
            };

            return View(m);
        }
        

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        #region Helpers

        private void saveNameInCookie(string name)
        {
                var cookieName = new HttpCookie("name", name);
                Response.AppendCookie(cookieName);
        }

        private List<Result> SetTheLowestAndHighest(List<Result> list)
        {
            var highest = list.First();
            var lowest = list.First();
            foreach (var result in list)
            {
                if (result.Estimate >= Estimation.zero)
                {
                    if (result.Estimate > highest.Estimate)
                    {
                        highest = result;
                    }

                    if (result.Estimate < lowest.Estimate)
                    {
                        lowest = result;
                    }
                }
            }

            foreach (var result in list)
            {
                if (result.Estimate == highest.Estimate)
                {
                    result.IsHighest = true;
                }
                if (result.Estimate == lowest.Estimate)
                {
                    result.IsLowest = true;
                }
            }

            return list;
        }

        #endregion

    }
}