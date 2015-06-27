namespace PlanningPoker.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.UI;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PlanningPoker.Controllers;
    using PlanningPoker.Models;
    using PlanningPoker.Tests.Fakes;

    [TestClass]
    public class ResultControllerTests
    {

        public static HttpContextBase FakeHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var cookies = new HttpCookieCollection();
            var items = new ListDictionary();

            request.Setup(r => r.Cookies).Returns(cookies);
            response.Setup(r => r.Cookies).Returns(cookies);

            context.Setup(ctx => ctx.Items).Returns(items);

            context.SetupGet(ctx => ctx.Request).Returns(request.Object);
            context.SetupGet(ctx => ctx.Response).Returns(response.Object);
            context.SetupGet(ctx => ctx.Session).Returns(session.Object);
            context.SetupGet(ctx => ctx.Server).Returns(server.Object);

            return context.Object;
        }


        [TestMethod]
        public void CheckIfThereIsConsensus()
        {
            var db = new FakePlanningPokerDb();
            db.AddSet(TestData.ResultsWithConsensus);
            db.AddSet(TestData.VotingPeopleCounts10);
            var controller = new ResultController(db);

            var httpContext = FakeHttpContext();
            var route = new RouteData();
            controller.ControllerContext = new ControllerContext(new RequestContext(httpContext, route), controller);

            var result = controller.Index() as ViewResult;
            var viewBag = result.ViewName;
            Assert.AreEqual("ThereIsConsensus", viewBag);
        }

        [TestMethod]
        public void Bartek0IsLowest()
        {
            var db = new FakePlanningPokerDb();
            db.AddSet(TestData.Results);
            db.AddSet(TestData.VotingPeopleCounts5);
            var controller = new ResultController(db);

            var httpContext = FakeHttpContext();
            var route = new RouteData();
            controller.ControllerContext = new ControllerContext(new RequestContext(httpContext, route), controller);

            var result = controller.Index() as ViewResult;
            var model = result.Model as ReturnModel;
            var user = model.Team.Results.FirstOrDefault(k => k.Nickname == "Bartek1");
            Assert.IsTrue(user.IsLowest);
        }

        [TestMethod]
        public void Bartek1IsHighest()
        {
            var db = new FakePlanningPokerDb();
            db.AddSet(TestData.Results);
            db.AddSet(TestData.VotingPeopleCounts5);
            var controller = new ResultController(db);

            var httpContext = FakeHttpContext();
            var route = new RouteData();
            controller.ControllerContext = new ControllerContext(new RequestContext(httpContext, route), controller);

            var result = controller.Index() as ViewResult;
            var model = result.Model as ReturnModel;
            var user = model.Team.Results.FirstOrDefault(k => k.Nickname == "Bartek4");
            Assert.IsTrue(user.IsHighest);
        }

        [TestMethod]
        public void CheckIfReturning10resultsAfterSubmittingDiffrentVotes()
        {
            var db = new FakePlanningPokerDb();
            db.AddSet(TestData.Results);
            db.AddSet(TestData.VotingPeopleCounts10);
            var controller = new ResultController(db);

            var httpContext = FakeHttpContext();
            var route = new RouteData();
            controller.ControllerContext = new ControllerContext(new RequestContext(httpContext, route), controller);

            var result = controller.Index() as ViewResult;
            var model = result.Model as ReturnModel;
            Assert.AreEqual(10, model.Team.Results.Count);
        }

        [TestMethod]
        public void MaximumAmountOfPeople()
        {
            var db = new FakePlanningPokerDb();
            db.AddSet(TestData.Results);
            db.AddSet(TestData.VotingPeopleCounts10);
            var controller = new ResultController(db);
            var cookie = new HttpCookie("name","roman");
            
            controller.ControllerContext = new FakeControllerContext();
           

            var model = new Result
            {
                Id = 10,
                Estimate = Estimation.five,
                Nickname = "Json"
            };

            

            var result = controller.Submit(0, "Default", "Json", 5) as ViewResult;
            var viewBag = result.ViewName;

            Assert.AreEqual("Toomany", viewBag);
        }

        //[TestMethod]
        //public void ChangeAmountOfPeople()
        //{
        //    var db = new FakePlanningPokerDb();
        //    db.AddSet(TestData.Results);
        //    db.AddSet(TestData.VotingPeopleCounts10);

        //    var controller = new ResultController(db);

        //    var vote = new Team
        //    {
        //        Id = 0,
        //        Amount = 13
        //    };

        //    var route = controller.SetAmount(vote) as RedirectToRouteResult;

        //    Assert.AreEqual("Submit", route.RouteValues["action"]);

        //    var model = new Result
        //    {
        //        Id = 10,
        //        Estimate = Estimation.five,
        //        Nickname = "Json"
        //    };

        //    var result = controller.Submit(model) as RedirectToRouteResult;

        //    Assert.AreEqual("Index", result.RouteValues["action"]);
        //}

        //[TestMethod]
        //public void ProperlyUpdated()
        //{
        //    var db = new FakePlanningPokerDb();
        //    db.AddSet(TestData.Results);
        //    db.AddSet(TestData.VotingPeopleCounts10);
        //    var controller = new ResultController(db);
        //    var model = new Result
        //    {
        //        Estimate = Estimation.infinite,
        //        Nickname = "Bartek2"
        //    };

        //    var result = controller.Submit(model) as RedirectToRouteResult;

        //    Assert.AreEqual("Index", result.RouteValues["action"]);
        //}

        //[TestMethod]
        //public void AddNewPersonAndModifyTheData()
        //{
        //    var db = new FakePlanningPokerDb();
        //    db.AddSet(TestData.Results7);
        //    db.AddSet(TestData.VotingPeopleCounts10);
        //    var model = new Result
        //    {
        //        Estimate = Estimation.infinite,
        //        Nickname = "Bartek2"
        //    };

        //    var controller = new ResultController(db);

        //    Assert.AreEqual(controller.db.Query<Result>().Count(), 7);

        //}

        //[TestMethod]
        //public void AddSevenPeopleDecreaseMaximumLevelToSixModifyOneOfPeople()
        //{
        //    var db = new FakePlanningPokerDb();
        //    db.AddSet(TestData.Results5);
        //    db.AddSet(TestData.VotingPeopleCounts10);

        //    var model1 = new Result
        //    {
        //        Estimate = Estimation.infinite,
        //        Nickname = "Bartek2"
        //    };

        //    var model2 = new Result
        //    {
        //        Estimate = Estimation.infinite,
        //        Nickname = "Bartek3"
        //    };

        //    var controller = new ResultController(db);

        //    controller.Submit(model1);
        //    controller.Submit(model2);

        //    var prop = new Team
        //    {
        //        Amount = 6
        //    };

        //    controller.SetAmount(prop);

        //    model1.Estimate = Estimation.eight;

        //    controller.Submit(model1);

        //    var model = controller.db.Query<Result>().FirstOrDefault(k => k.Nickname == model1.Nickname);

        //    Assert.AreEqual(Estimation.eight, model.Estimate);
        //}

        //[TestMethod]
        //public void Add7PeopleDecreaseMaxLevelTo5CheckIfRedirectToConsensus()
        //{
        //    var db = new FakePlanningPokerDb();
        //    db.AddSet(TestData.Results7c);
        //    db.AddSet(TestData.VotingPeopleCounts10);

        //    var controller = new ResultController(db);
        //    var prop = new Team
        //    {
        //        Name = "Default",
        //        Amount = 5,
        //        AmountOfPeopleConnected = 0
        //    };

        //    controller.SetAmount(prop);

        //    var result = controller.Index() as ViewResult;

        //    Assert.AreEqual("ThereIsConsensus", result.ViewName);
        //}

        //[TestMethod]
        //public void TryToSeeResultsButVotingIsNotFinished()
        //{
        //    var db = new FakePlanningPokerDb();
        //    db.AddSet(TestData.Results7c);
        //    db.AddSet(TestData.VotingPeopleCounts10);

        //    var controller = new ResultController(db);

        //    var result = controller.Index() as ViewResult;

        //    Assert.AreEqual("VotingNotFinished", result.ViewName);
        //}

        //[TestMethod]
        //public void AddSomePeopleNextEstimationAmountOfPeopleShouldBeZero()
        //{
        //    var db = new FakePlanningPokerDb();
        //    db.AddSet(TestData.Results7c);
        //    db.AddSet(TestData.VotingPeopleCounts10);

        //    var controller = new ResultController(db);
        //    var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
        //    request.Setup(x => x.Cookies).Returns(new HttpCookieCollection());

        //    var context = new Mock<HttpContextBase>(MockBehavior.Strict);

        //    context.SetupGet(x => x.Request).Returns(request.Object);

        //    controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

        //    controller.Submit(1);
        //}


    }
}
