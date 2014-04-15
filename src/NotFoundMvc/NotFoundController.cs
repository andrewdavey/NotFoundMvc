using System.Web.Mvc;
using System.Web.Routing;

namespace NotFoundMvc
{
    public class NotFoundController : IController, INotFoundController
    {
        public void Execute(RequestContext requestContext)
        {
            ExecuteNotFound(requestContext);
        }

        public void ExecuteNotFound(RequestContext requestContext)
        {
            new NotFoundViewResult().ExecuteResult(
                new ControllerContext(requestContext, new FakeController())
            );
        }

        public ActionResult NotFound()
        {
            return new NotFoundViewResult();
        }

        // ControllerContext requires an object that derives from ControllerBase.
        // NotFoundController does not do this.
        // So the easiest workaround is this FakeController.
        class FakeController : Controller { }
    }

    public interface INotFoundController: IController
    {
        ActionResult NotFound();
    }
}