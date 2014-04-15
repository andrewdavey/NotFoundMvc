using System;
using System.Web.Mvc;

namespace NotFoundMvc
{
    static class ActionInvokerSelector
    {
        static ActionInvokerSelector()
        {
            var mvcVersion = typeof (Controller).Assembly.GetName().Version.Major;
            Current = mvcVersion <= 3 ? _mvc3Invoker : _mvc4Invoker;
        }

        private static readonly Func<IActionInvoker, IActionInvoker> _mvc3Invoker =
           originalActionInvoker => new ActionInvokerWrapper(originalActionInvoker);

        private readonly static Func<IActionInvoker, IActionInvoker> _mvc4Invoker =
            originalActionInvoker => new NotFoundAsyncControllerActionInvoker();

        public static Func<IActionInvoker, IActionInvoker> Current { get; set; }

    }
}