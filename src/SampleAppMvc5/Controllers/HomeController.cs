using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleAppMvc5.Controllers
{
    using NotFoundMvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product(int id)
        {
            // NotFoundViewResults inherits from HttpNotFoundResult
            return new NotFoundViewResult();
        }
        
        public ActionResult Fail()
        {
            Response.Write("Attempt to write some content."); // Expecting the NotFoundViewResult to clear the response before sending its output.

            throw new HttpException(404, "Not found!");
        }
    }
}