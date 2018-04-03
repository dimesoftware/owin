using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.Owin
{
    /// <summary>
    ///
    /// </summary>
    public static class OwinContextUtilities
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool SupportsEncoding(this IOwinContext context, string encoding)
        {
            IEnumerable<string> acceptedEncoding = context.Request.Headers["Accept-Encoding"]?.Split(',');
            return acceptedEncoding.Any(x => string.Equals(x, encoding, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this IOwinContext context)
            => context.Request.Headers.ContainsKey("X-Requested-With")
               && context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsWebSocketRequest(this IOwinContext context)
            => context.Request.Path.ToString().Contains("signalr");

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsFileRequest(this IOwinContext context)
        {
            string requestUriExtension = Path.GetExtension(context.Request.Path.ToString());
            return !string.IsNullOrEmpty(requestUriExtension);
        }
    }
}