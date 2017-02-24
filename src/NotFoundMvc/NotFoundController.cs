namespace NotFoundMvc
{
    using System.Web.Mvc;

    public class NotFoundController : ControllerBase, INotFoundController
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
