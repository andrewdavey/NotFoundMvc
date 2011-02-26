using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: PreApplicationStartMethod(typeof(NotFoundMvc.PreApplicationStarter), "Start")]

namespace NotFoundMvc
{
    /// <summary>
    /// Runs at web application start.
    /// </summary>
    public class PreApplicationStarter
    {
        static bool started;

        public static void Start()
        {
            if (started) return; // Only start once.
            started = true;

            DynamicModuleUtility.RegisterModule(typeof(InstallerModule));
        }
    }
}