using System;
using System.Globalization;
using System.IO;

namespace Microsoft.Owin
{
    /// <summary>
    ///
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
            Microsoft.Owin.IHeaderDictionary headers = request.Headers;
            string authorizationHeader = headers["Authorization"] ?? string.Empty;

            return CultureInfo.InvariantCulture.CompareInfo.IndexOf(authorizationHeader, "Bearer", CompareOptions.IgnoreCase) >= 0; ;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>    
        public static bool IsSuppressedRequest(this IOwinRequest request)
        {
            Microsoft.Owin.IHeaderDictionary responseHeader = request.Headers;
            if (responseHeader == null)
                return false;

            return (responseHeader["Suppress-Redirect"] == "True");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>       
        public static bool IsAjaxRequest(this IOwinRequest request)
        {
            string xmlHttpRequest = "XMLHttpRequest";

            IReadableStringCollection query = request.Query;
            Microsoft.Owin.IHeaderDictionary headers = request.Headers;

            string requestedWithQuery = query["X-Requested-With"] ?? string.Empty;
            string requestedWithHeaders = headers["X-Requested-With"] ?? string.Empty;

            if (query != null && requestedWithQuery.Equals(xmlHttpRequest, StringComparison.InvariantCulture))
                return true;

            return headers != null&& requestedWithHeaders.Equals(xmlHttpRequest, StringComparison.InvariantCulture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static bool IsWebSocketRequest(this IOwinRequest context)
        {
            return context.Path.ToString().Contains("signalr");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static bool IsFileRequest(this IOwinRequest context)
        {
            string requestUriExtension = Path.GetExtension(context.Path.ToString());
            return !string.IsNullOrEmpty(requestUriExtension);
        }
    }
}