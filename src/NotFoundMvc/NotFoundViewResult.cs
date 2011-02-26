using System.Web.Mvc;

namespace NotFoundMvc
{
    /// <summary>
    /// Renders a view called "NotFound" and sets the response status code to 404.
    /// View data is assigned for "RequestedUrl" and "ReferrerUrl".
    /// </summary>
    public class NotFoundViewResult : ViewResult
    {
        public NotFoundViewResult()
        {
            ViewName = "NotFound";
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            var request = context.HttpContext.Request;
            var url = request.Url.OriginalString;

            ViewData["RequestedUrl"] = url;
            ViewData["ReferrerUrl"] = (request.UrlReferrer != null && request.UrlReferrer.OriginalString != url)
                                        ? request.UrlReferrer.OriginalString 
                                        : null;

            response.StatusCode = 404;
            // Prevent IIS7 from overwriting our error page!
            response.TrySkipIisCustomErrors = true;

            base.ExecuteResult(context);
        }
    }
}