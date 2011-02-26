using System.Web.Mvc;
using NotFoundMvc;

namespace SampleApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test(int id)
        {
            return new NotFoundViewResult();
        }
    }
}
