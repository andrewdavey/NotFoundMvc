namespace NotFoundMvc
{
    using System.Web.Mvc;

    [AllowAnonymous]
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

    public interface INotFoundController : IController
    {
        ActionResult NotFound();
    }
}
