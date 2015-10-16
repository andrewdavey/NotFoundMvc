namespace SampleAppMvc5
{
    using System.Globalization;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using NLog;
    using NotFoundMvc;

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

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
