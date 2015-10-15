namespace NotFoundMvc
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    internal class InstallerModule : IHttpModule
    {
        private static readonly object InstallerLock = new object();

        private static bool installed;

        public void Init(HttpApplication application)
        {
            if (installed)
            {
                return;
            }

            lock (InstallerLock)
            {
                if (installed)
                {
                    return;
                }

                Install();
                installed = true;
            }
        }

        public void Dispose()
        {
        }

        private static void Install()
        {
            WrapControllerBuilder();

            var routes = RouteTable.Routes;
            using (routes.GetWriteLock())
            {
                AddNotFoundRoute(routes);
                AddCatchAllRoute(routes);
            }
        }

        private static void WrapControllerBuilder()
        {
            ControllerBuilder.Current.SetControllerFactory(
                new ControllerFactoryWrapper(
                    ControllerBuilder.Current.GetControllerFactory()
                )
            );
        }

        private static void AddNotFoundRoute(RouteCollection routes)
        {
            // To allow IIS to execute "/notfound" when requesting something which is disallowed,
            // such as /bin or /add_data.
            var route = new Route(
                "notfound",
                new RouteValueDictionary(new { controller = "NotFound", action = "NotFound" }),
                new RouteValueDictionary(new { incoming = new IncomingRequestRouteConstraint() }),
                new MvcRouteHandler()
            );

            // Insert at start of route table. This means the application can still create another route like "{name}" that won't capture "/notfound".
            routes.Insert(0, route);
        }

        private static void AddCatchAllRoute(RouteCollection routes)
        {
            routes.MapRoute(
                "NotFound-Catch-All",
                "{*any}",
                new { controller = "NotFound", action = "NotFound" }
            );
        }

        private class IncomingRequestRouteConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                return routeDirection == RouteDirection.IncomingRequest;
            }
        }
    }
}
