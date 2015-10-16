namespace NotFoundMvc
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class NotFoundController : ControllerBase
    {
        public ActionResult NotFound()
        {
            return new NotFoundViewResult();
        }

        protected override void ExecuteCore()
        {
            new NotFoundViewResult().ExecuteResult(this.ControllerContext);
        }
    }
}