namespace NotFoundMvc
{
    using System;
    using System.Web.Mvc;

    internal static class ActionInvokerSelector
    {
        private static readonly Func<IActionInvoker, IActionInvoker> Mvc3Invoker =
           originalActionInvoker => new ActionInvokerWrapper(originalActionInvoker);

        private readonly static Func<IActionInvoker, IActionInvoker> Mvc4Invoker =
            originalActionInvoker => new NotFoundAsyncControllerActionInvoker();

        public static Func<IActionInvoker, IActionInvoker> Current { get; } = typeof(Controller).Assembly.GetName().Version.Major <= 3 ? Mvc3Invoker : Mvc4Invoker;
    }
}