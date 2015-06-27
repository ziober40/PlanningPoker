namespace PlanningPoker.Tests.Fakes
{
    using System.Collections.Specialized;
    using System.Web;

    public class FakeHttpRequest : HttpRequestBase
    {
        public override NameValueCollection Headers
        {
            get
            {
                return new NameValueCollection();
            }
        }

        public override string this[string key]
        {
            get
            {
                return null;
            }
        }
    }
}