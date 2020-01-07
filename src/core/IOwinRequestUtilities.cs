using System;
using System.Globalization;
using System.IO;

namespace Microsoft.Owin
{
    /// <summary>
    /// Useful utilities that add functionality to the <see cref="Microsoft.Owin.IOwinRequest"/> class.
    /// </summary>
    public static class OwinRequestUtilities
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
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
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsSuppressedRequest(this IOwinRequest request)
        {
            IHeaderDictionary responseHeader = request.Headers;
            if (responseHeader == null)
                return false;

            return responseHeader["Suppress-Redirect"] == "True";
        }

        /// <summary>
        /// Determines if the current request is an AJAX request.
        /// </summary>
        /// <param name="request">The current request context</param>
        /// <returns>True if the current request contains the 'X-Requested-With' key in the HEADERS config object.</returns>
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
        /// Determines if the current request is a SignalR request.
        /// </summary>
        /// <param name="context">The current request context</param>
        /// <returns>True if the current request finds 'signalr' in the request path</returns>
        internal static bool IsWebSocketRequest(this IOwinRequest context)
            => context.Path.ToString().Contains("signalr");

        /// <summary>
        /// Determines if the current request is a file request.
        /// </summary>
        /// <param name="context">The current request context</param>
        /// <returns>True if the current request contains a file extension in the request path</returns>
        internal static bool IsFileRequest(this IOwinRequest context)
            => !string.IsNullOrEmpty(Path.GetExtension(context.Path.ToString()));
    }
}