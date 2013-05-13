using System.Web;
using System.Web.Mvc;

namespace NotFoundMvc
{
    /// <summary>
    /// Wraps another IActionInvoker except it handles the case of an action method
    /// not being found and invokes the NotFoundController instead.
    /// </summary>
    class ActionInvokerWrapper : IActionInvoker
    {
        readonly IActionInvoker actionInvoker;

        public ActionInvokerWrapper(IActionInvoker actionInvoker)
        {
            this.actionInvoker = actionInvoker;
        }

        public bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            if (InvokeActionWith404Catch(controllerContext, actionName))
                return true;

            // No action method was found, or it was, but threw a 404 HttpException.
            ExecuteNotFoundControllerAction(controllerContext);

            return true;
        }

        static void ExecuteNotFoundControllerAction(ControllerContext controllerContext)
        {
            IController controller;
            if (NotFoundHandler.CreateCustomNotFoundController != null)
            {
                controller = NotFoundHandler.CreateCustomNotFoundController(controllerContext.RequestContext) ?? new NotFoundController();
            }
            else
            {
                controller = new NotFoundController();
            }

            controller.Execute(controllerContext.RequestContext);
        }

        bool InvokeActionWith404Catch(ControllerContext controllerContext, string actionName)
        {
            try
            {
                return actionInvoker.InvokeAction(controllerContext, actionName);
            }
            catch (HttpException ex)
            {
                if (ex.GetHttpCode() == 404)
                {
                    return false;
                }
                throw;
            }
        }
    }
}
