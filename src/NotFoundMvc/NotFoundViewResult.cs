using System.Web.Mvc;

namespace NotFoundMvc
{
    /// <summary>
    /// Renders a view called "NotFound" and sets the response status code to 404.
    /// View data is assigned for "RequestedUrl" and "ReferrerUrl".
    /// </summary>
    public class NotFoundViewResult : HttpNotFoundResult
    {
        public NotFoundViewResult()
        {
            ViewName = "NotFound";
            ViewData = new ViewDataDictionary();
        }

        /// <summary>
        /// The name of the view to render. Defaults to "NotFound".
        /// </summary>
        public string ViewName { get; set; }
        public ViewDataDictionary ViewData { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            var request = context.HttpContext.Request;
            var url = request.Url.OriginalString;

            var viewResult = new ViewResult
            {
                ViewName = ViewName,
                ViewData = ViewData
            };

            ViewData["RequestedUrl"] = url;
            ViewData["ReferrerUrl"] = (request.UrlReferrer != null && request.UrlReferrer.OriginalString != url)
                                        ? request.UrlReferrer.OriginalString 
                                        : null;
            
            response.StatusCode = 404;
            // Prevent IIS7 from overwriting our error page!
            response.TrySkipIisCustomErrors = true;
            
            viewResult.ExecuteResult(context);
        }
    }
}