﻿using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace NotFoundMvc
{
    class ControllerFactoryWrapper : IControllerFactory
    {
        readonly IControllerFactory factory;

        public ControllerFactoryWrapper(IControllerFactory factory)
        {
            this.factory = factory;
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                var controller = factory.CreateController(requestContext, controllerName);
                WrapControllerActionInvoker(controller);
                return controller;
            }
            catch (HttpException ex)
            {
                if (ex.GetHttpCode() == 404)
                {
                    IController controller = null;
                    if (NotFoundHandler.CreateCustomNotFoundController != null)
                    {
                        controller = NotFoundHandler.CreateCustomNotFoundController(requestContext);
                    } 
                    
                    return controller ?? new NotFoundController();
                }

                throw;
            }
        }

        void WrapControllerActionInvoker(IController controller)
        {
            var controllerWithInvoker = controller as Controller;
            if (controllerWithInvoker != null)
            {
                controllerWithInvoker.ActionInvoker = new ActionInvokerWrapper(controllerWithInvoker.ActionInvoker);
            }
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return factory.GetControllerSessionBehavior(requestContext, controllerName);
        }

        public void ReleaseController(IController controller)
        {
            factory.ReleaseController(controller);
        }
    }
}