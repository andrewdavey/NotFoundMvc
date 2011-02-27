using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NotFoundMvc
{
    public class NotFoundHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "NotFound");
            var controllerContext = new ControllerContext(new HttpContextWrapper(context), routeData, new FakeController());
            var notFoundViewResult = new NotFoundViewResult();
            notFoundViewResult.ExecuteResult(controllerContext);
        }

        public bool IsReusable
        {
            get { return false; }
        }

        // ControllerContext requires an object that derives from ControllerBase.
        class FakeController : Controller { }
    }
}