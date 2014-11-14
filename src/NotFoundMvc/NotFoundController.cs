using System.Web.Mvc;
using System.Web.Routing;

namespace NotFoundMvc
{
    public class NotFoundController : IController
    {
        public void Execute(RequestContext requestContext)
        {
            ExecuteNotFound(requestContext);
        }

        public void ExecuteNotFound(RequestContext requestContext)
        {
            Controller controller = new FakeController();
            ControllerContext context = new ControllerContext(requestContext, controller);
            controller.ControllerContext = context;
            new NotFoundViewResult().ExecuteResult(
                context
            );
        }

        // ControllerContext requires an object that derives from ControllerBase.
        // NotFoundController does not do this.
        // So the easiest workaround is this FakeController.
        class FakeController : Controller { }
    }

}