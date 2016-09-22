namespace NotFoundMvc
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

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

        public override string ActionName
        {
            get { return "NotFound"; }
        }

        public override ParameterDescriptor[] GetParameters()
        {
            return new ParameterDescriptor[] { };
        }

        public override ControllerDescriptor ControllerDescriptor
        {
            get { return new ReflectedControllerDescriptor(typeof(NotFoundController)); }
        }
    }
}