namespace NotFoundMvc
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class NotFoundHandler : IHttpHandler
    {
        private static Func<RequestContext, IController> createNotFoundController = context => new NotFoundController();

        public static Func<RequestContext, IController> CreateNotFoundController
        {
            get
            {
                return createNotFoundController;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                createNotFoundController = value;
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            this.ProcessRequest(new HttpContextWrapper(context));
        }

        private void ProcessRequest(HttpContextBase context)
        {
            var requestContext = this.CreateRequestContext(context);
            var controller = createNotFoundController(requestContext);
            controller.Execute(requestContext);
        }

        private RequestContext CreateRequestContext(HttpContextBase context)
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "NotFound");
            var requestContext = new RequestContext(context, routeData);
            return requestContext;
        }
    }
}