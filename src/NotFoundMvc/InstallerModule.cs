using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NotFoundMvc
{
    class InstallerModule : IHttpModule
    {
        static bool installed;
        static readonly object installerLock = new object();

        public void Init(HttpApplication application)
        {
            if (!installed)
            {
                lock (installerLock)
                {
                    if (!installed)
                    {
                        Install();
                        installed = true;
                    }
                }
            }
        }

        void Install()
        {
            WrapControllerBuilder();
            AddNotFoundRoute();
            AddCatchAllRoute();
        }

        void WrapControllerBuilder()
        {
            ControllerBuilder.Current.SetControllerFactory(
                new ControllerFactoryWrapper(
                    ControllerBuilder.Current.GetControllerFactory()
                )
            );
        }

        void AddNotFoundRoute()
        {
            // To allow IIS to execute "/notfound" when requesting something which is disallowed,
            // such as /bin or /add_data.
            var route = new Route(
                "notfound",
                new RouteValueDictionary(new {controller = "NotFound", action = "NotFound"}),
                new RouteValueDictionary(new {incoming = new IncomingRequestRouteConstraint()}),
                new MvcRouteHandler()
            );
            
            // Insert at start of route table. This means the application can still create another route like "{name}" that won't capture "/notfound".
            RouteTable.Routes.Insert(0, route);
        }

        void AddCatchAllRoute()
        {
            RouteTable.Routes.MapRoute(
                "NotFound-Catch-All",
                "{*any}",
                new { controller = "NotFound", action = "NotFound" }
            );
        }

        public void Dispose() { }

        class IncomingRequestRouteConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                return routeDirection == RouteDirection.IncomingRequest;
            }
        }
    }
}
