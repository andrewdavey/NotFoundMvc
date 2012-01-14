using System;
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
            var url = request.AppRelativeCurrentExecutionFilePath == "~/notfound" ? ExtractOriginalUrlFromExecuteUrlModeErrorRequest(request.Url) : request.Url.OriginalString;

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

        /// <summary>
        /// Handles the case when a web.config &lt;error statusCode="404" path="/notfound" responseMode="ExecuteURL" /&gt; is triggered.
        /// The original URL is passed via the querystring.
        /// </summary>
        string ExtractOriginalUrlFromExecuteUrlModeErrorRequest(Uri url)
        {
            // Expected format is "?404;http://hostname.com/some/path"
            var start = url.Query.IndexOf(';');
            if (0 <= start && start < url.Query.Length - 1)
            {
                return url.Query.Substring(start + 1);
            }
            else
            {
                // Unexpected format, so just return the full URL!
                return url.ToString();
            }
        }
    }
}