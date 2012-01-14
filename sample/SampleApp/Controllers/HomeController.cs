using System.Web;
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

        public ActionResult Fail()
        {
            throw new HttpException(404, "Not found!");
        }
    }
}
