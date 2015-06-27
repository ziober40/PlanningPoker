namespace PlanningPoker.Tests.Fakes
{
    using System.Web;

    public class FakeHttpContext : HttpContextBase
    {
        private readonly HttpRequestBase request = new FakeHttpRequest();

        public override HttpRequestBase Request
        {
            get
            {
                return request;
            }
        }
    }
}