namespace SampleApp
{
    using System.Globalization;
    using System.Web.Mvc;
    using System.Web.Routing;
    using NLog;
    using NotFoundMvc;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            NotFoundConfig.OnNotFound = (req, uri) =>
            {
                // The current URI, which can be the same as the original requested URI http://localhost:43825/bin
                // or something triggered from the IIS via the system.webServer/httpErrors http://localhost:43825/notfound?404;http://localhost:43825/bin
                System.Diagnostics.Trace.WriteLine(req.Url.ToString());

                // This is the original requested URI http://localhost:43825/bin
                System.Diagnostics.Trace.WriteLine(uri);

                Log.Warn(CultureInfo.InvariantCulture, "404 {0}", uri);
            };
        }
    }
}