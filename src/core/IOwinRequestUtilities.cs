using System;
using System.Globalization;
using System.IO;

namespace Microsoft.Owin
{
    /// <summary>
    /// Utilities that extend the <see cref="IOwinRequest"/> class
    /// </summary>
    public static class OwinRequestUtilities
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="request">The request context</param>
        /// <returns></returns>       
        public static bool IsBearerTokenRequest(this IOwinRequest request)
        {
            IHeaderDictionary headers = request.Headers;
            string authorizationHeader = headers["Authorization"] ?? string.Empty;

            return CultureInfo.InvariantCulture.CompareInfo.IndexOf(authorizationHeader, "Bearer", CompareOptions.IgnoreCase) >= 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request">The request context</param>
        /// <returns></returns>    
        public static bool IsSuppressedRequest(this IOwinRequest request)
        {
            IHeaderDictionary responseHeader = request.Headers;
            if (responseHeader == null)
                return false;

            return responseHeader["Suppress-Redirect"] == "True";
        }

        /// <summary>
        /// Checks if the request is an AJAX request
        /// </summary>
        /// <param name="request">The request context</param>
        /// <returns>True if the request is an AJAX request</returns>
        public static bool IsAjaxRequest(this IOwinRequest request)
        {
            const string xmlHttpRequest = "XMLHttpRequest";

            IReadableStringCollection query = request.Query;
            IHeaderDictionary headers = request.Headers;

            string requestedWithQuery = query["X-Requested-With"] ?? string.Empty;
            string requestedWithHeaders = headers["X-Requested-With"] ?? string.Empty;

            if (query != null && requestedWithQuery.Equals(xmlHttpRequest, StringComparison.InvariantCulture))
                return true;

            return headers != null && requestedWithHeaders.Equals(xmlHttpRequest, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Checks if the request is a web sockets request
        /// </summary>
        /// <param name="context">The request context</param>
        /// <returns>True if the request is a web sockets request</returns>
        public static bool IsWebSocketRequest(this IOwinRequest context)
            => context.Path.ToString().Contains("signalr");

        /// <summary>
        /// Checks if the request is a file request
        /// </summary>
        /// <param name="context">The request context</param>
        /// <returns>True if the request is a file request</returns>
        public static bool IsFileRequest(this IOwinRequest context)
            => !string.IsNullOrEmpty(Path.GetExtension(context.Path.ToString()));
    }
}