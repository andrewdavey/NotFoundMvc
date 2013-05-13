using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NotFoundMvc
{
    public class NotFoundHandler : IHttpHandler
    {
        public static Func<RequestContext, IController> CreateCustomNotFoundController { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "NotFound");

            if (CreateCustomNotFoundController != null)
            {
                var ctx = new HttpContextWrapper(context);
                var requestContext = new RequestContext(ctx, routeData);
                var controller = CreateCustomNotFoundController(requestContext);
                if (controller != null)
                {
                    controller.Execute(requestContext);
                    return;
                }
            }

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