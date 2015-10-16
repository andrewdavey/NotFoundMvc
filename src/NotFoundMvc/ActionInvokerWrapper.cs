namespace NotFoundMvc
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Wraps another IActionInvoker except it handles the case of an action method
    /// not being found and invokes the NotFoundController instead.
    /// </summary>
    internal class ActionInvokerWrapper : IActionInvoker
    {
        private readonly IActionInvoker actionInvoker;

        public ActionInvokerWrapper(IActionInvoker actionInvoker)
        {
            this.actionInvoker = actionInvoker;
        }

        public bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            if (this.InvokeActionWith404Catch(controllerContext, actionName))
            {
                return true;
            }

            // No action method was found, or it was, but threw a 404 HttpException.
            ExecuteNotFoundControllerAction(controllerContext);
            return true;
        }

        private static void ExecuteNotFoundControllerAction(ControllerContext controllerContext)
        {
            IController controller;
            if (NotFoundHandler.CreateNotFoundController != null)
            {
                controller = NotFoundHandler.CreateNotFoundController(controllerContext.RequestContext) ?? new NotFoundController();
            }
            else
            {
                controller = new NotFoundController();
            }

            controller.Execute(controllerContext.RequestContext);
        }

        private bool InvokeActionWith404Catch(ControllerContext controllerContext, string actionName)
        {
            try
            {
                return this.actionInvoker.InvokeAction(controllerContext, actionName);
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
