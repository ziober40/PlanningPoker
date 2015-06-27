namespace PlanningPoker.Tests.Fakes
{
    using System.Web;
    using System.Web.Mvc;

    public class FakeControllerContext : ControllerContext
    {
        public HttpContextBase Context = new FakeHttpContext();

        public override System.Web.HttpContextBase HttpContext
        {
            get
            {
                return Context;
            }

            set
            {
                Context = value;
            }
        }
    }
}

