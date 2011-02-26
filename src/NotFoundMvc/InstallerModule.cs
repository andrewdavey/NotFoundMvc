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

        void AddCatchAllRoute()
        {
            RouteTable.Routes.MapRoute(
                "NotFound",
                "{*any}",
                new { controller = "NotFound", action = "NotFound" }
            );
        }

        public void Dispose() { }
    }
}
