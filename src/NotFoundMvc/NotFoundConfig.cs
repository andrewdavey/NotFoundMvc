namespace NotFoundMvc
{
    using System;
    using System.Web;

    public static class NotFoundConfig
    {
        private static readonly Action<HttpRequestBase, Uri> NullOnNotFound = (req, uri) => { /*noop*/ };

        private static Action<HttpRequestBase, Uri> onNotFound;

        /// <summary>
        /// Gets or sets the action to execute when a 404 has occurred.
        /// Here you can pass the 404 on to your own logging (NLog, log4net) or error handling (ELMAH).
        /// </summary>
        /// <value>
        /// The action to execute when a 404 has occurred.
        /// </value>
        public static Action<HttpRequestBase, Uri> OnNotFound
        {
            get
            {
                return onNotFound ?? NullOnNotFound;
            }

            set
            {
                onNotFound = value;
            }
        }
    }
}
