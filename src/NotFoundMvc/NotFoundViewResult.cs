namespace NotFoundMvc
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Renders a view called "NotFound" and sets the response status code to 404.
    /// View data is assigned for "RequestedUrl" and "ReferrerUrl".
    /// </summary>
    public class NotFoundViewResult : HttpNotFoundResult
    {
        public NotFoundViewResult()
        {
            this.ViewName = "NotFound";
            this.ViewData = new ViewDataDictionary();
        }

        /// <summary>
        /// The name of the view to render. Defaults to "NotFound".
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// The view data passed to the NotFound view.
        /// </summary>
        public ViewDataDictionary ViewData { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;
            var request = context.HttpContext.Request;

            var requestedUrl = GetRequestedUrl(request);
            NotFoundConfig.OnNotFound(context.RequestContext.HttpContext.Request, new Uri(requestedUrl));
            this.ViewData["RequestedUrl"] = requestedUrl;
            this.ViewData["ReferrerUrl"] = GetReferrerUrl(request, request.Url.OriginalString);

            // Make sure the status code is 404 for the search engine bots
            response.StatusCode = 404;

            // Prevent IIS7 from overwriting our error page!
            response.TrySkipIisCustomErrors = true;

            var viewResult = new ViewResult
            {
                ViewName = this.ViewName,
                ViewData = this.ViewData
            };
            response.Clear();
            viewResult.ExecuteResult(context);
        }

        private static string GetRequestedUrl(HttpRequestBase request)
        {
            return request.AppRelativeCurrentExecutionFilePath == "~/notfound"
                       ? ExtractOriginalUrlFromExecuteUrlModeErrorRequest(request.Url)
                       : request.Url.OriginalString;
        }

        private static string GetReferrerUrl(HttpRequestBase request, string url)
        {
            return request.UrlReferrer != null && request.UrlReferrer.OriginalString != url
                       ? request.UrlReferrer.OriginalString
                       : null;
        }

        /// <summary>
        /// Handles the case when a web.config &lt;error statusCode="404" path="/notfound" responseMode="ExecuteURL" /&gt; is triggered.
        /// The original URL is passed via the querystring.
        /// </summary>
        private static string ExtractOriginalUrlFromExecuteUrlModeErrorRequest(Uri url)
        {
            // Expected format is "?404;http://hostname.com/some/path"
            var start = url.Query.IndexOf(';');
            if (0 <= start && start < url.Query.Length - 1)
            {
                return url.Query.Substring(start + 1);
            }

            // Unexpected format, so just return the full URL!
            return url.ToString();
        }
    }
}