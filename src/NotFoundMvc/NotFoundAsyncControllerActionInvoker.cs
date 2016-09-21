using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Async;

namespace NotFoundMvc
{
    public class NotFoundAsyncControllerActionInvoker : AsyncControllerActionInvoker
    {
        protected override ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor,
            string actionName)
        {
            var result = base.FindAction(controllerContext, controllerDescriptor, actionName);
            if (result == null)
            {
                return new NotFoundActionDescriptor();
            }
            return result;
        }
    }

    public class NotFoundActionDescriptor : ActionDescriptor
    {
        public override object Execute(ControllerContext controllerContext, IDictionary<string, object> parameters)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException(nameof(controllerContext));
            }

            INotFoundController notFound = NotFoundHandler.CreateNotFoundController(controllerContext.RequestContext);
            controllerContext.RouteData.Values["action"] = "NotFound";
            return notFound.NotFound();
        }

        public override ParameterDescriptor[] GetParameters()
        {
            return new ParameterDescriptor[] { };
        }

        public override string ActionName
        {
            get { return "NotFound"; }
        }

        public override ControllerDescriptor ControllerDescriptor
        {
            get { return new ReflectedControllerDescriptor(typeof(NotFoundController)); }
        }
    }
}