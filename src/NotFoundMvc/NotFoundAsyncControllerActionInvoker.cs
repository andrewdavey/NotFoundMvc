namespace NotFoundMvc
{
    using System.Web.Mvc;
    using System.Web.Mvc.Async;

    public class NotFoundAsyncControllerActionInvoker : AsyncControllerActionInvoker
    {
        protected override ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, string actionName)
        {
            var result = base.FindAction(controllerContext, controllerDescriptor, actionName);
            if (result == null)
            {
                return new NotFoundActionDescriptor();
            }

            return result;
        }
    }
}