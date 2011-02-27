using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NotFoundMvc
{
    class InstallerModule : IHttpModule
    {
        static bool installed;
        static object installerLock = new object();

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
            RouteTable.Routes.MapRoute(
                "NotFound",
                "notfound",
                new { controller = "NotFound", action = "NotFound" }
            );
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
    }
}
